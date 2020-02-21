using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AchievePNManager : MonoBehaviour
{
    public static AchievePNManager instance;

    public GameObject uiGrid;
    private int gridPos = 0;

    public bool isMoving = false;

    public UIToggle[] Container = new UIToggle[8];

    private void Awake()
    {
        instance = this;
    }

    public void InitAchievementPN()
    {
        Dictionary<string, object> dic = DataSave.instance.dbData.haveAchieve;
        for (int i = 0; i < dic.Count; i++)
        {
            Container[i].value = (bool)dic[PetPNManager.instance.allPetNames[i + 1]];
        }
    }

    public void OnToggleValueChange()
    {
        UIToggle current = UIToggle.current;
        SetContainer(current, current.value);
    }

    public void SetContainer(UIToggle uiToggle, bool toggle)
    {
        if (toggle)
        {
            uiToggle.transform.Find("ToggleOn").gameObject.SetActive(true);
            uiToggle.transform.Find("ToggleOff").gameObject.SetActive(false);
        }
        else
        {
            uiToggle.transform.Find("ToggleOn").gameObject.SetActive(false);
            uiToggle.transform.Find("ToggleOff").gameObject.SetActive(true);
        }
    }

    #region 스크롤바
    public void onScrollUpButtonClick()
    {
        if (!isMoving)
        {
            float y = uiGrid.transform.localPosition.y;

            if (gridPos > 0)
            {
                isMoving = true;

                uiGrid.transform.DOLocalMoveY(y - 120, 0.5f).OnComplete(() =>
                {
                    isMoving = false;
                });
                gridPos--;
            }

        }
    }

    public void onScrollDownButtonClick()
    {
        if (!isMoving)
        {
            float y = uiGrid.transform.localPosition.y;
            if (gridPos + 2 < 6)
            {
                isMoving = true;

                uiGrid.transform.DOLocalMoveY(y + 120, 0.5f).OnComplete(() =>
                {
                    isMoving = false;
                });
                gridPos++;
            }
        }
    }
    #endregion

    private void OnDestroy()
    {
        instance = null;
    }
}
