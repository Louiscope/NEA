using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen : MonoBehaviour
{
    // Declaration of Tilesets and a checker for tileset (DiffVal) -- Dictionary declaration used for the tilesets to be stored
    public GameObject[] tilesVe;
    public GameObject[] tilesEa;
    public GameObject[] tilesNo;
    public GameObject[] tilesHa;
    public GameObject[] tilesVh;
    public GameObject[] tilesEx;
    public GameObject[] tiles;
    public int DiffVal = 1;
    public bool Check = false;
    IDictionary<int, GameObject[]> tilesets = new Dictionary<int, GameObject[]>();

    // Calling finishlevel.cs
    FinishLevel FL;

    // Contains values for how the level in generated
    public GameObject StartPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float tileGap = 9f;
    public int seed = 0;
    public int X = 1;
    public bool Passed = false;

    // Gets the component of FinishLevel from FinishLogger upon start -- Adds all tilesets to the dictionary -- Collects the seed for random generation and sets the tileset to X (1 to start)
    void Start(){
        FL = GameObject.FindGameObjectWithTag("Fin").GetComponent<FinishLevel>();
        tilesets.Add(0, tilesVe); tilesets.Add(1, tilesEa); tilesets.Add(2, tilesNo); tilesets.Add(3, tilesHa); tilesets.Add(4, tilesVh); tilesets.Add(5, tilesEx);
        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);
        tiles = tilesets[X];
        Gen();
    }

    // Calls debugging tool to switch tilesets instantly
    void Update()
    {
        DiffSetter();
    }
    
    // Performs random generation in a grid of 5 by 5
    public void Gen(){
        Check = false;
        // Checks if the level has already been completed with Passed
        if (Passed)
        {
            // Calculates and determines difficulty values -- Resets the counters from previous level
            X = FL.GetDifficulty();
            FL.ResetKillCount();
            FL.ResetSpawned();
        }
        tiles = tilesets[X];
        Passed = true;
        
        // Nested iteration to apply the multidirectional generation horizontally and vertically when looked at top down
        for (int i = 0; i < gridWidth; i++){
            for (int j = 0; j < gridHeight; j++){
                // Picks a random tile from the current tileset
                int prefabIndex = Random.Range(0, tiles.Length);
                GameObject chosenPrefab = tiles[prefabIndex];

                // Determines location of tiles in the grid
                Vector3 tilePos = new Vector3(i * tileGap,0, j * tileGap);

                // Checks to see if first tile -- Generates a predetermined start room with no enemies
                if ((i == 0) & (j == 0)){
                    GameObject starttile = Instantiate(StartPrefab, tilePos, Quaternion.identity);
                }
                // otherwise generates normally with the random tile
                else
                {
                    GameObject tile = Instantiate(chosenPrefab, tilePos, Quaternion.identity);
                    tile.transform.parent = transform;
                }
            }
        }
    }
    //Debugging tool used to change difficulty by pressing keys
    public void DiffSetter()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DiffVal = 0;
            Check = true;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            DiffVal = 1;
            Check = true;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            DiffVal = 2;
            Check = true;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            DiffVal = 3;
            Check = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            DiffVal = 4;
            Check = true;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            DiffVal = 5;
            Check = true;
        }
        if (Check)
        {
            FL.SetScores(DiffVal);
            X = DiffVal;
            Passed = false;
            FL.AddDeath(true);
            FL.Destroy("Tile");
            Gen();
        }
        
    }
}
