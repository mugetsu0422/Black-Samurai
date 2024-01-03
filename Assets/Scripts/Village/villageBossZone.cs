using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class villageBossZone : MonoBehaviour
{
    // Start is called before the first frame update
    public bool spawnboss;
    public GameObject boss;
    public GameObject boder;
    public Vector3 boss_pos;
    CompositeCollider2D col;
    TilemapRenderer tileRender;
    void Start()
    {
        col = boder.gameObject.GetComponent<CompositeCollider2D>();
        tileRender = boder.gameObject.GetComponent<TilemapRenderer>();
        if (KaguraBachiData.bossKilled.ContainsKey(boss.transform.name)){
            spawnboss = !KaguraBachiData.bossKilled[boss.transform.name];
        }
        else{
            KaguraBachiData.bossKilled.Add(boss.transform.name,false);
            spawnboss = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && spawnboss){
            tileRender.enabled = true;
            col.isTrigger = false;
            GameObject g_boss = Instantiate(boss,boss_pos, new Quaternion());
            g_boss.GetComponent<VillageBoss>().bossboder = boder;
        }
        Destroy(gameObject);
    }
}
