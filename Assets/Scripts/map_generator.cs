using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class map_generator : MonoBehaviour
{
    // Start is called before the first frame update
    public TileBase tile;
    public Tilemap tilemap;
    void Start()
    {
        for (int i = -20 ; i < 20  ; i ++){
            for(int j = -20 ; j < 20 ; j ++){
                if (UnityEngine.Random.Range(1,10)<8)
                    tilemap.SetTile(new Vector3Int(i,j,0),tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
