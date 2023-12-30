using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] GameObject loadingScreen;
    float target;
    bool isLoading = false;
    float iconStartingPosX;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            iconStartingPosX = LoadingScreenProgressBar.Instance.loadingIndicativeIcon.transform.localPosition.x;
        }
    }

    public async void LoadScene(string sceneName)
    {
        target = 0;
        LoadingScreenProgressBar.Instance.currentLoadingPercent.fillAmount = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        isLoading = true;

        do
        {
            await Task.Delay(200);
            target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
        isLoading = false;
    }

    private void Update()
    {
        if (!isLoading)
        {
            return;
        }
        var amount = Mathf.MoveTowards(LoadingScreenProgressBar.Instance.currentLoadingPercent.fillAmount, target, 3 * Time.deltaTime);
        LoadingScreenProgressBar.Instance.currentLoadingPercent.fillAmount = amount;
        float iconPosition = amount * LoadingScreenProgressBar.Instance.currentLoadingPercent.rectTransform.rect.width + iconStartingPosX;
        LoadingScreenProgressBar.Instance.loadingIndicativeIcon.transform.localPosition = new Vector3(iconPosition, LoadingScreenProgressBar.Instance.loadingIndicativeIcon.transform.localEulerAngles.y, 0);
    }
}
