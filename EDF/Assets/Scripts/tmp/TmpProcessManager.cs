using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpProcessManager : MonoBehaviour
{
    public static TmpProcessManager instance;

    public enum _TmpJudges { Perfect, Good, Miss };
    public _TmpJudges _judge;
    public GameObject judgeUI;
    private Animator judgeUIAnimator;
    private int combo = 0;

    public GameObject comboUI;
    private Animator comboUIAnimator;

    public UILabel scoreUI;
    
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
    private void Start()
    {
        judgeUIAnimator = judgeUI.GetComponent<Animator>();
        comboUIAnimator = comboUI.GetComponent<Animator>();
    }
    public void getChk(int judge, GameObject enemy)
    {
        _judge = (_TmpJudges)judge;
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
            case _TmpJudges.Good:
                DataSave.instance.gameData.goodCnt += _enemyInfo.type;
                break;
            case _TmpJudges.Perfect:
                DataSave.instance.gameData.perfectCnt += _enemyInfo.type;
                break;
            case _TmpJudges.Miss:
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
        if (DataSave.instance.gameData.maxCombo < combo)
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

    public void DestroyEnemy()
    {
        _enemy.SetActive(false);
        ObjectPoolContainer.Instance.Return(_enemy);
    }

    public void DestroyCombo()
    {
        combo = 0;
    }

    public void ProcessScore()
    {
        switch (_judge)
        {
            case _TmpJudges.Perfect:
                DataSave.instance.gameData.score += 100 * _enemyInfo.type;
                break;
            case _TmpJudges.Good:
                DataSave.instance.gameData.score += 80 * _enemyInfo.type;
                break;
        }
        scoreUI.text = DataSave.instance.gameData.score.ToString();
    }

    
    private void OnDestroy()
    {
        instance = null;
    }
}
