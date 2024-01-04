using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public static BossHealthbar instance { get; private set; }
    public Text bossName;
    public Image bar;
    float originalSize;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        originalSize = bar.rectTransform.rect.width;
    }

    public void SetEnable(bool value) {
       gameObject.SetActive(value);
    }

    public void SetValue(float value)
    {
        bar.fillAmount = value;
    }

    public void SetBossName(string value)
    {
        bossName.text = value;
    }
}