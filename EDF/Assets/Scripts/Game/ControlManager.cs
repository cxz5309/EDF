using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;

    public GameObject player;
    public GameObject pet;

    public bool canActive;
    private bool doOnceOnUpdate;
    private int aniSet = 0;

    private Touch[] myTouches;
    private Vector3 firstTouchedPos;
    private Vector3 secondTouchedPos;
    private bool touchOn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canActive = false;
        doOnceOnUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            myTouches = Input.touches;

            for (int i = 0; i < Input.touchCount; i++)
            {
                firstTouchedPos = myTouches[0].position;
                secondTouchedPos = myTouches[1].position;

                if (firstTouchedPos.x < 640 && myTouches[0].phase == TouchPhase.Began)
                {
                    pet.GetComponent<PetInfo>().PlayPetAttack();
                    if (canActive && ProcessManager.instance.isRightPosition(0))
                    {
                        TouchProcess();
                    }
                }
                if (firstTouchedPos.x >= 640 && myTouches[0].phase == TouchPhase.Began)
                {
                    switch (aniSet)
                    {
                        case 0:
                            player.GetComponent<PlayerInfo>().PlayPunch1();
                            break;
                        case 1:
                            player.GetComponent<PlayerInfo>().PlayPunch2();
                            break;
                        case 2:
                            player.GetComponent<PlayerInfo>().PlayKick();
                            break;
                    }
                    aniSet = (aniSet + 1) % 3;
                    if (canActive && ProcessManager.instance.isRightPosition(1))
                    {
                        TouchProcess();
                    }
                }
                if(myTouches[0].phase == TouchPhase.Ended)
                {
                    doOnceOnUpdate = true;
                }

                if(Input.touchCount > 1)
                {
                    if(Mathf.Abs(firstTouchedPos.x - secondTouchedPos.x) >= 640 && myTouches[0].phase == TouchPhase.Stationary && myTouches[1].phase == TouchPhase.Stationary)
                    {
                        if (doOnceOnUpdate)
                        {
                            AudioManager.instance.Play("하");
                            player.GetComponent<PlayerInfo>().PlayJump();
                            pet.GetComponent<PetInfo>().PlayPetSkill();

                            if (canActive && ProcessManager.instance.isRightPosition(2))
                            {
                                doOnceOnUpdate = false;
                                TouchProcess();
                            }
                        }
                    }
                }
            }
        }
#endif

#region UNITY & PC
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pet.GetComponent<PetInfo>().PlayPetAttack();
            if (canActive && ProcessManager.instance.isRightPosition(0))
            {
                TouchProcess();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            switch (aniSet)
            {
                case 0:
                    player.GetComponent<PlayerInfo>().PlayPunch1();
                    break;
                case 1:
                    player.GetComponent<PlayerInfo>().PlayPunch2();
                    break;
                case 2:
                    player.GetComponent<PlayerInfo>().PlayKick();
                    break;
            }
            aniSet = (aniSet + 1) % 3;
            if (canActive && ProcessManager.instance.isRightPosition(1))
            {
                TouchProcess();
            }
        }

        if(Input.GetKey(KeyCode.Q)&&Input.GetKey(KeyCode.P))
        {
            if (doOnceOnUpdate)
            {
                AudioManager.instance.Play("하");
                player.GetComponent<PlayerInfo>().PlayJump();
                pet.GetComponent<PetInfo>().PlayPetSkill();

                if (canActive && ProcessManager.instance.isRightPosition(2)) { 
                    doOnceOnUpdate = false;
                    TouchProcess();
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Q)|| Input.GetKeyUp(KeyCode.P))
        {
            doOnceOnUpdate = true;
        }
#endregion
    }

    private void TouchProcess()
    {
        ProcessManager.instance.ProcessCombo();
        ProcessManager.instance.ProcessJudge();
        ProcessManager.instance.ProcessFever();
        ProcessManager.instance.ProcessScore();
        ProcessManager.instance.ProcessEffect();
        ProcessManager.instance.ProcessHP();
        ProcessManager.instance.DestroyEnemy();
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
