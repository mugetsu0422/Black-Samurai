using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusBoard : MonoBehaviour
{
    public Text normalEssence;
    public Text pureHeart;
    public Text ATK;
    public Text MP;

    public Text normalAtk;
    public Text specialAtk1;
    public Text specialAtk2;
    public GameObject statusBoard;
    // Start is called before the first frame update

    void Start()
    {
        getInformation();
    }

    // Update is called once per frame
    void Update()
    {
        getInformation();
        if (Input.GetKeyDown(KeyCode.I)){
            if(!statusBoard.activeInHierarchy){
                statusBoard.SetActive(true);
            }
            else{
                statusBoard.SetActive(false);
            }
        }
    }

    public void getInformation(){
        normalEssence.text = "x" + KaguraBachiData.ParasiteEssence;
        pureHeart.text = "x" + KaguraBachiData.PureParasiteHeart;
        ATK.text = KaguraBachiData.Atk.ToString();
        MP.text = KaguraBachiData.Ki + "/" + KaguraBachiData.MaxKi;
        normalAtk.text = KaguraBachiData.Atk.ToString();
        specialAtk1.text = (KaguraBachiData.Atk * KaguraBachiData.AttackMultiplier.Special1).ToString();
        specialAtk2.text = (KaguraBachiData.Atk * KaguraBachiData.AttackMultiplier.Special2).ToString();
    }
    
}
