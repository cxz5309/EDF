using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeInfo : MonoBehaviour
{
    public enum Judges { Perfect, Good, Miss };
    public Judges judge;
    private void OnTriggerEnter(Collider other)
    {
        ControlManager.instance.canActive = true;

        ProcessManager.instance.getChk((int)judge, other.gameObject);

        if (judge == Judges.Miss)
        {
            ProcessManager.instance.DestroyCombo();
            ProcessManager.instance.ProcessJudge();
            ProcessManager.instance.ProcessFever();
            ProcessManager.instance.ProcessScore();
            ProcessManager.instance.DestroyEffect(other.gameObject);
            ProcessManager.instance.ProcessHP();
            ProcessManager.instance.DestroyEnemy();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ControlManager.instance.canActive = true;
        ProcessManager.instance.getChk((int)judge, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        ControlManager.instance.canActive = false;
    }
}
