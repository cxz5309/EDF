using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TmpGameManager : MonoBehaviour
{
    public static TmpGameManager instance;

    public GameObject TmpRed;
    public GameObject TmpBlue;
    public GameObject TmpGreen;
    public GameObject TmpPerple;

    public GameObject HitEffect;

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

        ObjectPoolContainer.Instance.CreateObjectPool("HitEffect", HitEffect, 120);
    }

    private void Awake()
    {
        instance = this;
        InitEnemyObjectPool();
        InitObjectPool();
        DataSave.instance.InitAllDataNull();
        TmpNote.instance.InitNote();
        FirstStart();
    }

    public void FirstStart()
    {
        Invoke("NoteCorutine", 1f);//게임시작하고 대기 1초
    }

    public void NoteCorutine()
    {
        StartCoroutine("coNoteStart");
        //StartCoroutine("coNoteEnd");
    }

    public void StopNoteCoroutine()
    {
        StopCoroutine("coNoteStart");
        //StopCoroutine("coNoteEnd");
    }

    IEnumerator coNoteStart()
    {
        NoteStart();//노트 시작하고 텍스트파일 명시된 musicTime만큼 노트 진행
        yield return new WaitForSeconds(TmpNote.instance.enemyReachTime);
        BpmAnalyzer.instance.SetTimer1();
        TmpAudioManager.instance.Play(TmpNote.instance.backgroundSoundName);
    }

    IEnumerator coNoteEnd()
    {
        yield return new WaitForSeconds(TmpNote.instance.musicTime);//musicTime 이후 노트종료후 음악꺼지고 뮤직스탑
        TmpNote.instance.NoteEnd();
        TmpAudioManager.instance.Stop(TmpAudioManager.instance.nowSound);
        yield return new WaitForSeconds(5f);//대기 5초 후 결과씬 이동
        GameClear();
    }

    public void NoteStart()
    {
        TmpNote.instance.NoteStart();
    }


    public void GameClear()
    {
        Debug.Log("게임 종료");
        Time.timeScale = 0;
    }
    
   
    private void OnDisable()
    {
        instance = null;
    }
}
