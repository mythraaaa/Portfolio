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
    [SerializeField] public AudioSource AttackAudioSource; // �U�������Đ����邽�߂̃I�[�f�B�I�\�[�X
    [SerializeField] public AudioClip attackClip; // �U�����̌��ʉ�

    // Start is called before the first frame update
    void Start()
    {
        //�̗͂�錾
        currentHealth = maxHealth;
        this.b2d = GetComponent<BoxCollider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.Player = GameObject.Find("Player");

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("�G�c�胉�C�t�F" + currentHealth);

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
                // isWalk�A�j���[�V�������Đ����̏ꍇ�AisWalk�A�j���[�V�������I��点��
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

    /* �f�o�b�O�p
        * void changeState(int value) 
    {

        TimeCounter = 0f;
        stateNumber = value;
        Debug.Log("���" + value + "�ɑJ��");
    } */

    // Update is called once per frame
    void Update()

    {

        Vector2 Position = transform.position;
        float distance = Vector2.Distance(Player.transform.position, transform.position);

        switch (stateNumber)
        {
            case 0://�j���[�g����
                {
                    if (TimeCounter > 1f)
                    {
                        TimeCounter = 0f;
                    }
                    if (distance < 6f)
                    {
                        stateNumber = 1;
                        //Debug.Log("���1�֑J��");
                        //changeState(1);
                    }
                }
                break;

            case 1: //�v���C���[�Ɍ������Ĉړ�
                {

                    if (this.transform.position.x > Player.transform.position.x)
                    {
                        //�v���C���[�������ɂ���
                        this.transform.localScale = new Vector2(4f, 4f);
                        rb.velocity = new Vector2(-3f, 0f);
                        anim.SetBool("isWalk", true);
                    }
                    else
                    {
                        //�v���C���[���E���ɂ���
                        this.transform.localScale = new Vector2(-4f, 4f);
                        rb.velocity = new Vector2(3f, 0f);
                        anim.SetBool("isWalk", true);
                    }

                    if (distance < 2f)
                    {
                        stateNumber = 2;
                        //Debug.Log("���2�֑J��");
                        //changeState(2);
                    }
                    else if (distance > 3f)

                    {
                        rb.velocity = Vector2.zero;
                        anim.SetBool("isWalk", false);
                        stateNumber = 0;
                        //Debug.Log("���0�֑J��");
                        // changeState(0);
                    }
                }
                break;

            case 2: //�v���C���[�����΂��������玩���̌����𔽓]
                {
                    if (this.transform.position.x > Player.transform.position.x)
                    {
                        //�v���C���[�������ɂ���
                        this.transform.localScale = new Vector2(4f, 4f);
                        rb.velocity = new Vector2(-1f, 0f);
                    }
                    else
                    {
                        //�v���C���[���E���ɂ���
                        this.transform.localScale = new Vector2(-4f, 4f);
                        rb.velocity = new Vector2(1f, 0f);
                    }
                    if (distance < 2f) //������2f���߂��ꍇ
                    {
                        rb.velocity = Vector2.zero;
                        anim.SetBool("isWalk", false);
                        TimeCounter = 0f;
                        this.anim.SetTrigger("isAttack");
                        AttackAudioSource.PlayOneShot(attackClip);
                        stateNumber = 3;
                        //Debug.Log("���3�֑J��");
                        Attack();
                        Debug.Log("" + enemyAttackDamage);
                        //changeState(3);
                    }
                    else if (distance > 3f)
                    {
                        stateNumber = 1;
                        //Debug.Log("���1�֑J��");
                        //changeState(1);
                    }
                }
                break;

            case 3: //�v���C���[�����S�����ꍇ
                {
                    if (Player.GetComponent<BoxCollider2D>() == null)
                    {
                        //�����~
                        stateNumber = -5;
                        //changeState(-5);
                    }
                    else
                    {
                        //�N�[���^�C��
                        stateNumber = 4;
                        //changeState(4);
                    }
                }
                break;

            case 4: //�s����A2f�̃N�[���^�C��
            {
                    if (TimeCounter > 2f)
                    {
                        //TimeCounter = 0f;
                        stateNumber = 0;
                        //Debug.Log("���0�֑J��");
                        //changeState(0);
                    }
                }
                break;
        }
        TimeCounter += Time.deltaTime;
    }
}

