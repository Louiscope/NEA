using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    
    public Vector3 PlayerPos;
    public Vector3 StartPos = new Vector3(0, 1, 0);
    public GameObject Player;
    
    // Contains values for weighting to be used in SetDifficulty.cs
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
    
    void Start()
    {
        Points = 0;
        StartTime = 0;
        DeathEnemy = 0;
        DeathEnvironment = 0;
        rg = GameObject.FindGameObjectWithTag("Rand").GetComponent<RandomGen>();
        sd = GameObject.FindGameObjectWithTag("SetDiff").GetComponent<SetDifficulty>();
    }
    void Update()
    {
        PlayerPos = new Vector3(Player.transform.position.x - 432, 1, Player.transform.position.z - 432);
        
        if ((!Spawned) & (EnemyKillCount >= 5))
        {
            SpawnFinish();
        }
    }
    // Used for debugging
    public void SetScores(int val)
    {
        Scores[0] = val * 20; Scores[1] = val * 20; Scores[2] = val * 20;
    }
    
    void SpawnFinish()
    {
        GameObject Finish = Instantiate(FinishPrefab, FinishPos, Quaternion.identity);
        Spawned = true;
    }
    
    public void AddKillCount()
    {
        EnemyKillCount++;
    }
    
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
    
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Fin")
        {
           
            Scores[i % 3] = sd.Calc(Points, Time.time - StartTime, DeathEnemy, DeathEnvironment);
            i++;
            S = sd.avg(Scores);
            
            StartTime = Time.time;
            
            seed = System.DateTime.Now.Millisecond;
            Destroy("Tile");
            
            Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z);
            
            UnityEngine.Random.InitState(seed);
            rg.Gen();
        }
    }
    
    
    public int GetDifficulty()
    {
        return sd.f(S);
    }

    
    public void AddDeath(bool DeathType)
    {
        if (DeathType)
        {
            DeathEnemy++;
        }
        else
        {
            DeathEnvironment++;
        }
        Player.transform.position = new Vector3(StartPos.x, 1, StartPos.z);
    }
    
    public void AddPoints()
    {
        Points += 5000;
        if (Points > 100000)
        {
            Points = 100000;
        }
    }
    
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
