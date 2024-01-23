using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform[] spawnpoint;

    float timer;

    private void Awake()
    {
        spawnpoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.islive)
            return;

        timer += Time.deltaTime;

        if (timer > 15f)
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject item = GameManager.instance.itemPool.Get(Random.Range(0, GameManager.instance.itemPool.pools.Length));
        item.transform.position = spawnpoint[Random.Range(1, spawnpoint.Length)].position;
    }
}
