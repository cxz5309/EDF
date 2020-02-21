using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    public static ProcessManager instance;

    public enum _Judges { Perfect, Good, Miss };
    public _Judges _judge;
    public GameObject judgeUI;
    private Animator judgeUIAnimator;
    private int combo=0;

    public GameObject comboUI;
    private Animator comboUIAnimator;

    public UILabel scoreUI;

    public UISlider HPSlider;
    public float HP;

    public GameObject FeverMap;
    public UISlider FeverSlider;
    private float fever;
    private int feverScoreMultiple = 1;

    public UISlider MusicProgressSlider;
    public float musicProgress;

    private GameObject _enemy;
    private EnemyInfo _enemyInfo;

    public bool isRightPosition(int UpOrDown)
    {
        if (_enemy != null)
        {
            if (UpOrDown == _enemy.GetComponent<EnemyInfo>().upOrdown) return true;
        }
        return false;
    }

    private void Awake()
    {
        instance = this;
    }
    public void InitProcess()
    {
        judgeUIAnimator = judgeUI.GetComponent<Animator>();
        comboUIAnimator = comboUI.GetComponent<Animator>();
        SetHPSlider();
        SetFeverSlider();
        SetScore();
        musicProgress = 0;
    }
    public void getChk(int judge, GameObject enemy)
    {
        _judge = (_Judges)judge;
        _enemy = enemy;
        _enemyInfo = _enemy.GetComponent<EnemyInfo>();
    }

    public void ProcessJudge()
    {
        judgeUI.GetComponent<UISprite>().spriteName = _judge.ToString();
        judgeUI.SetActive(true);
        judgeUIAnimator.Play("JudgeAnim", -1, 0);
        switch (_judge)
        {
            case _Judges.Good:
                DataSave.instance.gameData.goodCnt += _enemyInfo.type;
                break;
            case _Judges.Perfect:
                DataSave.instance.gameData.perfectCnt += _enemyInfo.type;
                break;
            case _Judges.Miss:
                DataSave.instance.gameData.missCnt += _enemyInfo.type;
                DestroyCombo();
                break;
        }
    }

    public void ProcessCombo()
    {
        combo += _enemyInfo.type;
        comboUI.SetActive(true);
        comboUI.GetComponent<UILabel>().text = combo + " Combo";
        comboUIAnimator.Play("ComboAnim", -1, 0);
        if (DataSave.instance.gameData.maxCombo < combo)//콤보는 스코어와 달리 도중에 최고점을 찍어야 함
        {
            DataSave.instance.gameData.maxCombo = combo;
        }
    }

    public void ProcessEffect()
    {
        GameObject hitEffect = ObjectPoolContainer.Instance.Pop("HitEffect");
        
        hitEffect.transform.position = _enemy.transform.position;
        hitEffect.SetActive(true);
    }

    public void DestroyEffect(GameObject hitEffect)
    {
        //hitEffect.SetActive(false);
        //ObjectPoolContainer.Instance.Return(hitEffect);
    }

    public void DestroyEnemy()
    {
        _enemy.SetActive(false);
        ObjectPoolContainer.Instance.Return(_enemy);
        ControlManager.instance.canActive = false;
    }

    public void DestroyCombo()
    {
        combo = 0;
    }

    public void SetScore()
    {
        scoreUI.text = "0";
    }

    public void ProcessScore()
    {
        switch (_judge)
        {
            case _Judges.Perfect:
                DataSave.instance.gameData.score += 100 * _enemyInfo.type * feverScoreMultiple;
                break;
            case _Judges.Good:
                DataSave.instance.gameData.score += 80 * _enemyInfo.type * feverScoreMultiple;
                break;
        }
        scoreUI.text = DataSave.instance.gameData.score.ToString();
    }

    public void SetHPSlider()
    {
        HP = 1;
        HPSlider.value = HP;
    }

    public void ProcessHP()
    {
        switch (_judge)
        {
            case _Judges.Perfect:
            case _Judges.Good:
                HP += 0.05f;
                break;
            case _Judges.Miss:
                HP -= 0.2f;
                break;
        }
        if (HP > 1) HP = 1;
        if (HP < 0) HP = 0;
        HPSlider.value = HP;
    }

    public void OnHPSliderValZero()
    {
        if (HPSlider.value <= 0)
        {
            AudioManager.instance.Stop(AudioManager.instance.nowSound);
            GameManager.instance.GameFail();
        }
    }

    public void SetFeverSlider()
    {
        fever = 0;
        FeverSlider.value = fever;
    }

    public void ProcessFever()
    {
        switch (_judge)
        {
            case _Judges.Perfect:
            case _Judges.Good:
                fever += 0.02f;
                break;
        }
        if (fever > 1) fever = 1;
        if (fever < 0) fever = 0;
        FeverSlider.value = fever;
    }

    public void OnFeverSliderValMax()
    {
        if (FeverSlider.value >= 1)
        {
            StartCoroutine(coFeverTime());
        }
    }

    IEnumerator coFeverTime()
    {
        AudioManager.instance.Play("피버타이무다");
        FeverMap.SetActive(true);
        feverScoreMultiple = 2;
        fever = 0;
        yield return new WaitForSeconds(8f);
        FeverMap.SetActive(false);
        feverScoreMultiple = 1;
        yield break;
    }

    public void SetMusicProgress()
    {
        musicProgress = 0;
        MusicProgressSlider.value = musicProgress;
    }

    public void ProcessMusicProgress()
    {
        StartCoroutine("coProcessMusicProgress");
    }

    public void StopProcessMusicProgress()
    {
        StopCoroutine("coProcessMusicProgress");
    }

    IEnumerator coProcessMusicProgress()
    {
        //float time = 0;
        //while (musicProgress < 1)
        //{
        //    time += Time.deltaTime;
        //    if (time >= 0.1f)
        //    {
        //        musicProgress += 0.1f / NoteManager.instance.musicTime;
        //        MusicProgressSlider.value = musicProgress;
        //        time = 0;
        //    }
        //    yield return null;
        //}
        while (musicProgress < 1)
        {
            musicProgress += 0.25f / NoteManager.instance.musicTime;
            MusicProgressSlider.value = musicProgress;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
