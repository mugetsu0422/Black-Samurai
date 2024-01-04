using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenProgressBar : MonoBehaviour
{
    public static LoadingScreenProgressBar Instance { get; private set; }
    public Image currentLoadingPercent;
    public Image loadingIndicativeIcon;
    float iconStartingPosX;
    public bool isLoading = false;
    float current;

    void Awake()
    {
        Instance = this;
        iconStartingPosX = loadingIndicativeIcon.transform.localPosition.x;
        gameObject.transform.parent.GetComponent<Canvas>().sortingOrder = 100;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLoadingPercent.fillAmount = 0;
    }

    public void SetValue(float target)
    {
        current = target;
    }

    private void Update()
    {
        if (!isLoading)
        {
            return;
        }
        var amount = Mathf.MoveTowards(currentLoadingPercent.fillAmount, current, 3 * Time.deltaTime);
        currentLoadingPercent.fillAmount = amount;
        float iconPosition = amount * currentLoadingPercent.rectTransform.rect.width + iconStartingPosX;
        loadingIndicativeIcon.transform.localPosition = new Vector3(iconPosition, loadingIndicativeIcon.transform.localEulerAngles.y, 0);
    }
}
