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
            Scores[0] = sd.Calc(100000, 95, 0, 0);
            Debug.Log(Scores[0]);
            seed = System.DateTime.Now.Millisecond;
            Destroy("Tile");
            Random.InitState(seed);
            rg.Gen();
            Player.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z);

        }
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
