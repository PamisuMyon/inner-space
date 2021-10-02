using UnityEngine;

public class Alien : LivingEntity
{

    public float startForce;
    public float triggerDuration;
    public Scroller scroller;

    float triggerCounter;
    Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        // var force = Random.insideUnitCircle * startForce;
        // rb.AddForce(force, ForceMode2D.Impulse);   
        rb.angularVelocity = startForce;
    }

    protected override void Die()
    {
        base.Die();
        var go = EffectPool.Instance.GetObject(EffectType.FleshExplode);
        go.transform.position = transform.position;
        go = Instantiate(EffectPool.Instance.shockwavePrefab);
        go.transform.position = transform.position;
        GameManager.Instance.PlayShockwave();
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (scroller.end) return;
        if (other.tag == "Player" && hp == maxHp)
        {
            triggerCounter += Time.deltaTime;
            if (triggerCounter >= triggerDuration)
            {
                scroller.End();
                UIManager.Instance.HideAll();
                other.GetComponent<Player>().turnAlien = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            triggerCounter = 0f;
        }
    }
}
