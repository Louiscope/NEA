using System.Collections;
using System.Collections.Generic;
using System;
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
    public int PrevDeathEnvironment = 0;
    public int PrevDeathEnemy = 0;
    public int EnemyKillCount = 0;
    public int seed;
    public double[] Scores = {0,0,0};

    // Values used for placing the finish tile upon n kills on enemies -- Refer to documentation for information about the objectives of the game
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
    // Resets the kill count back to 0 and the bool checker for the finish tile to false, called in RandomGen.cs
    public void Reset()
    {
        PrevDeathEnvironment = DeathEnvironment;
        PrevDeathEnemy = DeathEnemy;
        DeathEnemy = 0;
        DeathEnvironment = 0;
        EnemyKillCount = 0;
        Spawned = false;
    }

    public int GetPrevDeaths()
    {
        return Math.Max(PrevDeathEnemy, PrevDeathEnvironment);
    }
    // Once the player enters a trigger (The finish tile)
    public void OnTriggerEnter(Collider other)
    {
        // Checks that the trigger is the finish
        if (other.tag == "Fin")
        {
            // Applies calculation with values and stores to current Scores location between 0 and 2
            Scores[i % 3] = sd.Calc(Points, Time.time - StartTime, DeathEnemy, DeathEnvironment);
            i++;
            S = sd.avg(Scores);
            // Sets new start time from the time the last level was completed
            StartTime = Time.time;
            // Sets a new seed for the random generation, as it is milliseconds there is an infinitesmal chance it is the same as another if there were infinite tiles
            seed = System.DateTime.Now.Millisecond;
            Destroy("Tile");
            // Moves player to start position
            Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
            // Set up and calling of Gen in RandomGem.cs
            UnityEngine.Random.InitState(seed);
            rg.Gen();
        }
    }
    
    // Uses the Reciprocal Exponential calculation found in SetDifficulty.cs for RandomGen.cs to use in the tilesets selection -- Refer to documentation for more information on the difficulty system
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
    // Increases points by 5000 until 100000 which is the cap -- Refer to the documentation for information on the points system
    public void AddPoints()
    {
        Points += 5000;
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
