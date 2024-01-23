using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class itemCircleGauge : MonoBehaviour
{
    Image circlebar;

    public float col = 5f;
    public bool isactive = false;

    // Start is called before the first frame update
    void Start()
    {
        circlebar = GetComponent<Image>();
    }

    void Update()
    {
        if (!GameManager.instance.islive)
            return;
        if (isactive)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);

        circlebar.fillAmount = (col - Time.deltaTime) / col;
    }

}
