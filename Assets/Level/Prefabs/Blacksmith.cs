using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && playerIsClosed){
            if(DialogueContainer.activeInHierarchy){
                ZeroText();
            }
            else{
                DialogueContainer.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    public void ZeroText(){
        dialogueText.text = "";
        index=0;
        DialogueContainer.SetActive(false);
    }

    IEnumerator Typing(){
        foreach(char letter in dialogue[index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine(){
        if(index < dialogue.Length -1){
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else{
            ZeroText();
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
            ZeroText();
        }
    }
}
