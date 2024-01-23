using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Level, Kill, Time, Health }
    public InfoType type;

    Text mytext;
    Slider myslider;

    private void Awake()
    {
        mytext = GetComponent<Text>();
        myslider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Level:
                mytext.text = string.Format("Level : {0:F0}", GameManager.instance.level+1); 
                break;
            case InfoType.Kill:
                mytext.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                int sec = Mathf.FloorToInt(GameManager.instance.gameTime) % 60;
                int min = Mathf.FloorToInt(GameManager.instance.gameTime) / 60;
                mytext.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                myslider.value = curHealth / maxHealth;
                break;
        }
    }
}
