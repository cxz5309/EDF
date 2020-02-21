using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    private bool isExpandMenu = false;
    public BoxCollider[] buttonColliders;
    public Vector3[] buttonPosArray;
    private bool isMovingMenuButton = false;
    public GameObject StartPanel;

    public SwipeManager swipeManager;
    public bool canSwipe = true;

    public GameObject keyLabel;
    public bool onKeyInfo = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {        
        PetPNManager.instance.InitPetInfomation();
        AchievePNManager.instance.InitAchievementPN();
        AudioManager.instance.InitAudioManager();
        AudioManager.instance.Play("Stay Around");
        StartPNManager.instance.SetMusicSelObject();
    }

    #region Click Events
    //StartPanel 클릭이벤트
    public void OnMenuButtonClick ()
    {
        ////AsyncOperation async = Application.LoadLevelAsync("MyBigLevel");
        if (isMovingMenuButton)
            return;
        isMovingMenuButton = true;

        switch (isExpandMenu)
        {
            case true:
                isExpandMenu = false;
                for (int i = 0; i < buttonColliders.Length; i++)
                {
                    buttonColliders[i].transform.Find("Label").gameObject.SetActive(false);
                    buttonColliders[i].enabled = false;
                    buttonColliders[i].transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        isMovingMenuButton = false;
                    });
                }

                break;
            case false:
                isExpandMenu = true;
                int indexCount = 0;
                if (!canSwipe)
                {
                    isMovingMenuButton = false;
                    return;
                }
                for (int i = 0; i < buttonColliders.Length; i++)
                {
                    buttonColliders[i].transform.Find("Label").gameObject.SetActive(true);
                    buttonColliders[i].enabled = false;
                    buttonColliders[i].transform.DOLocalMove(buttonPosArray[i], 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        indexCount++;
                        if (indexCount == buttonColliders.Length-1)
                        {
                            for (int j = 0; j < buttonColliders.Length; j++)
                            {
                                buttonColliders[j].enabled = true;
                                isMovingMenuButton = false;
                            }
                        }
                    });
                }
                break;
        }
    }

    public void OnStartButtonClick()
    {
        canSwipe = false;
        DataSave.instance.selectedData.InitSelectedData();
        if (isExpandMenu)
        {
            OnMenuButtonClick();
        }
        StartPanelTween();
    }

    public void OnKeyButtonClick()
    {
        if (!onKeyInfo)
        {
            onKeyInfo = true;
            keyLabel.SetActive(true);
        }
        else
        {
            onKeyInfo = false;
            keyLabel.SetActive(false);
            }
        }

    public void StartPanelTween()
    {
        StartPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        StartPanel.SetActive(true);
        StartPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
    }
    #endregion

    private void OnDisable()
    {
        instance = null;
    }
}
