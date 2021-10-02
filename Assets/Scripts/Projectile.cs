using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{

    public float initialSpeed;
    public float angleSpeed;
    public float damage;

    public EffectType[] effects;

    float speed;
    SpriteRenderer childSprite;
    bool damageMaked;

    protected virtual void Start() 
    {
        speed = initialSpeed;
        childSprite = GetComponentInChildren<SpriteRenderer>();
        if (angleSpeed > 0 && childSprite != null)
        {
            childSprite.transform.Rotate(0, 0, Random.Range(0f, 360f) , Space.Self);
        }
    }

    protected virtual void Update()
    {
        if (speed > 0)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (angleSpeed > 0 && childSprite != null)
        {
            childSprite.transform.Rotate(0, 0, angleSpeed * Time.deltaTime, Space.Self);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (damageMaked) return;
        if (!other.isTrigger)    
        {
            var livingEntity = other.GetComponent<LivingEntity>();
            if (livingEntity)
            {
                livingEntity.TakeDamage(damage);
                damageMaked = true;
            }
            Explode();
        }
    }

    protected virtual void Explode()
    {
        foreach (var item in effects)
        {
            var go = EffectPool.Instance.GetObject(item);
            go.transform.position = transform.position;
        }
        gameObject.SetActive(false);
        damageMaked = false;
    }

}
