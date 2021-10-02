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
            if (other.gameObject.tag == "Enemy")
            {
                GameManager.Instance.UpdateCoin(-1);
            }
            else if (other.gameObject.tag == "Player" || other.tag == "Boss")
            {
                GameManager.Instance.UpdateCoin(1);
            }
            coinAudio.pitch = Random.Range(0.8f, 1.2f);
            coinAudio.Play();
            Explode();
        }
    }
}
