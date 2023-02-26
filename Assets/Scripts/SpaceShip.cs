using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : LivingEntity
{

    public enum Pattern
    {
        Forward, Kamikaze
    }

    public enum FireType
    {
        Line, Spread
    }

    public Pattern pattern;
    public FireType fireType;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float speed;
    public float shootNum;
    public float shootBurst;
    public float shootInterval;
    public float shootCoolDown;
    public float forwardTime;
    public float stayTime;
    public float kamikazeDis;
    public float damage = 10f;

    [Space]
    public AudioSource shootAudio;

    Rigidbody2D rb;
    Transform target;

    float shootCounter;
    float coolDownCounter;
    float shootNumCounter;
    bool kamikaze;
    int bulletLayer;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        
        bulletLayer = LayerMask.NameToLayer("EnemyBullet");

        transform.right = Vector2.left;

        shootCounter = shootInterval;
        coolDownCounter = shootCoolDown;
    }

    void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Forward();
            return;
        }
        else 
        {
            target = player.transform;
        }
        if (pattern == Pattern.Forward)
            Forward();
        else if (pattern == Pattern.Kamikaze)
            Kamikaze();
    }

    void Forward()
    {
        rb.velocity = speed * transform.right;
        Fire();
    }

    void Kamikaze()
    {
        if (kamikaze)
        {
            rb.velocity = speed * transform.right;
            Fire();
        }
        else
        {
            rb.velocity = speed * transform.right;
            var dir = target.position - transform.position;
            if (dir.magnitude < kamikazeDis)
            {
                kamikaze = true;
                transform.right = Utils.RandomSign() * dir;
            }
        }
    }

    void Fire()
    {
        if (bulletPrefab == null) return;
        if (coolDownCounter <= 0)
        {
            if (shootCounter <= 0)
            {
                DoFire();
                shootCounter = shootInterval;
                shootNumCounter++;
                if (shootNumCounter >= shootNum)
                {
                    coolDownCounter = shootCoolDown;
                    shootNumCounter = 0;
                }
            }
            else
            {
                shootCounter -= Time.deltaTime;
            }
        }
        else
        {
            coolDownCounter -= Time.deltaTime;
        }
    }

    void DoFire()
    {
        if (fireType == FireType.Line)
        {
            var go = Instantiate(bulletPrefab);
            go.layer = bulletLayer;
            go.transform.position = firePoint.position;
            go.transform.right = firePoint.right;
        }
        else if (fireType == FireType.Spread)
        {
            // var tarDir = target.position - transform.position;
            var dir = firePoint.right;
            Quaternion rot = Quaternion.AngleAxis(-15, Vector3.forward);
            for (int i = 0; i < shootBurst / 2; i++)
            {
                dir = rot * dir;
                var go = Instantiate(bulletPrefab);
                go.layer = bulletLayer;
                go.transform.position = firePoint.position;
                go.transform.right = dir;
            }
            rot = Quaternion.AngleAxis(15, Vector3.forward);
            dir = firePoint.right;
            for (int i = 0; i < shootBurst / 2; i++)
            {
                var go = Instantiate(bulletPrefab);
                go.layer = bulletLayer;
                go.transform.position = firePoint.position;
                go.transform.right = dir;
                dir = rot * dir;
            }
        }
        shootAudio.pitch = Random.Range(.8f, 1.2f);
        shootAudio.Play();
    }

    protected override void Die()
    {
        base.Die();
        var go = EffectPool.Instance.GetObject(EffectType.Explode2);
        go.transform.position = transform.position;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LivingEntity livingEntity = other.gameObject.GetComponent<LivingEntity>();
            if (livingEntity)
                livingEntity.TakeDamage(20f);
            Die();
        }
    }

}
