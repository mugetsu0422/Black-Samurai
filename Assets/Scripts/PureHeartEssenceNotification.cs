using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PureHeartEssenceNotification : MonoBehaviour
{
    public Image HeartEssence;
    public Text Noti;
    public Image NotiContainer;

    public GameObject Notification;

    public static PureHeartEssenceNotification instance { get; private set; }

    AudioSource audioSource;

    public AudioClip collectSound;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void openNotification(){
        gameObject.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeNotification());
    }

    IEnumerator FadeNotification()
    {
        Color containerColor = NotiContainer.color;
        Color HeartEssenceColor = HeartEssence.color;
        Color NotiColor = Noti.color;

        float duration = 0.2f;
        float elapsedTime = 0f;
        PlaySound(collectSound);
        while (elapsedTime < duration)
        {
            containerColor.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            HeartEssenceColor.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            NotiColor.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);

            NotiContainer.color = containerColor;
            HeartEssence.color = HeartEssenceColor;
            Noti.color = NotiColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        containerColor.a = 0f;
        HeartEssenceColor.a = 0f;
        NotiColor.a = 0f;

        NotiContainer.color = containerColor;
        HeartEssence.color = HeartEssenceColor;
        Noti.color = NotiColor;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
