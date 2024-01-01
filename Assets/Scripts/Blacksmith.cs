using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith : MonoBehaviour
{
    public GameObject DialogueContainer;
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClosed;

    public Image ParasiteEssence;
    public Image ParasiteHeart;

    private Coroutine cor = null;

    public GameObject talktome;

    AudioSource audioSource;

    public AudioClip Upgrade;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DialogueContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsClosed && !DialogueContainer.activeInHierarchy){
            talktome.SetActive(true);
        }
        else talktome.SetActive(false);
        if(Input.GetKeyDown(KeyCode.Q) && playerIsClosed){
            if(DialogueContainer.activeInHierarchy){
                ZeroText();
            }
            else{
                DialogueContainer.SetActive(true);
                dialogueText.text = "";
                cor = StartCoroutine(Typing());
            }
        }
        if(Input.GetKeyDown(KeyCode.E) && DialogueContainer.activeInHierarchy){
            NextLine();
        }
    }

    public void ZeroText(){
        dialogueText.text = "";
        index=0;
        DialogueContainer.SetActive(false);
    }

    IEnumerator Typing(){
        ParasiteEssence.gameObject.SetActive(false);
        ParasiteHeart.gameObject.SetActive(false);
        foreach(char letter in dialogue[index].ToCharArray()){
            if (char.IsDigit(letter))
            {
                dialogueText.text += "<color=yellow>" + letter + "</color>";
            }
            else if (letter == '@')
            {
                ParasiteEssence.gameObject.SetActive(true);
            }
            else if (letter == '#'){
                ParasiteHeart.gameObject.SetActive(true);
            }
            
            else
            {
                dialogueText.text += letter;
            }
            
            //dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine(){
        if(index < 3){
            index++;
            StopCoroutine(cor);
            dialogueText.text = "";
            cor  = StartCoroutine(Typing());
        }
        else if(index ==3){
            int result = UpgradeWeapon();
            if(result == 1){
                index = 4;
                StopCoroutine(cor);
                dialogueText.text = "";
                PlaySound(Upgrade);
                cor  = StartCoroutine(Typing());
            }
            if(result == 2){
                index = 5;
                StopCoroutine(cor);
                dialogueText.text = "";
                cor  = StartCoroutine(Typing());
            }
            if(result == 3){
                index = 6;
                StopCoroutine(cor);
                dialogueText.text = "";
                cor  = StartCoroutine(Typing());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            playerIsClosed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            playerIsClosed = false;
            try{
                StopCoroutine(cor);
            }
            catch{

            }
            ZeroText();
        }
    }

    private int UpgradeWeapon(){
        if(KaguraBachiData.WeaponLevel == 1){
            if(KaguraBachiData.ParasiteEssence >= 100 && KaguraBachiData.PureParasiteHeart >=1){
                KaguraBachiData.WeaponLevel = 2;
                EssenceCollected.instance.setNormalEssence(KaguraBachiData.ParasiteEssence, -100);
                KaguraBachiData.ParasiteEssence -=100;
                KaguraBachiData.PureParasiteHeart -=1;
                return 1;
            }
            else return 2;
        }
        if(KaguraBachiData.WeaponLevel == 2){
            if(KaguraBachiData.ParasiteEssence >= 100 && KaguraBachiData.PureParasiteHeart >=1){
                KaguraBachiData.WeaponLevel = 3;
                EssenceCollected.instance.setNormalEssence(KaguraBachiData.ParasiteEssence, -300);
                KaguraBachiData.ParasiteEssence -=300;
                KaguraBachiData.PureParasiteHeart -=1;
                return 1;
            }
            else return 2;
        }
        return 3;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
