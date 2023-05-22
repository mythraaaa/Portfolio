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

    //�X�s�[�h�A�W�����v�̐��l
    public float speed = 12f;
    public float jumpForce = 25f;

    private bool isGround = false;

    public LayerMask groundLayer; // �n�ʃ��C���[
    public LayerMask enemyLayers; // �G���C���[

    public int stateNumber = 0;

    //�I������
    public bool isEnd = false;

    private float TimeCounter = 0f;
    
    [SerializeField] GameObject gameOverCanvas; //�Q�[���I�����ɕ\������e�L�X�g
    [SerializeField] GameObject GameOverSound; //�Q�[���I�����ɗ���鉹�y

    //�U����
    public int enemyAttackDamage = 50;

    public int playerAttackDamage = 100;

    //�U������
    public Transform attackPoint;

    private float attackRange = 0.5f;

    //�̗�
    public int currentHealth;

    public int maxHealth = 500;

    public HealthBar healthBar;

    //SE
    [SerializeField] public AudioSource footstepAudioSource; // �������Đ����邽�߂̃I�[�f�B�I�\�[�X
    [SerializeField] public AudioClip footstepClip; // �����̌��ʉ�
    [SerializeField] public AudioSource JumpAudioSource; // jump�����Đ����邽�߂̃I�[�f�B�I�\�[�X
    [SerializeField] public AudioClip jumpClip; // jump���̌��ʉ�
    [SerializeField] public AudioSource AttackAudioSource; // �U�������Đ����邽�߂̃I�[�f�B�I�\�[�X
    [SerializeField] public AudioClip attackClip; // �U�����̌��ʉ�

    // Start is called before the first frame update
    void Start()
    {

        //�̗͂�錾
        currentHealth = maxHealth;
        //HP�o�[�ɍő�̗͂��Z�b�g
        healthBar.SetMaxHealth(maxHealth);
        this.b2d = GetComponent<BoxCollider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.Enemy = GameObject.Find("Enemy");
        
        //�t���[���Œ�
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
            //�Q�[���I�[�o�[�̏���
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

            //�j���[�g����
            case 0: //0~�ړ��n
                {
                    //���E�ړ��̓��͂ŕ����ƈړ��ʂ��l��
                    float x = Input.GetAxis("Horizontal");

                    //�L�����N�^�[���ړ�����
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);

                    //�A�j���[�V�����ɐ��l�𑗂�
                    anim.SetFloat("isSpeed", Mathf.Abs(x * speed));

                    //�L�����N�^�[�̑��x��0.1f�ȏォ�H
                    if (Mathf.Abs(rb.velocity.x) > 0.1f && isGround)
                    {
                        //�����������ĂȂ��Ȃ�
                        if (!footstepAudioSource.isPlaying)
                        {
                            //�炷
                            PlayFootstepSound();
                        }
                    }
                    else
                    {
                        footstepAudioSource.Stop();
                    }

                    if (x < 0)
                    {
                        //�����蔻������]������
                        this.transform.localScale = new Vector2(-4f, 4f);

                    }

                    if (x > 0)
                    {

                        //�����蔻������]������
                        this.transform.localScale = new Vector2(4f, 4f);

                    }

                    //�n�ʂɋ���A���W�����v�L�[����������
                    if (Input.GetButtonDown("Jump") && isGround)
                    {
                        //�W�����v����

                        Debug.Log("���1�֑J��");
                        stateNumber = 1;
                    }

                    // 1�b�o�����H
                    if ( TimeCounter > 1f)
                    {
                        //�n�ʂɂ���A���N���b�N������
                        if (Input.GetButtonDown("Fire1") && isGround && rb.velocity == Vector2.zero)
                        {

                            Debug.Log("���10�֑J��");
                            stateNumber = 10;
                        }
                    }
                    
                    
                }
                break;

            //�W�����v
            case 1:
                {
                    anim.SetBool("isJump", true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    JumpAudioSource.PlayOneShot(jumpClip);

                    Debug.Log("���2�֑J��");
                    stateNumber = 2;
                }
                break;

            //�W�����v������
            case 2:
                {
                    anim.SetBool("isJump", false);
                    Debug.Log("���0�֑J��");
                    stateNumber = 0;
                }
                break;
                
            case 10: //10~�U��
                {

                    // anim.SetTrigger("isDamage")���Đ�����Ă���ꍇ
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
                    {
                        Debug.Log("���0�֑J��");
                        stateNumber = 0;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        Attack();
                        AttackAudioSource.PlayOneShot(attackClip);
                        Debug.Log("" + playerAttackDamage);
                        Debug.Log("���15�֑J��");
                        TimeCounter = 0.0f;
                        stateNumber = 15;
                    }
                }            
                break;

            case 15: //�U����j���[�g������
                {
                     anim.SetBool("isAttack", false);
                     Debug.Log("���0�֑J��");
                     stateNumber = 0; 
                }
                break;
        }

        TimeCounter += Time.deltaTime;

    }
    private void FixedUpdate()
    {
       //�ڒn����

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
