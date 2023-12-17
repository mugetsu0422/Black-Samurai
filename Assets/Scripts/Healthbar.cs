using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image totalHealthBar;
    public Image HeartCurrent;
    public static Healthbar instance { get; private set; }
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    public void setFillAmount(float healthChange){
        float newFillAmount = HeartCurrent.fillAmount + (float) (healthChange * 0.33);
        HeartCurrent.fillAmount = Mathf.Clamp01(newFillAmount);
    }
}
