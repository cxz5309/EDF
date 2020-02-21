using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicSelSwipeManager : MonoBehaviour
{
    public static MusicSelSwipeManager instance;

    public Transform[] popupTrArr;
    public GameObject touchBlockObj;

    float startTime;
    bool isPress = false;

    int start = 0;
    int left = 1;
    int middle = 2;
    int right = 3;
    int end = 4;
    int popupLength;

    private void Awake()
    {
        instance = this;
        popupLength = popupTrArr.Length;
    }
    public void OnPress()
    {
        if (isPress == false)
        {
            startTime = Time.time;
            isPress = true;
        }
    }

    public void OnRelease()
    {
        if (Mathf.Abs(UICamera.currentTouch.totalDelta.y) <= 70 &&
                Time.time - startTime < 0.5f)
        {
            if (UICamera.currentTouch.totalDelta.x < -150)
            {
                SwipeToLeft();
            }
            else if (UICamera.currentTouch.totalDelta.x > 150)
            {
                SwipeToRight();
            }
        }
        isPress = false;
    }

    public void SwipeToLeft()
    {
        touchBlockObj.SetActive(true);
        popupTrArr[middle].GetComponentInChildren<UISprite>().color = Color.white;
        popupTrArr[(end + 1 + popupLength) % popupLength].localPosition = new Vector2(800, 0);
        popupTrArr[left].DOLocalMoveX(-800, 0.5f);
        popupTrArr[middle].DOLocalMoveX(-400, 0.5f);
        popupTrArr[right].DOLocalMoveX(0, 0.5f);
        popupTrArr[end].DOLocalMoveX(400, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            touchBlockObj.SetActive(false);
        });
        start = (start + 1 + popupLength) % popupLength;
        left = (left + 1 + popupLength) % popupLength;
        right = (right + 1 + popupLength) % popupLength;
        middle = (middle + 1 + popupLength) % popupLength;
        end = (end + 1 + popupLength) % popupLength;
        StartPNManager.instance.OnMusicSel(popupTrArr[middle]);
        popupTrArr[middle].GetComponentInChildren<UISprite>().color = Color.gray;
    }
    public void SwipeToRight()
    {
        touchBlockObj.SetActive(true);
        popupTrArr[middle].GetComponentInChildren<UISprite>().color = Color.white;
        popupTrArr[(start - 1 + popupLength) % popupLength].localPosition = new Vector2(-800, 0);
        popupTrArr[start].DOLocalMoveX(-400, 0.5f);
        popupTrArr[left].DOLocalMoveX(0, 0.5f);
        popupTrArr[middle].DOLocalMoveX(400, 0.5f);
        popupTrArr[right].DOLocalMoveX(800, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            touchBlockObj.SetActive(false);
        });
        start = (start - 1 + popupLength) % popupLength;
        left = (left - 1 + popupLength) % popupLength;
        right = (right - 1 + popupLength) % popupLength;
        middle = (middle - 1 + popupLength) % popupLength;
        end = (end - 1 + popupLength) % popupLength;
        StartPNManager.instance.OnMusicSel(popupTrArr[middle]);
        popupTrArr[middle].GetComponentInChildren<UISprite>().color = Color.gray;
    }

    public Transform ReturnMiddleMusicTr()
    {
        return popupTrArr[middle];
    }

    public void OnClick()
    {
        if(UIButton.current.transform == popupTrArr[middle])
        {
            popupTrArr[middle].GetComponentInChildren<UISprite>().color = Color.gray;
            StartPNManager.instance.OnMusicSel(popupTrArr[middle]);
        }
        else if(UIButton.current.transform == popupTrArr[right])
        {
            SwipeToLeft();
        }
        else if (UIButton.current.transform == popupTrArr[left])
        {
            SwipeToRight();
        }
    }

    private void OnDisable()
    {
        instance = null;
    }
}