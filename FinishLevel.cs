using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    // Finds Player position to reset upon level completion
    public Vector3 PlayerPos;
    public Vector3 StartPos = new Vector3(0, 1, 0);
    public GameObject Player;
    
    // Contains Script calls for RandomGen.cs and SetDifficulty.cs -- Contains values for weighting to be used in SetDifficulty.cs
    RandomGen rg;
    SetDifficulty sd;
    public float Points;
    public float StartTime;
    public int DeathEnemy;
    public int DeathEnvironment;
    public int EnemyKillCount = 0;
    public int seed;
    public double[] Scores = {0,0,0};

    // Values used for placing the finish tile upon X deaths
    public GameObject FinishPrefab;
    Vector3 FinishPos = new Vector3(432f, 1, 432f);
    public bool Spawned = false;

    [SerializeField] public int i = 0;
    [SerializeField] public double S;
    
    // Mainly Setter of Values such as Points and deaths -- Finds GameObjects with the scripts of RandomGen.cs and SetDifficulty.cs attached
    void Start()
    {
        Points = 0;
        StartTime = 0;
        DeathEnemy = 0;
        DeathEnvironment = 0;
        rg = GameObject.FindGameObjectWithTag("Rand").GetComponent<RandomGen>();
        sd = GameObject.FindGameObjectWithTag("SetDiff").GetComponent<SetDifficulty>();
        
    }
    // Finds current player position for spawning
    void Update()
    {
        PlayerPos = new Vector3(Player.transform.position.x - 432, 1, Player.transform.position.z - 432);
        // Checks if the finish is not already spawned and the amount of kills is at least 5
        if ((!Spawned) & (EnemyKillCount >= 5))
        {
            SpawnFinish();
        }
    }
    // Used for debugging / initial setting of difficulty when game starts
    public void SetScores(int val)
    {
        Scores[0] = val * 20; Scores[1] = val * 20; Scores[2] = val * 20;
        Debug.Log(sd.avg(Scores));
    }
    // Spawns the finish tile
    void SpawnFinish()
    {
        GameObject Finish = Instantiate(FinishPrefab, FinishPos, Quaternion.identity);
        Spawned = true;
    }
    // Increases EnemyKillCount value by 1 upon and enemy being killed, called from EnemyAiTutorial.cs
    public void AddKillCount()
    {
        EnemyKillCount++;
    }
    // Resets the kill count back to 0, called in RandomGen.cs
    public void ResetKillCount()
    {
        EnemyKillCount = 0;
    }
    // Resets the bool of spawned once new level begins, called in RandomGen.cs
    public void ResetSpawned()
    {
        Spawned = false;
    }
    // Once the player enters a trigger (The finish tile)
    public void OnTriggerEnter(Collider other)
    {
        // Checks that the trigger is the finish
        if (other.tag == "Fin")
        {
            // Applies calculation with values and stores to current Scores location -- Destroys the tiles and then regenerates with the next tileset based upon difficulty
            Scores[i % 3] = sd.Calc(Points, Time.time - StartTime, DeathEnemy, DeathEnvironment);
            i++;
            S = sd.avg(Scores);
            StartTime = Time.time;
            seed = System.DateTime.Now.Millisecond;
            Destroy("Tile");
            GameObject.Destroy(other);
            Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
            Random.InitState(seed);
            rg.Gen();
        }
    }
    
    // Uses the Exponential calculation found in SetDifficulty.cs for RandomGen.cs to use in the tilesets selection
    public int GetDifficulty()
    {
        return sd.f(S);
    }

    // Adds to death count, uses bool to determine type of death as only 2 types, Enemy and environment
    public void AddDeath(bool Z)
    {
        if (Z)
        {
            DeathEnemy++;
        }
        else
        {
            DeathEnvironment++;
        }
        Player.transform.position = new Vector3(StartPos.x, 1, StartPos.z);
    }
    // Increases points by 5000 until 100000 which is the cap
    public void AddPoints()
    {
        Points += 5000;
        Debug.Log(Points);
        if (Points > 100000)
        {
            Points = 100000;
        }
    }
    // Removes all game objects of a certain name and removes the finish tile
    public void Destroy(string tag)
    {
        GameObject[] DestroyObjs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject dest in DestroyObjs)
        {
            GameObject.Destroy(dest);
        }
        GameObject.Destroy(GameObject.Find("Finish(Clone)"));
    }
}
