using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetDirection(float direction)
    {
        animator.SetFloat("LookX", direction);
    }

    void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}
