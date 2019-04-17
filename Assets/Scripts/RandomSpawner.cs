using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour {

    public GameObject[] spawness;
    public Vector3 pos;
    LevelManager levelM;
    //public Transform spawnPos;
    private float InstantiationTimer = 2f;
    public float lifeTime = 2f;
    int randomInt;
	// Use this for initialization
	void Start () {
       
	}

    void Awake()
    {
         pos = new Vector3(Random.Range(-10f, 8f), 20, Random.Range(18f, 88f));
    }

    // Update is called once per frame
    void Update () {
        SpawnRandom();
        // spawnPos.position = pos;
         pos = new Vector3(Random.Range(-10f, 8f), 20, Random.Range(18f, 88f));
    }

   public void SpawnRandom()
    {
        InstantiationTimer -= Time.deltaTime;
        randomInt = Random.Range(0, spawness.Length);
        if (InstantiationTimer <= 0)
        {

                Instantiate(spawness[randomInt], pos, Quaternion.identity);
            
            InstantiationTimer = 2f;
        }
       
    }


 
}
