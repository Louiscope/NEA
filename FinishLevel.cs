using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public Vector3 PlayerPos;
    public GameObject Player;
    RandomGen rg;
    public int seed;


    void Start()
    {
        rg = GameObject.FindGameObjectWithTag("Rand").GetComponent<RandomGen>();
        
    }
    void Update()
    {
        PlayerPos = new Vector3(Player.transform.position.x - 432, Player.transform.position.y, Player.transform.position.z - 432);
    }
    public void OnTriggerEnter(Collider other)
    {
        //PlayerPos = new Vector3(Player.transform.position.x - 432, Player.transform.position.y, Player.transform.position.z - 432);
        Player.transform.position = new Vector3(PlayerPos.x,PlayerPos.y,PlayerPos.z);

        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);
        rg.Gen();
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
