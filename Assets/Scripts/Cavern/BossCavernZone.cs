using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCavernZone : MonoBehaviour
{
    public GameObject bossZone;
    public GameObject boss;
    BringerofDeath bringerofDeath;
    private Vector3 intialPosition;
    // Start is called before the first frame update
    void Start()
    {
        bringerofDeath = boss.GetComponent<BringerofDeath>();
        intialPosition = boss.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collide with player
        if (other.CompareTag("Player"))
        {
            BossHealthbar.instance.SetEnable(true);
            bossZone.SetActive(true);
            BossHealthbar.instance.SetBossName("Bringer of Deadth");
            BackgroundMusic.instance.changeBossBGM();
            BossHealthbar.instance.SetValue(1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Player exited the zone
        if (other.CompareTag("Player"))
        {
            BossHealthbar.instance.SetEnable(false);
            bossZone.SetActive(false);
            BackgroundMusic.instance.originalBGM();
            if (!bringerofDeath.isDead) {
                bringerofDeath.currentHP = bringerofDeath.hp;
                boss.transform.position = intialPosition;
                bringerofDeath.isChasing = false;
            }
        }
    }
}
