using UnityEngine;

public class Coin : Projectile
{
    public AudioSource coinAudio;

    protected override void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.isTrigger)    
        {
            var livingEntity = other.GetComponent<LivingEntity>();
            if (livingEntity)
                livingEntity.TakeDamage(damage);
            if (other.gameObject.CompareTag("Enemy"))
            {
                GameManager.Instance.UpdateCoin(-1);
            }
            else if (other.gameObject.CompareTag("Player") || other.CompareTag("Boss"))
            {
                GameManager.Instance.UpdateCoin(1);
            }
            coinAudio.pitch = Random.Range(0.8f, 1.2f);
            coinAudio.Play();
            Explode();
        }
    }
}
