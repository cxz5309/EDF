using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartPNManager : MonoBehaviour
{
    public static StartPNManager instance;

    string selMusicName;
    int selLevel;
    public GameObject[] ToggleButton = new GameObject[3];
    public GameObject MusicSelPanel;
    public GameObject TouchChk;
    public GameObject AlertPanel;

    public UILabel VestScoreText;
    Transform musicSelObject;
    string musicTitle;
    string musicArtist;

    private void Awake()
    {
        instance = this;
    }

    public void SetMusicSelObject()
    {
        foreach(KeyValuePair<string, object> items in DataSave.instance.dbData.musicVestScore)
        {
            FindHierarchy(items.Key);
            musicSelObject.Find("Artist").GetComponent<UILabel>().text = musicArtist;
            musicSelObject.Find("Title").GetComponent<UILabel>().text = musicTitle;
            musicSelObject.Find("Sprite").GetComponent<UISprite>().spriteName = items.Key;
        }
    }

    public void FindHierarchy(string song) {
        string[] a = song.Split('_');
        musicArtist = a[0];
        musicTitle = a[1];
        musicSelObject = MusicSelPanel.transform.Find(song);
    }

    public void ElseRadioButtonActive()
    {
        for (int i = 0; i < ToggleButton.Length; i++)
        {
            if (ToggleButton[i].GetComponent<UIToggle>().value == false)
            {
                ToggleButton[i].GetComponentInChildren<UISprite>().color = Color.white;
            }
        }
    }

    public void OnMusicSelLevelButtonClick(string _level)//레벨 버튼 클릭
    {
        UIButton.current.GetComponent<UIToggle>().value = true;
        UIButton.current.GetComponentInChildren<UISprite>().color = Color.gray;
        ElseRadioButtonActive();
        selLevel = int.Parse(_level);
    }

    public void OnMusicSel(Transform selMusicTr)//스와이프후 middle의 선택된 곡
    {
        selMusicName = selMusicTr.name;
        VestScoreText.text = "VextScore : " + DataSave.instance.dbData.musicVestScore[selMusicTr.name];
    }

    public void OnMusicSelStartButtonClick()//게임시작 버튼 클릭
    {
        if (selMusicName == null || selLevel == 0)
        {
            AlertTween(true);//곡과 레벨을 골라라 버튼 생성
            TouchChk.SetActive(true);
        }
        else
        {
            AudioManager.instance.Stop("Stay Around");

            DataSave.instance.selectedData.SetSelectedData(selMusicName, selLevel);//게임시작시 실행, db, 스테이지, txt파일을 읽을 musicTitle, level 을 datasave쪽에 저장
            SceneManager.LoadScene("GameScene");//게임 시작
        }
    }

    public void OnMusicSelBackButtonClick()//뒤로 버튼 클릭
    {
        LobbyManager.instance.canSwipe = true;
        LobbyManager.instance.StartPanel.SetActive(false);
    }

    public void AlertTween(bool OnOff)
    {
        if (OnOff)//곡과 레벨을 골라라 버튼 생성
        {
            AlertPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            AlertPanel.SetActive(true);
            AlertPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        }
        else//곡과 레벨을 골라라 버튼 제거
        {
            AlertPanel.transform.DOScale(0f, 0.4f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                AlertPanel.SetActive(false);
            });
        }
    }

    public void onAlertChkBlockClick()//곡과 레벨을 골라라 버튼 클릭
    {
        AlertTween(false);
        TouchChk.SetActive(false);
    }

    private void OnDisable()
    {
        instance = null;
    }
}
