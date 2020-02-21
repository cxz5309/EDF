using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInfo : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void PlayPetAttack()
    {
        animator.Play("PetAttack", -1, 0);
    }

    public void PlayPetSkill()
    {
        animator.Play("PetSkill", -1, 0);
    }
}
