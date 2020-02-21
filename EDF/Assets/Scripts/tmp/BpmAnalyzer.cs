using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BpmAnalyzer : MonoBehaviour
{
    public static BpmAnalyzer instance;

    private float _BPM;
    private float _beatPerSecond;
    private float _timePerBeat;
    private float _enemyTime;
    public int allNotes;
    public UILabel bpmText;
    public UILabel beatPerSecondText;
    public UILabel timePerBeatText;
    public UILabel enemyDefaultSpeedText;
    public UILabel enemySpeedText;
    public UILabel enemyTimeText;
    public UILabel enemyReachTimeText;
    public UILabel MusicTimeText;
    public UILabel BeatPerMusicTimeText;
    public UILabel NowBeatText;
    public UILabel StartBeatText;
    public UILabel allNoteText;

    public UILabel Timer;
    public UILabel Timer1;
    public UILabel Timer2;
    float time1;
    float time2;
    float time3;

    public bool pauseOn;
    public bool isPitching;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InfoSet();
        ShowAllInfomation();
        SetTimer();
    }

    public void InfoSet()
    {
        _BPM = TmpNote.instance.bpm;
        _beatPerSecond = _BPM / 60;
        _timePerBeat = 1 / _beatPerSecond;
        _enemyTime = 1 / (_beatPerSecond * 5);
    }

    public void ShowAllInfomation()
    {
        bpmText.text = "1분당 박자 수(bpm) : " + _BPM;
        beatPerSecondText.text = "1초당 박자 수(bpm/60) : " + _beatPerSecond;
        timePerBeatText.text = "한 박자당 몇초(60/bpm) : " + _timePerBeat;
        enemyDefaultSpeedText.text = "적 스피드 기본값 : 5(아직 변수로 설정이 안됨)";
        enemySpeedText.text = "적 스피드=1초당 이동거리(1초당 박자 수 * 기본값) : " + (_beatPerSecond * 5);
        enemyTimeText.text = "적 이동거리당 시간(1/스피드) : " + _enemyTime;
        enemyReachTimeText.text = "적의 도달 시간(이동거리당 시간 * 15) : " + _enemyTime*15;
        MusicTimeText.text = "음악 길이 : " + TmpNote.instance.musicTime;
        BeatPerMusicTimeText.text = "음악 길이당 총 박자 수 : " + TmpNote.instance.musicTime / _timePerBeat;
        allNoteText.text = "총 노트 수 : " + allNotes;
        Timer2.text = "풀콤보 : " + TmpNote.instance.fullComboCnt;
    }

    public void ShowNowBeat(int i)
    {
        StartCoroutine(coShowNowBeat(i));
    }

    IEnumerator coShowNowBeat(int i)
    {
        StartBeatText.text = "출발한 박자 : " + (i + 1);
        yield return new WaitForSeconds(TmpNote.instance.enemyReachTime);
        NowBeatText.text = "현재 박자 : " + (i + 1);
    }

    public void OnBackButtonClick()
    {
        TmpAudioManager.instance.Stop(TmpNote.instance.backgroundSoundName);
        SceneManager.LoadScene("BeatMakeStartScene");
    }

    public void onPauseButtonClick()
    {
        if (!pauseOn)
        {
            pauseOn = true;
            TmpAudioManager.instance.SetPause(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseOn = false;
            TmpNote.instance.checkTime = 0;
            TmpAudioManager.instance.SetPause(false);
            Time.timeScale = 1;
            StopCoroutine("coBackwards");
            StopCoroutine("coFastForwards");
        }
    }

    public void onBackwardsButtonClick()
    {
        StopCoroutine("coBackwards");
        StopCoroutine("coFastForwards");

        if (!isPitching)
        {
            isPitching = true;
        }
        GameObject[] allNotes = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject a in allNotes)
        {
            a.SetActive(false);
            a.transform.position = new Vector3(-13, 0, 0);
            ObjectPoolContainer.Instance.Return(a);
        }
        TmpNote.instance.EnemyListIndexChange(-8);
        TmpAudioManager.instance.SetPause(false);

        StartCoroutine("coBackwards");
    }

    IEnumerator coBackwards()
    {
        float checkTime = 0;

        TmpAudioManager.instance.SetPitch(-8f);
        Time.timeScale = 1;

        while (checkTime < (_timePerBeat))
        {
            checkTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        TmpAudioManager.instance.SetPause(true);
        Time.timeScale = 0;
        TmpAudioManager.instance.SetPitch(1f);

        time1 -= _timePerBeat;
        time2 -= _timePerBeat;
        if (isPitching)
        {
            isPitching = false;
        }
        TmpNote.instance.checkTime = 0;
    }

    public void onFastForwardsButtonClick()
    {
        StopCoroutine("coFastForwards");
        StopCoroutine("coBackwards");

        if (!isPitching)
        {
            isPitching = true;
        }

        GameObject[] allNotes = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject a in allNotes)
        {
            a.SetActive(false);
            a.transform.position = new Vector3(-13, 0, 0);
            ObjectPoolContainer.Instance.Return(a);
        }
        TmpNote.instance.EnemyListIndexChange(8);

        TmpAudioManager.instance.SetPause(false);

        StartCoroutine("coFastForwards");
    }

    IEnumerator coFastForwards()
    {
        float checkTime = 0;

        TmpAudioManager.instance.SetPitch(8f);
        Time.timeScale = 1;

        while (checkTime < _timePerBeat)
        {
            checkTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        TmpAudioManager.instance.SetPause(true);
        Time.timeScale = 0;
        TmpAudioManager.instance.SetPitch(1f);

        time1 += _timePerBeat;
        time2 += _timePerBeat;
        if (isPitching)
        {
            isPitching = false;
        }
        TmpNote.instance.checkTime = 0;
    }
    public void SetTimer()
    {
        StartCoroutine(coTimer());
    }
    public void SetTimer1()
    {
        Debug.Log(1);
        StartCoroutine(coTimer1());
    }
    public void SetTimer2()
    {
        Debug.Log(2);
        StartCoroutine(coTimer2());
    }

    IEnumerator coTimer()
    {
        time1 = 0;
        while (true)
        {
            Timer.text = time1.ToString();
            yield return new WaitForSeconds(0.1f);
            time1 += 0.1f;
        }
    }
    IEnumerator coTimer1()
    {
        time2 = 0;
        while (true)
        {
            Timer1.text = time2.ToString();
            yield return new WaitForSeconds(0.1f);
            time2 += 0.1f;
        }
    }
    IEnumerator coTimer2()
    {
        time3 = 0;
        while (true)
        {
            Timer2.text = time3.ToString();
            yield return new WaitForSeconds(0.1f);
            time3 += 0.1f;
        }
    }
}
