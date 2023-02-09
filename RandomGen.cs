using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject finishPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float tileGap = 9f;
    public int seed = 0;

    // Start is called before the first frame update
    void Start()
    {
        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);

        Gen();
    }
    public void Test()
    {
        Debug.Log("Test");
    }

    public void Gen()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                int prefabIndex = Random.Range(0, tiles.Length);
                GameObject chosenPrefab = tiles[prefabIndex];


                Vector3 tilePos = new Vector3(i * tileGap,0, j * tileGap);

                if ((i == 4) & (j == 4))
                {
                    GameObject finish = Instantiate(finishPrefab, tilePos, Quaternion.identity);
                }

                GameObject tile = Instantiate(chosenPrefab, tilePos, Quaternion.identity);


                tile.transform.parent = transform;
            }
        }
    }
}
