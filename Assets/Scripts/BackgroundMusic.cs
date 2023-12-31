using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource bgm;
    public AudioClip bossFightBGM;
    public AudioClip origin;

    public static BackgroundMusic instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        bgm = gameObject.GetComponent<AudioSource>();
    }

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeBossBGM(){
        StartCoroutine(changeBackgroundMusic(bossFightBGM));
    }

    public void originalBGM(){
        StartCoroutine(changeBackgroundMusic(origin));
    }

    IEnumerator changeBackgroundMusic(AudioClip clip){
        bgm.Stop();
        yield return new WaitForSeconds(2f);
        bgm.clip = clip;
        bgm.Play();
    }
}
