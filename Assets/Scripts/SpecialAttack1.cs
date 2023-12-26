using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack1 : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDirection(float direction)
    {
        animator.SetFloat("LookX", direction);
        // Debug.Break();
    }

    void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}
