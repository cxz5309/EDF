using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    public GameObject Player;

    public UISprite musicArt;
    public UILabel musicTitle;
    public UILabel musicArtist;

    public UILabel GameResult;
    public GameObject ResultEffect;

    public UILabel scoreRank;

    public UILabel score;
    public UILabel maxCombo;
    public UILabel perfectCnt;
    public UILabel goodCnt;
    public UILabel missCnt;

    public GameObject isVestScore;
    public GameObject isFullCombo;

    public GameObject petPopup;
    public UISprite petSprite;
    public UILabel petLabel;
    public GameObject TouchChk;
    Queue<string> achieveQueue = new Queue<string>();
    private bool isTweening = false;

    private void Start()
    {
        if (DataSave.instance.gameData.isGameClear)
        {
            GameClearEffect();
        }
        else {
            GameFailEffect();
        }
        FirstAchievementAndSetPetQ();
        RecursiveGetPetByQ();
        musicArt.spriteName = DataSave.instance.selectedData.musicTxt;
        musicTitle.text = DataSave.instance.parsedData.musicTitle;
        musicArtist.text = DataSave.instance.parsedData.musicArtist;
        scoreRank.text = DataSave.instance.gameData.scoreRank;
        score.text = DataSave.instance.gameData.score.ToString();
        maxCombo.text = DataSave.instance.gameData.maxCombo.ToString() + "/" + DataSave.instance.parsedData.fullComboCnt;
        perfectCnt.text = DataSave.instance.gameData.perfectCnt.ToString();
        goodCnt.text = DataSave.instance.gameData.goodCnt.ToString();
        missCnt.text = DataSave.instance.gameData.missCnt.ToString();
        if (DataSave.instance.gameData.isFullCombo) {
            isFullCombo.SetActive(true); }
        else { isFullCombo.SetActive(false); }

        if (DataSave.instance.gameData.isVestScore) {
            FirebaseParser.instance.SetVestScoreToFirebase(DataSave.instance.selectedData.musicTxt);
            isVestScore.SetActive(true);
        }
        else {isVestScore.SetActive(false);}
    }
    #region 게임클리어 이펙트
    public void GameClearEffect()
    {
        AudioManager.instance.Play("으아 넘어왔다");
        ResultEffect.SetActive(true);
        GameResult.text = "GameClear!";
        GameResult.color = Color.white;
    }
    public void GameFailEffect()
    {
        ResultEffect.SetActive(false);
        GameResult.text = "GameFail";
        GameResult.color = Color.gray;
    }
    #endregion
    
    public void FirstAchievementAndSetPetQ()
    {
        string a;
        foreach(KeyValuePair<string, object> items in DataSave.instance.dbData.haveAchieve)
        {
            if((bool)items.Value == false)//achieve상태가 false일 경우에만 [검사] (처음 한 번만 실행되도록(이 이후에는 무조건 true니까))
            {
                a = AchievementManager.instance.GameDataAchievement(items.Key, DataSave.instance.parsedData, DataSave.instance.gameData);
                if (a != null)
                {
                    achieveQueue.Enqueue(a);
                }
            }
        }
    }

    public void RecursiveGetPetByQ()
    {
        string tmpQPetName;
        if (achieveQueue.Count != 0)
        {
            tmpQPetName = achieveQueue.Dequeue();

            DataSave.instance.dbData.SetHavePet(tmpQPetName, true);
            FirebaseParser.instance.SetHavePetDBtoFirebase(tmpQPetName);

            DataSave.instance.dbData.SetHaveAchieve(tmpQPetName, true);
            FirebaseParser.instance.SetHaveAchieveToFirebase(tmpQPetName);

            petSprite.spriteName = tmpQPetName;
            petLabel.text = tmpQPetName;
            DoPetPopup();
        }
        else
        {
            return;
        }
    }

    public void DoPetPopup()
    {
        isTweening = true;
        petPopup.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        petPopup.SetActive(true);
        TouchChk.SetActive(true);
        petPopup.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isTweening = false;
        });
    }
    
    public void OnPetChkBlockClick()
    {
        if (!isTweening)
        {
            isTweening = true;
            petPopup.transform.DOScale(0f, 0.4f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                petPopup.SetActive(false);
                TouchChk.SetActive(false);
                isTweening = false;
                RecursiveGetPetByQ();
            });
        }
    }

    #region 버튼클릭
    public void OnRetryButtonClick()
    {
        AudioManager.instance.Stop(AudioManager.instance.nowSound);
        SceneManager.LoadScene("GameScene");
    }

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }
    #endregion
}
