using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack1 : MonoBehaviour
{
    initgame initgame;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        initgame = GameObject.Find("InitGame").GetComponent<initgame>();
        if (initgame)
        {
            transform.localScale = 2 * initgame.player_scale - new Vector3(0.5f, 0.5f, 0f);
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
