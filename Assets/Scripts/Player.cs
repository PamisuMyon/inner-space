using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{

    // public enum State
    // {
    //     None, Normal, NoGravity
    // }

    [Space]
    public ParticleSystem blow1;
    public ParticleSystem blow2;
    public ParticleSystem blow3;
    public ParticleSystem blow4;
    public float moveForce;
    public float deceleration;
    public bool turnAlien;

    [Space]
    public AudioSource jetPackAudio;

    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;
    // TriggerArea2D triggerArea;
    PlayerInput input;
    [HideInInspector] public PlayerWeapon weapon;

    protected override void Start()
    {
        base.Start();
        weapon = GetComponent<PlayerWeapon>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        input = GetComponent<PlayerInput>();
        // triggerArea = GetComponentInChildren<TriggerArea2D>();
        // triggerArea.TriggerEnter2D += OnInteractTriggerEnter;
        // triggerArea.TriggerStay2D += OnInteractTriggerStay;
        // triggerArea.TriggerExit2D += OnInteractTriggerExit;

        // state = State.Normal;

        GameManager.Instance.RegisterPlayer(this);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isDead) return;
        animator.SetBool("IsAlien", turnAlien);
        Fire();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        Move(blow1, input.move1);
        Move(blow2, input.move2);
        Move(blow3, input.move3);
        Move(blow4, input.move4);

        if (!input.move1 && !input.move2 && !input.move3 && !input.move4)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration);
            jetPackAudio.Stop();
        }
    }

    void Move(ParticleSystem blow, bool isMove)
    {
        if (isMove)
        {
            if (!blow.isPlaying)
                blow.Play();
            rb.AddForce(-blow.transform.forward * moveForce, ForceMode2D.Force);
            if (!jetPackAudio.isPlaying)
            {
                jetPackAudio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                jetPackAudio.Play();
            }
        }
        else
        {
            if (!blow.isStopped)
                blow.Stop();
        }
    }

    void Fire()
    {
        if (turnAlien) return;
        if (input.fire)
        {
            weapon.Fire();
        }
        animator.SetBool("IsFiring", input.fire);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.Instance.UpdateHp(maxHp, hp);
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
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
        // Destroy(gameObject);
    }

    public void Revive()
    {
        hp = maxHp;
        isDead = false;
        animator.Play("Idle");
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        UIManager.Instance.UpdateHp(maxHp, hp);
        turnAlien = false;
    }

}
