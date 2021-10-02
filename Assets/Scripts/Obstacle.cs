using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : LivingEntity
{

    public float initialSpeed;
    public float angularSpeed;
    public float speed;
    public float damage;
    public EffectType[] effects;

    Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        rb.angularVelocity = angularSpeed - Random.Range(-angularSpeed * .1f, angularSpeed * .1f);
    }

    void Update()
    {
        if (isDead) return;
        rb.velocity = Vector2.left * speed;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        var livingEntity = other.gameObject.GetComponent<LivingEntity>();
        if (livingEntity)
            livingEntity.TakeDamage(damage);
        Die();   
    }

    protected override void Die()
    {
        base.Die();
        foreach (var item in effects)
        {
            var go = EffectPool.Instance.GetObject(item);
            go.transform.position = transform.position;
        }
        Destroy(gameObject);
    }

}
