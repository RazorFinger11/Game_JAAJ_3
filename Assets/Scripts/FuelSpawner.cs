using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSpawner : MonoBehaviour
{
    public GameObject fuelPrefab;

    public Transform[] spawnPoints;
    public List<Vector3> transformVectors;

    public float timeToSpawn;
    float curTime = 0;
    public float CurTime { get => curTime; set => curTime = value; }

    public float maxFuels;
    float curFuels;
    public float CurFuels { get => curFuels; set => curFuels = value; }

    private void Start()
    {
        for(int i = 0; i <= spawnPoints.Length; i++)
        {
            transformVectors.Add(spawnPoints[i].transform.position);
        }
    }

    void Update()
    {
        if(curTime >= timeToSpawn)
        {
            if (curFuels < maxFuels)
            {
                int i = Random.Range(0, transformVectors.Count);
                Instantiate(fuelPrefab, transformVectors[i], this.transform.rotation);
                transformVectors.Remove(transformVectors[i]);
                curFuels++;
            }
            curTime = 0;
        }
        else
        {
            curTime += Time.deltaTime;
        }
    }
}
