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

    void Awake()
    {
        instance = this;
    }

    public void openNotification(){
        StartCoroutine(FadeNotification());
    }

    IEnumerator FadeNotification()
    {
        Color containerColor = NotiContainer.color;
        Color HeartEssenceColor = HeartEssence.color;
        Color NotiColor = Noti.color;

        float duration = 0.2f;
        float elapsedTime = 0f;

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
}
