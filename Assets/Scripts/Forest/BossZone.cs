using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossZone : MonoBehaviour
{
    [SerializeField] string bossName;
    [SerializeField] Tilemap bossZoneTilemap;
    bool isDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        BossHealthbar.instance.SetBossName(bossName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDefeated)
        {
            bossZoneTilemap.gameObject.SetActive(true);
            BossHealthbar.instance.SetEnable(true);
            BackgroundMusic.instance.changeBossBGM();
        }
    }

    public void BossDefeated()
    {
        bossZoneTilemap.gameObject.SetActive(false);
        BossHealthbar.instance.SetEnable(false);
        BackgroundMusic.instance.originalBGM();
        isDefeated = true;
    }
}
