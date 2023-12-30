using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenProgressBar : MonoBehaviour
{
    public static LoadingScreenProgressBar Instance { get; private set; }
    public Image currentLoadingPercent;
    public Image loadingIndicativeIcon;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLoadingPercent.fillAmount = 0;
    }
}
