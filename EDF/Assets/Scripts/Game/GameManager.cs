using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TmpRed;
    public GameObject TmpBlue;
    public GameObject TmpGreen;
    public GameObject TmpPerple;

    public GameObject HitEffect;

    public GameObject CountDown;

    public GameObject Player;

    public void InitEnemyObjectPool()
    {
        TmpRed = Resources.Load("Prefabs/EnemyPrefabs/TmpRed") as GameObject;
        TmpBlue = Resources.Load("Prefabs/EnemyPrefabs/TmpBlue") as GameObject;
        TmpGreen = Resources.Load("Prefabs/EnemyPrefabs/TmpGreen") as GameObject;
        TmpPerple = Resources.Load("Prefabs/EnemyPrefabs/TmpPerple") as GameObject;

        HitEffect = Resources.Load("Prefabs/EffectPrefabs/CFX4 Sparks Explosion B") as GameObject;
    }

    void InitObjectPool()
    {
        ObjectPoolContainer.Instance.CreateObjectPool("TmpRed", TmpRed, 120);
        ObjectPoolContainer.Instance.CreateObjectPool("TmpBlue", TmpBlue, 120);
        ObjectPoolContainer.Instance.CreateObjectPool("TmpGreen", TmpGreen, 120);
        ObjectPoolContainer.Instance.CreateObjectPool("TmpPerple", TmpPerple, 120);
        ObjectPoolContainer.Instance.CreateObjectPool("HitEffect", HitEffect, 140);
    }

    private void Awake()
    {
        instance = this;

        InitEnemyObjectPool();
        InitObjectPool();
        DataSave.instance.gameData.InitGameData();
        NoteManager.instance.InitNote(DataSave.instance.selectedData.musicTxt, DataSave.instance.selectedData.level);
        AudioManager.instance.InitAudioManager();
        ProcessManager.instance.InitProcess();
        StageManager.instance.InitStage(Player.transform);
    }

    private void Start()
    {
        //게임 시작
        FirstStart();
    }

    public void FirstStart()
    {
        Player.GetComponent<PlayerInfo>().PlayerPosition();
        Invoke("NoteCorutine", 1f);//게임시작하고 대기 1초
    }

    public void NoteCorutine()
    {
        StartCoroutine("coCountDownSound");
        CountDown.SetActive(true);//카운트다운 3.8초
        StartCoroutine("coNoteStart");
    }

    public void StopNoteCoroutine()
    {
        CountDown.SetActive(false);
        StopCoroutine("coNoteStart");
        StopCoroutine("coCountDownSound");
    }

    IEnumerator coCountDownSound()
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.Play("뿅");
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("뿅");
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("뿅");
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("삐");
    }

    IEnumerator coNoteStart()
    {
        yield return new WaitForSeconds(3.8f);
        NoteStart();//노트 시작하고 텍스트파일 명시된 musicTime만큼 노트 진행
        yield return new WaitForSeconds(NoteManager.instance.enemyReachTime);
        AudioManager.instance.Play(NoteManager.instance.backgroundSoundName);
        ProcessManager.instance.ProcessMusicProgress();
    }

    public void NoteEnd(float beatIntervalTime)
    {
        StartCoroutine(coNoteEnd(beatIntervalTime));
    }
    IEnumerator coNoteEnd(float beatIntervalTime)
    {
        yield return new WaitForSeconds(beatIntervalTime + 2f);//musicTime 이후 노트종료후 음악꺼지고 뮤직스탑
        AudioManager.instance.Stop(AudioManager.instance.nowSound);
        yield return new WaitForSeconds(5f);//대기 5초 후 결과씬 이동
        GameClear();
    }

    public void NoteStart()
    {
        CountDown.SetActive(false);
        NoteManager.instance.NoteStart();
    }

    
    public void GameClear()
    {
        SaveGameClearData(true);
        SceneManager.LoadScene("ResultScene");
    }

    public void GameFail()
    {
        SaveGameClearData(false);
        SceneManager.LoadScene("ResultScene");
    }

    public string SetRank(int _score, int _fullComboScore)
    {
        switch (10 * _score / _fullComboScore)
        {
            default:
                return "F";
            case 12:
            case 13:
            case 14:
            case 15:
                return "S+";
            case 11:
                return "S+";
            case 10:
                return "S";
            case 9:
                return "A+";
            case 8:
                return "A";
            case 7:
                return "B+";
            case 6:
                return "B";
            case 5:
                return "C";

        }
    }

    public bool IsFullCombo(int combo, int fullCombo)
    {
        if (combo == fullCombo)
            return true;
        return false;
    }

    public bool IsVestScore(string artist_music, int score, int lastScore)
    {
        if (score > lastScore)
        {
            DataSave.instance.dbData.SetMusic_VestScore(artist_music, score);
            return true;
        }
        return false;
    }

    public void SaveGameClearData(bool GameClear)
    {
        string thisArtist_Music = DataSave.instance.selectedData.musicTxt;

        int combo = DataSave.instance.gameData.maxCombo;
        int fullCombo = DataSave.instance.parsedData.fullComboCnt;

        int score = DataSave.instance.gameData.score;
        int lastScore = 0;
        if (DataSave.instance.dbData.musicVestScore[thisArtist_Music] != null)
        {
            lastScore = Convert.ToInt32(DataSave.instance.dbData.musicVestScore[thisArtist_Music]);
        }

        if (GameClear)
        {
            DataSave.instance.gameData.isGameClear = true;
            DataSave.instance.gameData.scoreRank = SetRank(score, fullCombo * 100);//랭크
            DataSave.instance.gameData.isFullCombo = IsFullCombo(combo, fullCombo);//풀콤보인지 아닌지
            DataSave.instance.gameData.isVestScore = IsVestScore(thisArtist_Music, score, lastScore);//최고점수인지 아닌지, 최고점수 DataSave에 등록
        }
        else {
            DataSave.instance.gameData.isGameClear = false;
            DataSave.instance.gameData.scoreRank = "F";
        }
    }

    private void OnDisable()
    {
        instance = null;
    }
}
