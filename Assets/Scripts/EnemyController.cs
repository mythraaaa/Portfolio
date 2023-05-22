using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    private BoxCollider2D b2d;

    private Animator anim;

    public int stateNumber = 0;

    public int currentHealth;

    public int maxHealth = 200;

    private GameObject Player;

    public LayerMask playerLayers;

    public Transform attackPoint;

    public bool isEnd = false;

    private float attackRange = 0.5f;

    public int enemyAttackDamage = 50;

    public int playerAttackDamage = 100;

    private float TimeCounter = 0f;

    [SerializeField] GameObject gameClearCanvas;
    [SerializeField] GameObject GameClearSound;

    //SE
    [SerializeField] public AudioSource AttackAudioSource; // 攻撃音を再生するためのオーディオソース
    [SerializeField] public AudioClip attackClip; // 攻撃音の効果音

    // Start is called before the first frame update
    void Start()
    {
        //体力を宣言
        currentHealth = maxHealth;
        this.b2d = GetComponent<BoxCollider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.Player = GameObject.Find("Player");

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("敵残りライフ：" + currentHealth);

        if (currentHealth <= 0)
        {

            Debug.Log("Enemy dead!");

            anim.SetTrigger("isDead");

            this.enabled = false;

            Destroy(this.gameObject.GetComponent<Rigidbody2D>());

            Destroy(this.gameObject.GetComponent<BoxCollider2D>());

            this.isEnd = true;
            Invoke("ChangeTimeScale", 1f);
            gameClearCanvas.SetActive(true);
            GameClearSound.SetActive(true);

            


            if (Player.GetComponent<Animator>().GetBool("isWalk") ||
                    Player.GetComponent<Animator>().GetBool("isJump") ||
                    Player.GetComponent<Animator>().GetBool("isAttack") ||
                    Player.GetComponent<Animator>().GetBool("isDamage"))
            {
                // isWalkアニメーションが再生中の場合、isWalkアニメーションを終わらせる
                Player.GetComponent<Animator>().SetBool("isWalk", false);
                Player.GetComponent<Animator>().SetBool("isJump", false);
                Player.GetComponent<Animator>().SetBool("isAttack", false);
                Player.GetComponent<Animator>().SetBool("isDamage", false);

                //Time.timeScale = 0;

                stateNumber = -1;
                //changeState(-1);

            }
            else
            {
                anim.SetTrigger("isDamage");
            }
        }
    }

    private void ChangeTimeScale()
    {
        Time.timeScale = 0f;
    }
    void Attack()
    {

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Player.GetComponent<PlayerController>().TakeDamage(enemyAttackDamage);
        }

    }

    /* デバッグ用
        * void changeState(int value) 
    {

        TimeCounter = 0f;
        stateNumber = value;
        Debug.Log("状態" + value + "に遷移");
    } */

    // Update is called once per frame
    void Update()

    {

        Vector2 Position = transform.position;
        float distance = Vector2.Distance(Player.transform.position, transform.position);

        switch (stateNumber)
        {
            case 0://ニュートラル
                {
                    if (TimeCounter > 1f)
                    {
                        TimeCounter = 0f;
                    }
                    if (distance < 6f)
                    {
                        stateNumber = 1;
                        //Debug.Log("状態1へ遷移");
                        //changeState(1);
                    }
                }
                break;

            case 1: //プレイヤーに向かって移動
                {

                    if (this.transform.position.x > Player.transform.position.x)
                    {
                        //プレイヤーが左側にいる
                        this.transform.localScale = new Vector2(4f, 4f);
                        rb.velocity = new Vector2(-3f, 0f);
                        anim.SetBool("isWalk", true);
                    }
                    else
                    {
                        //プレイヤーが右側にいる
                        this.transform.localScale = new Vector2(-4f, 4f);
                        rb.velocity = new Vector2(3f, 0f);
                        anim.SetBool("isWalk", true);
                    }

                    if (distance < 2f)
                    {
                        stateNumber = 2;
                        //Debug.Log("状態2へ遷移");
                        //changeState(2);
                    }
                    else if (distance > 3f)

                    {
                        rb.velocity = Vector2.zero;
                        anim.SetBool("isWalk", false);
                        stateNumber = 0;
                        //Debug.Log("状態0へ遷移");
                        // changeState(0);
                    }
                }
                break;

            case 2: //プレイヤーが反対を向いたら自分の向きを反転
                {
                    if (this.transform.position.x > Player.transform.position.x)
                    {
                        //プレイヤーが左側にいる
                        this.transform.localScale = new Vector2(4f, 4f);
                        rb.velocity = new Vector2(-1f, 0f);
                    }
                    else
                    {
                        //プレイヤーが右側にいる
                        this.transform.localScale = new Vector2(-4f, 4f);
                        rb.velocity = new Vector2(1f, 0f);
                    }
                    if (distance < 2f) //距離が2fより近い場合
                    {
                        rb.velocity = Vector2.zero;
                        anim.SetBool("isWalk", false);
                        TimeCounter = 0f;
                        this.anim.SetTrigger("isAttack");
                        AttackAudioSource.PlayOneShot(attackClip);
                        stateNumber = 3;
                        //Debug.Log("状態3へ遷移");
                        Attack();
                        Debug.Log("" + enemyAttackDamage);
                        //changeState(3);
                    }
                    else if (distance > 3f)
                    {
                        stateNumber = 1;
                        //Debug.Log("状態1へ遷移");
                        //changeState(1);
                    }
                }
                break;

            case 3: //プレイヤーが死亡した場合
                {
                    if (Player.GetComponent<BoxCollider2D>() == null)
                    {
                        //動作停止
                        stateNumber = -5;
                        //changeState(-5);
                    }
                    else
                    {
                        //クールタイム
                        stateNumber = 4;
                        //changeState(4);
                    }
                }
                break;

            case 4: //行動後、2fのクールタイム
            {
                    if (TimeCounter > 2f)
                    {
                        //TimeCounter = 0f;
                        stateNumber = 0;
                        //Debug.Log("状態0へ遷移");
                        //changeState(0);
                    }
                }
                break;
        }
        TimeCounter += Time.deltaTime;
    }
}

