using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpJudgeInfo : MonoBehaviour
{
    public enum TmpJudges { Perfect, Good, Miss };
    public TmpJudges judge;
    private void OnTriggerEnter(Collider other)
    {
        TmpProcessManager.instance.getChk((int)judge, other.gameObject);

        TmpProcessManager.instance.DestroyCombo();
            TmpProcessManager.instance.ProcessJudge();
            TmpProcessManager.instance.ProcessScore();
            TmpProcessManager.instance.ProcessEffect();
            TmpProcessManager.instance.DestroyEnemy();
    }
}
