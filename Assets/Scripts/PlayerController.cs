using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private BoxCollider2D b2d;

    private Animator anim;

    private GameObject Enemy;

    //スピード、ジャンプの数値
    public float speed = 12f;
    public float jumpForce = 25f;

    private bool isGround = false;

    public LayerMask groundLayer; // 地面レイヤー
    public LayerMask enemyLayers; // 敵レイヤー

    public int stateNumber = 0;

    //終了判定
    public bool isEnd = false;

    private float TimeCounter = 0f;
    
    [SerializeField] GameObject gameOverCanvas; //ゲーム終了時に表示するテキスト
    [SerializeField] GameObject GameOverSound; //ゲーム終了時に流れる音楽

    //攻撃力
    public int enemyAttackDamage = 50;

    public int playerAttackDamage = 100;

    //攻撃判定
    public Transform attackPoint;

    private float attackRange = 0.5f;

    //体力
    public int currentHealth;

    public int maxHealth = 500;

    public HealthBar healthBar;

    //SE
    [SerializeField] public AudioSource footstepAudioSource; // 足音を再生するためのオーディオソース
    [SerializeField] public AudioClip footstepClip; // 足音の効果音
    [SerializeField] public AudioSource JumpAudioSource; // jump音を再生するためのオーディオソース
    [SerializeField] public AudioClip jumpClip; // jump音の効果音
    [SerializeField] public AudioSource AttackAudioSource; // 攻撃音を再生するためのオーディオソース
    [SerializeField] public AudioClip attackClip; // 攻撃音の効果音

    // Start is called before the first frame update
    void Start()
    {

        //体力を宣言
        currentHealth = maxHealth;
        //HPバーに最大体力をセット
        healthBar.SetMaxHealth(maxHealth);
        this.b2d = GetComponent<BoxCollider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.Enemy = GameObject.Find("Enemy");
        
        //フレーム固定
        Application.targetFrameRate = 60;
    }
    private void PlayFootstepSound()
    {
        footstepAudioSource.clip = footstepClip;
        footstepAudioSource.Play();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            //ゲームオーバーの処理
            Debug.Log("Your dead!");

            anim.SetTrigger("isDead");

            this.enabled = false;

            Destroy(this.gameObject.GetComponent<Rigidbody2D>());

            Destroy(this.gameObject.GetComponent<BoxCollider2D>());

            this.isEnd = true;
            
            gameOverCanvas.SetActive(true);

            GameOverSound.SetActive(true);

        }
        else
        {
            anim.SetTrigger("isDamage");
            
        }
    }

    void Attack()
    {
        anim.SetBool("isAttack", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(playerAttackDamage);
        }

    }
   

    // Update is called once per frame
    void Update()
    {
        
        switch (stateNumber)
        {

            //ニュートラル
            case 0: //0~移動系
                {
                    //左右移動の入力で方向と移動量を獲得
                    float x = Input.GetAxis("Horizontal");

                    //キャラクターを移動する
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);

                    //アニメーションに数値を送る
                    anim.SetFloat("isSpeed", Mathf.Abs(x * speed));

                    //キャラクターの速度は0.1f以上か？
                    if (Mathf.Abs(rb.velocity.x) > 0.1f && isGround)
                    {
                        //もし音が鳴ってないなら
                        if (!footstepAudioSource.isPlaying)
                        {
                            //鳴らす
                            PlayFootstepSound();
                        }
                    }
                    else
                    {
                        footstepAudioSource.Stop();
                    }

                    if (x < 0)
                    {
                        //当たり判定も反転させる
                        this.transform.localScale = new Vector2(-4f, 4f);

                    }

                    if (x > 0)
                    {

                        //当たり判定も反転させる
                        this.transform.localScale = new Vector2(4f, 4f);

                    }

                    //地面に居る、かつジャンプキーを押したら
                    if (Input.GetButtonDown("Jump") && isGround)
                    {
                        //ジャンプする

                        Debug.Log("状態1へ遷移");
                        stateNumber = 1;
                    }

                    // 1秒経った？
                    if ( TimeCounter > 1f)
                    {
                        //地面にいる、かつクリックしたら
                        if (Input.GetButtonDown("Fire1") && isGround && rb.velocity == Vector2.zero)
                        {

                            Debug.Log("状態10へ遷移");
                            stateNumber = 10;
                        }
                    }
                    
                    
                }
                break;

            //ジャンプ
            case 1:
                {
                    anim.SetBool("isJump", true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    JumpAudioSource.PlayOneShot(jumpClip);

                    Debug.Log("状態2へ遷移");
                    stateNumber = 2;
                }
                break;

            //ジャンプした後
            case 2:
                {
                    anim.SetBool("isJump", false);
                    Debug.Log("状態0へ遷移");
                    stateNumber = 0;
                }
                break;
                
            case 10: //10~攻撃
                {

                    // anim.SetTrigger("isDamage")が再生されている場合
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
                    {
                        Debug.Log("状態0へ遷移");
                        stateNumber = 0;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        Attack();
                        AttackAudioSource.PlayOneShot(attackClip);
                        Debug.Log("" + playerAttackDamage);
                        Debug.Log("状態15へ遷移");
                        TimeCounter = 0.0f;
                        stateNumber = 15;
                    }
                }            
                break;

            case 15: //攻撃後ニュートラルへ
                {
                     anim.SetBool("isAttack", false);
                     Debug.Log("状態0へ遷移");
                     stateNumber = 0; 
                }
                break;
        }

        TimeCounter += Time.deltaTime;

    }
    private void FixedUpdate()
    {
       //接地判定

        isGround = false;

        Vector2 groundPos =
            new Vector2(
                transform.position.x,
                transform.position.y
                );

        Vector2 groundArea = new Vector2(0.5f, 0.7f);

        //Debug.DrawLine(groundPos + groundArea, groundPos - groundArea, Color.red);

        isGround =
            Physics2D.OverlapArea(
                groundPos + groundArea,
                groundPos - groundArea,
                groundLayer
                );


        //Debug.Log(isGround);
    }
}
