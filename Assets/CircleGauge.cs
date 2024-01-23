using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleGauge : MonoBehaviour
{
    Image circlebar;

    // Start is called before the first frame update
    void Start()
    {
        circlebar = GetComponent<Image>();
    }

    void Update()
    {
        if (!GameManager.instance.islive)
            return;

        float col = GameManager.instance.player.cooltime;
        float cur = GameManager.instance.player.curtime;
        if (col == 0)
            circlebar.fillAmount = 1;
        else
            circlebar.fillAmount =  (col - cur) / col;
    }
}
