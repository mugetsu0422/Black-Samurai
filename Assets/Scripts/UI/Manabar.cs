using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manabar : MonoBehaviour
{
    public Image currentManaBar;
    public float fillSpeed = 0.5f;
    public static Manabar instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentManaBar.fillAmount = 0;
    }
    public void setFillAmount(float manaChange){
        float newFillAmount = currentManaBar.fillAmount + (float) (manaChange * 0.01);
        currentManaBar.fillAmount = Mathf.Clamp01(newFillAmount);
        
    }
}
