using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool islive;
    public float gameTime;
    public float maxGameTime = 100 * 60f;

    public int level;
    public int kill;
    public float health;
    public float maxHealth = 100;

    public PoolManager pool;
    public ItemPoolManager itemPool;
    public playerController player;
    public GameObject uiResult;
    public GameObject cooltime;
    public Item item;
    public itemCircleGauge swordFastcirclegauge;
    public itemCircleGauge bigCirclegauge;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;
        Animator p = player.GetComponent<Animator>();
        p.SetBool("islive", true);
        islive = true;

        AudioManager.instance.PlayBgm(true);
        Resume();
    }

    public void GameOver()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.gameover);
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        islive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.SetActive(true);
        cooltime.SetActive(false);

        Stop();

        AudioManager.instance.PlayBgm(false);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!islive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Stop()
    {
        islive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        islive = true;
        Time.timeScale = 1;
    }
}
