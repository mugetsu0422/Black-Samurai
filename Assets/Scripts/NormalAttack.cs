using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    Animator animator;
    initgame initgame;
    void Awake()
    {
        animator = GetComponent<Animator>();
        initgame = GameObject.Find("InitGame").GetComponent<initgame>();
        if (initgame)
        {
            transform.localScale = 4 * initgame.player_scale;
        }
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
