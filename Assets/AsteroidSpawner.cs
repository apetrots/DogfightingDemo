using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Transform min, max;

    public int Total = 100;

    public float MinScale = 0.5f, MaxScale = 2.0f;

    public GameObject[] prefabs;

    void Start()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogError("No asteroid prefabs in spawner");
            return;
        }

        for (int i = 0; i < Total; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        float scale = Random.Range(MinScale, MaxScale);
        float angle = Random.Range(0.0f, 360.0f);
        
        int i = Random.Range(0, prefabs.Length);

        Bounds bounds = prefabs[i].GetComponent<Collider2D>().bounds;
        
        Vector2 pos = Vector2.zero;
        bool spotFound = false;
        while (!spotFound)
        {
            pos = new Vector2(Random.Range(min.position.x, max.position.x), Random.Range(min.position.y, max.position.y));

            if (Physics2D.OverlapBox(pos, bounds.size + Vector3.one * 0.1f, angle))
                continue;

            spotFound = true;
        }

        var obj = Instantiate(prefabs[i], pos, Quaternion.Euler(0, 0, angle));
        obj.transform.localScale = Vector3.one * scale;

        Vector2 randomVelocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

        obj.GetComponent<Rigidbody2D>().velocity = randomVelocity;
    }
}
