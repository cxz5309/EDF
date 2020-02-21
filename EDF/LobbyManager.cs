using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{
    private bool isExpandMenu = false;

    public BoxCollider[] buttonColliders;
    public Vector3[] buttonPosArray;

    private bool isMovingMenuButton = false;

    #region Click Events

    public void OnMenuButtonClick ()
    {
        ////AsyncOperation async = Application.LoadLevelAsync("MyBigLevel");

    
        Debug.Log(isMovingMenuButton);
        if (isMovingMenuButton)
            return;

        isMovingMenuButton = true;

        switch (isExpandMenu)
        {
            case true:
                isExpandMenu = false;
                for (int i = 0; i < buttonColliders.Length; i++)
                {
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

                for (int i = 0; i < buttonColliders.Length; i++)
                {
                    buttonColliders[i].enabled = false;
                    buttonColliders[i].transform.DOLocalMove(buttonPosArray[i], 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        indexCount++;
                        if (indexCount == buttonColliders.Length-1)
                        {
                            Debug.Log("indexCount = " + indexCount);
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





    #endregion
}
