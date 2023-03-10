using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public Vector3 PlayerPos;
    public GameObject Player;

    RandomGen rg;
    SetDifficulty sd;

    public int seed;
    public double[] Scores = {0,0,0};
    public int i = 0;

    void Start()
    {
        rg = GameObject.FindGameObjectWithTag("Rand").GetComponent<RandomGen>();
        sd = GameObject.FindGameObjectWithTag("SetDiff").GetComponent<SetDifficulty>();
        
    }
    void Update()
    {
        PlayerPos = new Vector3(Player.transform.position.x - 432, Player.transform.position.y, Player.transform.position.z - 432);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Fin")
        {
            Scores[i % 3] = sd.Calc(100000, 95, 0, 0);
            i += 1;
            S = sd.Avg(Scores);
            seed = System.DateTime.Now.Millisecond;
            Destroy("Tile");
            Random.InitState(seed);
            rg.Gen();
            Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z);

        }
    }

    public double GetDifficulty()
    {
        return sd.f(S);
    }
    public void Destroy(string tag)
    {
        GameObject[] DestroyObjs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject dest in DestroyObjs)
        {
            GameObject.Destroy(dest);
        }
    }
}
