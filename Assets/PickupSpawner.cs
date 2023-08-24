using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public Transform min, max;

    public int Total = 5;

    public GameObject[] prefabs;

    void Start()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogError("No pickup prefabs in spawner");
            return;
        }

        for (int i = 0; i < Total; i++)
        {
            SpawnPickups();
        }
    }

    void SpawnPickups()
    {
        float angle = Random.Range(0.0f, 360.0f);
        
        int i = Random.Range(0, prefabs.Length);

        var pos = new Vector2(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y));

        Instantiate(prefabs[i], pos, Quaternion.Euler(0, 0, angle));
    }
}
