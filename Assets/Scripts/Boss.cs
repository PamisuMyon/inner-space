using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity
{

    public ParticleSystem blow1;
    public ParticleSystem blow2;
    public ParticleSystem blow3;
    public ParticleSystem blow4;
    public float moveForce;
    public float deceleration;
    public float moveInterval;
    public float moveDuration;
    public float fireInterval;
    public float fireDuation;
    public float distance;

    [Space]
    public AudioSource jetPackAudio;

    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;
    PlayerWeapon weapon;
    
    

    Transform target;
    [Header("View Only")]
    [SerializeField]
    float moveCounter;
    [SerializeField]
    float moveCoolDown;
    [SerializeField]
    float fireCounter;
    [SerializeField]
    float fireCoolDown;
    [SerializeField] bool canFire;
    [SerializeField] bool canMove;
    bool appearFinished = false;
    List<ParticleSystem> moveList;
    Camera cam;
    
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponent<PlayerWeapon>();
        moveList = new List<ParticleSystem>();

        weapon.bulletLayer = LayerMask.NameToLayer("EnemyBullet");
        cam = Camera.main;

        // target = GameObject.FindGameObjectWithTag("Player").transform;
        UIManager.Instance.UpdateBossHp(maxHp, hp);
    }

    void Update()
    {
        if (isDead) return;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;
        else
            target = null;

        if (target == null) 
        {
            animator.SetBool("IsFiring", false);
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration);
            return;
        }

        if (!appearFinished)
        {
            var dir = target.position - transform.position;
            if (dir.magnitude > distance)
            {
                Move(blow1, true);
                Move(blow2, true);
            }
            else
            {
                appearFinished = true;
                canMove = true;
                moveCounter = moveDuration;
                // Debug.Log("Finished");
            }
            return;
        }

        moveList.Clear();
        if (canMove)
        {
            var dir = target.position - transform.position;
            if (dir.magnitude >= distance)
            {
               moveList.Add(blow1);
               moveList.Add(blow2);
            }
            else if (transform.position.x < cam.transform.position.x + Random.Range(0f, 10f))
            {
                moveList.Add(blow3);
                moveList.Add(blow4);
            }
            if (target.position.y < transform.position.y)
            {
                if (!moveList.Contains(blow1))
                    moveList.Add(blow1);
                if (!moveList.Contains(blow3))
                    moveList.Add(blow3);
            }
            else
            {
                if (!moveList.Contains(blow2))
                    moveList.Add(blow2);
                if (!moveList.Contains(blow4))
                    moveList.Add(blow4);
            }

            moveCounter -= Time.deltaTime;
            if (moveCounter <= 0)
            {
                canMove = false;
                moveCoolDown = moveInterval;
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration);
            moveCoolDown -= Time.deltaTime;
            if (moveCoolDown <= 0)
            {
                canMove = true;
                moveCounter = moveDuration;
            }
        }
        Move(blow1, moveList.Contains(blow1));
        Move(blow2, moveList.Contains(blow2));
        Move(blow3, moveList.Contains(blow3));
        Move(blow4, moveList.Contains(blow4));
        // Debug.Log("Moving");
        if (moveList.Count == 0)
        {
            jetPackAudio.Stop();
        }
        else if (!jetPackAudio.isPlaying)
        {
            jetPackAudio.pitch = Random.Range(.8f, 1.2f);
            jetPackAudio.Play();
        }

        if (canFire)
        {
            Fire();
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                canFire = false;
                fireCoolDown = fireInterval;
            }
        }
        else
        {
            fireCoolDown -= Time.deltaTime;
            if (fireCoolDown <= 0)
            {
                canFire = true;
                fireCounter = fireDuation;
            }
        }
        animator.SetBool("IsFiring", canFire);
    }

    void Move(ParticleSystem blow, bool isMove)
    {
        if (isMove)
        {
            if (!blow.isPlaying)
                blow.Play();
            rb.AddForce(-blow.transform.forward * moveForce, ForceMode2D.Force);
        }
        else
        {
            if (!blow.isStopped)
                blow.Stop();
        }
    }

    void Fire()
    {
        if (canFire)
        {
            weapon.Fire();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.Instance.UpdateBossHp(maxHp, hp);
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Die");
    }

    public void OnDieAnimFinished()
    {
        var go = EffectPool.Instance.GetObject(EffectType.FleshExplode);
        go.transform.position = transform.position;
        go = EffectPool.Instance.GetObject(EffectType.Explode2);
        go.transform.position = transform.position;
        GameManager.Instance.GameEnd();
        Destroy(gameObject);
    }

}
