using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public Transform[] spawnpoint;

    public int level;
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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 60f), 3);
        GameManager.instance.level = level;

        if(timer > (level < 3 ? 0.5f : 0.2f))
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject enemy;
            enemy = GameManager.instance.pool.Get((level < 3 ? level : Random.Range(0, GameManager.instance.pool.pools.Length)));
        
        enemy.transform.position = spawnpoint[Random.Range(1, spawnpoint.Length)].position;
    }
}
