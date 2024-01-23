using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    public float x_key;
    public float y_key;
    public int Hp = 1;
    
    bool islive = true;

    Rigidbody2D rigid2D;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    Renderer render;

    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        render = GetComponent<Renderer>();
        this.gameObject.layer = 7;
        this.gameObject.tag = "enemy";
    }

    private void Start()
    {
        render.sortingOrder = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.islive)
            return;
        anim.SetBool("islive", true);

        if (Hp <= 0)
        {
            Dead();
        }

        if (!islive)
            return;
        if (target.position.x - rigid2D.position.x < 0)
            x_key = -1;
        else if (target.position.x - rigid2D.position.x > 0)
            x_key = 1;
        if (target.position.y - rigid2D.position.y < 0)
            y_key = -1;
        else if (target.position.y - rigid2D.position.y > 0)
            y_key = 1;

        if (Mathf.Abs(target.position.x - rigid2D.position.x) > Mathf.Abs(target.position.y - rigid2D.position.y))
            y_key = 0;
        else if (Mathf.Abs(target.position.x - rigid2D.position.x) < Mathf.Abs(target.position.y - rigid2D.position.y))
            x_key = 0;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.islive)
            return;

        if (!islive)
            return;
        Vector2 dirVec = target.position - rigid2D.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid2D.MovePosition(rigid2D.position + nextVec);
        rigid2D.velocity = Vector2.zero;

    }

    private void LateUpdate()
    {
        if (!GameManager.instance.islive)
            return;

        if (!islive)
            return;
        if (x_key != 0 || y_key != 0)
            anim.SetBool("ismove", true);
        else
            anim.SetBool("ismove", false);

        anim.SetFloat("inputx", x_key);
        anim.SetFloat("inputy", y_key);
        
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        islive = true;
        coll.enabled = true;
        rigid2D.simulated = true;
        spriter.sortingOrder = 1;
        Hp = 1;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }

    void Dead()
    {
        islive = false;
        coll.enabled = false;
        rigid2D.simulated = false;
        spriter.sortingOrder = 1;
        GameManager.instance.kill++;
        gameObject.SetActive(false);
    }
}
