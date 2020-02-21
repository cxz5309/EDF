using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwipeManager : MonoBehaviour
{
    public Transform[] popupTrArr;
    public GameObject touchBlockObj;

    float startTime;
    bool isPress = false;

    int left = -1;
    int middle = 0;
    int right = 1;

    public GameObject[] buttonLabel = new GameObject[4];
    public int a = 0;
    public int b = 1;
    public int c = 2;
    public int d = 3;
    public int e = 4;

    public void OnPress()
    {
        if (LobbyManager.instance.canSwipe)
        {
            if (isPress == false)
            {
                startTime = Time.time;
                isPress = true;
            }
        }
    }

    public void OnRelease()
    {
        if (LobbyManager.instance.canSwipe)
        {
            if (Mathf.Abs(UICamera.currentTouch.totalDelta.y) <= 70 &&
                    Time.time - startTime < 0.5f)
            {
                if (UICamera.currentTouch.totalDelta.x < -150)
                {
                    SwipeToLeft(-1);
                }
                else if (UICamera.currentTouch.totalDelta.x > 150)
                {
                    SwipeToRight(-1);
                }
            }
            isPress = false;
        }
    }
    public void SwipeToLeft(int Location)//-1일 경우 한칸 움직임
    {
        while (true)
        {
            if (middle == popupTrArr.Length-1) break;
            touchBlockObj.SetActive(true);
            popupTrArr[middle].DOLocalMoveX(-1280, 0.5f);
            popupTrArr[right].DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                touchBlockObj.SetActive(false);
            });
            left++;
            right++;
            middle++;
            if (middle == Location || Location == -1) break;
        }
    }
    public void SwipeToRight(int Location)//-1일 경우 한칸 움직임
    {
        while (true)
        {
            if (middle == 0) break;
            touchBlockObj.SetActive(true);
            popupTrArr[middle].DOLocalMoveX(1280, 0.5f);
            popupTrArr[left].DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                touchBlockObj.SetActive(false);
            });
            left--;
            right--;
            middle--;
            if (middle == Location || Location == -1) break;
        }
    }
   
    public void OnMenuButtonClick(int i)
    {
        if (middle != i)
        {
            if (middle < i)
            {
                SwipeToLeft(i);
            }
            else
            {
                SwipeToRight(i);
            }
        }
    }
}