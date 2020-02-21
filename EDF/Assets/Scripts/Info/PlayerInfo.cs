using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour
{
    SkeletonAnimation anim;
    Animator animator;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private void Awake()
    {
        anim = this.GetComponent<SkeletonAnimation>();
        //animator = this.GetComponent<Animator>();
        transform.position = startPoint;
    }
    public void PlayerPosition()
    {
        transform.DOMove(endPoint, 1f);
    }
    public void PlayJump()
    {
        anim.state.ClearTrack(0);

        anim.state.SetAnimation(0, "Jump", false).Complete += PlayRun;
        //animator.Play("cat_jump", -1, 0);
    }
    public void PlayPunch1()
    {
        anim.state.ClearTrack(0);

        anim.state.SetAnimation(0, "Punch1", false).Complete += PlayRun;
        //animator.Play("cat_shot", -1, 0);
    }
    public void PlayPunch2()
    {
        anim.state.ClearTrack(0);

        anim.state.SetAnimation(0, "Punch2", false).Complete += PlayRun;
        //animator.Play("cat_shot", -1, 0);
    }
    public void PlayKick()
    {
        anim.state.ClearTrack(0);

        anim.state.SetAnimation(0, "Kick1", false).Complete += PlayRun;
        //animator.Play("cat_joy", -1, 0);
    }
    private void PlayRun(Spine.TrackEntry trackEntry)
    {
        anim.state.ClearTrack(0);
        anim.state.SetAnimation(0, "Run", true);
    }
}
