using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    public enum State
    {
        None, Normal, RandomList, Random, Medic
    }

    public float bulletSpeed;
    public float fireInterval;
    public float spread;
    public Transform firePoint;
    public bool fireBackwards;
    public float randomBulletDuration;

    [Space]
    public AudioSource shootAudio;
    
    [Space]
    public Projectile defaultBullet;
    // public Projectile cannonBullet;
    public Projectile healBullet;
    // public Projectile catBullet;
    // public Projectile cupBullet;
    // public Projectile alienBullet;
    // public Projectile astronautBullet;
    public List<Projectile> randomBullets;

    float fireCounter;
    public State state;
    [Header("View Only")]
    [SerializeField] List<Projectile> pool;
    int randomIndex;
    float randomCounter;
    SpriteRenderer spriteRenderer;
    public int bulletLayer;

    void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("PlayerBullet");
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        pool = new List<Projectile>();
        // state = State.Normal;
    }
    
    public void Fire()
    {
        if (fireCounter <= 0)
        {
            DoFire();
            // if (!shootAudio.isPlaying)
            // {
                shootAudio.pitch = Random.Range(.8f, 1.2f);
                shootAudio.Play();
            // }
            fireCounter = fireInterval;
        }
        fireCounter -= Time.deltaTime;
    }

    void DoFire()
    {
        Projectile go = null;
        if (state == State.Normal)
        {
            go = GetProjectile(defaultBullet.gameObject.name, defaultBullet);
            if (go.initialSpeed < 0)
            {
                go.initialSpeed = bulletSpeed;
            }
        }
        else if (state == State.Medic)
        {
            go = GetProjectile(healBullet.gameObject.name, healBullet);
        }
        else if (state == State.Random)
        {
            int index = Random.Range(0, randomBullets.Count);
            var prefab = randomBullets[index];
            go = GetProjectile(prefab.gameObject.name, prefab);
            if (go.initialSpeed < 0)
            {
                go.initialSpeed = bulletSpeed;
            }
        }
        else if (state == State.RandomList)
        {
            if (randomCounter <= 0)
            {
                var r = randomBulletDuration * .35f;
                randomCounter = randomBulletDuration + Random.Range(-r, r);
                // randomIndex = (randomIndex + 1) % randomBullets.Count;
                randomIndex = Random.Range(0, randomBullets.Count);
            }
            randomCounter--;
            var prefab = randomBullets[randomIndex];
            go = GetProjectile(prefab.gameObject.name, prefab);
            if (go.initialSpeed < 0)
            {
                go.initialSpeed = bulletSpeed;
            }
        }

        if (go != null)
        {
            Vector2 point = firePoint.position;
            go.transform.position = point;
            go.gameObject.layer = bulletLayer;
            
            var dir = firePoint.right;
            var angle = Random.Range(-spread, spread);
            dir = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
            go.transform.right = fireBackwards? -dir : dir;

            var gunfire = EffectPool.Instance.GetObject(EffectType.Gunfire1);
            gunfire.transform.position = point;
        }
    }

    Projectile GetProjectile(string name, Projectile prefab)
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy
                && item.gameObject.name.Contains(name))
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        
        var go = Instantiate(prefab, transform);
        pool.Add(go);
        return go;
    }

}
