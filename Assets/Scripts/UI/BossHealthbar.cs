using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public static BossHealthbar instance { get; private set; }
    public Text bossName;
    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetEnable(bool value) {
       gameObject.SetActive(value);
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetBossName(string value)
    {
        bossName.text = value;
    }
}