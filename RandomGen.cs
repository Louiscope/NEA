using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen : MonoBehaviour
{
    public GameObject[] tilesVe;
    public GameObject[] tilesEa;
    public GameObject[] tilesNo;
    public GameObject[] tilesHa;
    public GameObject[] tilesVh;
    public GameObject[] tilesEx;
    public GameObject[] tiles;

    IDictionary<int, GameObject[]> tilesets = new Dictionary<int, GameObject[]>();

    FinishLevel FL;

    public GameObject finishPrefab;
    public GameObject EnemyPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float tileGap = 9f;
    public int seed = 0;

    void Start(){
        FL = GameObject.FindGameObjectWithTag("Fin").GetComponent<FinishLevel>();
        tilesets.Add(1, tilesVe); tilesets.Add(2, tilesEa); tilesets.Add(3, tilesNo); tilesets.Add(4, tilesHa); tilesets.Add(5, tilesVh); tilesets.Add(6, tilesEx);
        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);
        Gen();
    }
 
    public void Gen(){
        X = FL.GetDifficulty();
        tiles = tilesets[20*(X+10)/200];
        for (int i = 0; i < gridWidth; i++){
            for (int j = 0; j < gridHeight; j++){
                int prefabIndex = Random.Range(0, tiles.Length);
                GameObject chosenPrefab = tiles[prefabIndex];


                Vector3 tilePos = new Vector3(i * tileGap,0, j * tileGap);
                Vector3 EnemyPos = new Vector3(i * tileGap, 1, j * tileGap);

                if ((i == 4) & (j == 4)){
                    GameObject finish = Instantiate(finishPrefab, tilePos, Quaternion.identity);
                }
                GameObject Enemy = Instantiate(EnemyPrefab, EnemyPos, Quaternion.identity);
                GameObject tile = Instantiate(chosenPrefab, tilePos, Quaternion.identity);


                tile.transform.parent = transform;
            }
        }
    }
}
