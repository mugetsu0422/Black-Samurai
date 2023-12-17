using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EssenceCollected : MonoBehaviour
{
    public Text normalEssence;
    public static EssenceCollected instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNormalEssence(int pastEssence, int essenceCollected){
        int newEssence = pastEssence + essenceCollected;
        normalEssence.text = "x" + newEssence;
    }
}
