using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float curtime;
    public float cooltime;
    public Transform pos;
    public Vector3 tmp_pos;
    public Vector2 boxSize;
    public Vector2 tmp_boxSize;

    Rigidbody2D rigid2D;
    Animator anim;
    Animator attack_anim;
    Animator sword_anim;
    GameObject attack_motion;
    GameObject Sword;
    SpriteRenderer attack_sprite;

    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attack_motion = GameObject.Find("atk_motion");
        Sword = GameObject.Find("Sword");
        sword_anim = Sword.GetComponent<Animator>();
        attack_anim = Sword.GetComponent<Animator>();
        attack_sprite = attack_motion.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(3, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.islive)
            return;

        attack_sprite.sortingOrder = 4;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        tmp_pos = pos.position;
        tmp_boxSize = boxSize;

        if (Input.GetKey(KeyCode.UpArrow))
            tmp_pos.y = pos.position.y + 1.0f;
        else if (Input.GetKey(KeyCode.DownArrow))
            tmp_pos.y = pos.position.y - 1.0f;
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            float tmp;

            tmp = tmp_boxSize.x;
            tmp_boxSize.x = tmp_boxSize.y;
            tmp_boxSize.y = tmp;

            tmp_pos.x = pos.position.x - 1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            float tmp;

            tmp = tmp_boxSize.x;
            tmp_boxSize.x = tmp_boxSize.y;
            tmp_boxSize.y = tmp;

            tmp_pos.x = pos.position.x + 1.0f;
        }
        else
            tmp_pos.y = pos.position.y - 1.0f;

        attack_motion.transform.position = tmp_pos;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            attack_sprite.flipX = false;
            attack_motion.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attack_sprite.flipX = false;
            attack_motion.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            attack_sprite.flipX = true;
            attack_motion.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            attack_sprite.flipX = false;
            attack_motion.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            attack_sprite.flipX = false;
            attack_motion.transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        if (curtime <= 0)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.sword);
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(tmp_pos, tmp_boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "enemy")
                    {
                        collider.GetComponent<enemy>().TakeDamage(1);
                    }
                }
                sword_anim.SetTrigger("atk");
                curtime = cooltime;
            }
        }
        else
        {
            curtime -= Time.deltaTime;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tmp_pos, tmp_boxSize);
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.islive)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid2D.MovePosition(rigid2D.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.islive)
            return;

        if (inputVec.x != 0 || inputVec.y != 0)
            anim.SetBool("ismove", true);
        else
            anim.SetBool("ismove", false);

        anim.SetFloat("inputx", inputVec.x);
        anim.SetFloat("inputy", inputVec.y);

        if (attack_anim.GetCurrentAnimatorStateInfo(0).IsName("atk"))
        {
            attack_sprite.enabled = true;
        }
        else
        {
            attack_sprite.enabled = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.islive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health <= 0)
        {
            for(int index = 0; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
            anim.SetTrigger("dead");
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();

            switch(item.type)
            {
                case "heal":
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.heal);
                    if (item.isSmall)
                    {
                        GameManager.instance.health += 10;
                    }
                    else if(item.isBig)
                    {
                        GameManager.instance.health += 30;
                    }
                    break;
                case "allkill":
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.allkill);
                    Transform[] childList = GameManager.instance.pool.GetComponentsInChildren<Transform>();
                    for(int i = 1; i < childList.Length; i++)
                    {
                        childList[i].gameObject.SetActive(false);
                    }
                    GameManager.instance.kill += childList.Length;
                    break;
                case "swordfast":
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.potion);
                    if(item.isSmall)
                    {
                        cooltime = 0.5f;
                    }
                    else if (item.isBig)
                    {
                        cooltime = 0;
                    }
                    //GameManager.instance.swordFastcirclegauge.isactive = true;
                    Invoke("re_cool", 5f);
                    break;
                case "big":
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.mushroom);
                    transform.localScale = new Vector3(10, 10, 1);
                    boxSize = new Vector2(5, 7);
                    //GameManager.instance.swordFastcirclegauge.isactive = true;
                    Invoke("re_big", 5f);
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    void re_cool()
    {
        cooltime = 1;
        //GameManager.instance.swordFastcirclegauge.isactive = false;
    }

    void re_big()
    {
        transform.localScale = new Vector3(3, 3, 1);
        boxSize = new Vector2(2, 2);
        //GameManager.instance.bigCirclegauge.isactive = false;
    }
}
