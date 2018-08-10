using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float spawnTime;
    private int counter;
    public int counterLimit;
    // Use this for initialization
    void Start () {

        InvokeRepeating("Spawn", spawnTime, spawnTime);


    }

    // Update is called once per frame
    void Update () {
        if (counter > counterLimit)
        {
            CancelInvoke();
        }
        
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        counter++;
    }





}


