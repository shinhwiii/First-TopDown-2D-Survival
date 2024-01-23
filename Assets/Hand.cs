using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public SpriteRenderer spriter;

    Animator anim;
    SpriteRenderer player;

    Vector3 PosRight = new Vector3(0.19f, 0, 0);
    Vector3 PosLeft = new Vector3(-0.19f, 0, 0);
    Quaternion RotRight = Quaternion.Euler(0, 0, 13);
    Quaternion RotLeft = Quaternion.Euler(0, 0, 67);    

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.islive)
            return;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("atk"))
        {
            spriter.enabled = false;
        }
        else
        {
            spriter.enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localPosition = PosLeft;
            transform.localRotation = RotLeft;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localPosition = PosRight;
            transform.localRotation = RotRight;
        }

    }
}
