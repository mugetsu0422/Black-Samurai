using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DialogueContainer;
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClosed;

    private Coroutine cor = null;

    public GameObject talktome;

    public GameObject Q;
    public GameObject A;
    public GameObject Z;
    public GameObject X;
    public GameObject C;

    public GameObject left;
    public GameObject right;
    public GameObject space;

    public GameObject teleport;
    public GameObject startScene;

    // Start is called before the first frame update
    void Start()
    {
        DialogueContainer.SetActive(false);
        startScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!DialogueContainer.activeInHierarchy){
            talktome.SetActive(true);
        }
        else talktome.SetActive(false);
        if(Input.GetKeyDown(KeyCode.Q)){
            Q.SetActive(false);
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
        foreach(char letter in dialogue[index].ToCharArray()){
            if (char.IsDigit(letter))
            {
                dialogueText.text += "<color=yellow>" + letter + "</color>";
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
        if(index < dialogue.Length - 1){
            index++;
            StopCoroutine(cor);
            dialogueText.text = "";
            cor  = StartCoroutine(Typing());
            if(index == 3){
                left.SetActive(true);
                right.SetActive(true);
            }
            else{
                left.SetActive(false);
                right.SetActive(false);
            }
            if(index == 4){
                space.SetActive(true);
            }
            else space.SetActive(false);
            if(index == 8 || index == 9 || index == 10){
                Z.SetActive(true);
                X.SetActive(true);
                C.SetActive(true);
            }
            else{
                Z.SetActive(false);
                X.SetActive(false);
                C.SetActive(false);
            }
            if(index == 12){
                KaguraBachiData.Health += -1;
                Healthbar.instance.setFillAmount(-1);
            }
            if(index == 9 || index == 14){
                KaguraBachiData.Ki = 100;
                Manabar.instance.setFillAmount(100);
            }
            if(index == 14){
                A.SetActive(true);
            }
            else A.SetActive(false);
            if (index == 16) {
                StartCoroutine(waitForOpenStartScene());
            }
            if(index == 17){
                KaguraBachiData.Ki = 0;
                KaguraBachiData.Health = 3;
                Manabar.instance.setFillAmount(-100);
                Healthbar.instance.setFillAmount(3);
                teleport.SetActive(true);
            }
            else teleport.SetActive(false);
        }
    }

    IEnumerator waitForOpenStartScene()
    {
        yield return new WaitForSeconds(5);
        startScene.SetActive(true);
        StartCoroutine(closeStartScene());
    }

    IEnumerator closeStartScene()
    {
        yield return new WaitForSeconds(17);
        startScene.SetActive(false);
        NextLine();
    }
}
