using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int upOrdown;
    public int type; // 0:노트x 1:노트 2:양쪽노트 4:롱노트토글 5:롱노트 진행
    public float speed;

    public void goleft()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(-1 * speed,0,0);
    }
    
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.tag == "EndLine")
    //    {
    //        ControlManager.instance.GetThisEnemy(gameObject, type);

    //        ControlManager.instance.judge = ControlManager.judges.MISS;
    //        ControlManager.instance.ProcessJudge(ControlManager.judges.MISS);
    //        ControlManager.instance.DestroyCombo();
    //        PlayerInfo.instance.GetDamage(damage);
    //        gameObject.SetActive(false);
    //    }
    //    else if (col.tag == "MissLine")
    //    {
    //        ControlManager.instance.GetThisEnemy(gameObject, type);
    //        ControlManager.instance.judge = ControlManager.judges.MISS;
    //    }
    //    else if (col.tag == "HitLine")
    //    {
    //        ControlManager.instance.GetThisEnemy(gameObject, type);
    //        ControlManager.instance.judge = ControlManager.judges.GOOD;
    //    }
    //    else if (col.tag == "PerfectLine")
    //    {
    //        ControlManager.instance.GetThisEnemy(gameObject, type);
    //        ControlManager.instance.judge = ControlManager.judges.PERFECT;
    //    }
    //}
}