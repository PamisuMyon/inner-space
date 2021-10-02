using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class LivingEntity : MonoBehaviour
{
    
    public float maxHp;
    public float hp;
    public bool isDead;
    public float hurtFlashDuartion = .2f;

    protected SpriteRenderer spriteRenderer;
    protected bool isFlashing;
    protected Sequence sequence;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        hp -= damage;
        hp = Mathf.Clamp(hp, 0f, maxHp);
        if (damage < 0)
        {
            var go = EffectPool.Instance.GetObject(EffectType.Heal);
            go.transform.position = transform.position;
            GameManager.Instance.PlayHeal();
        }
        else
        {
            if (!isFlashing)
            {
                isFlashing = true;
                sequence = DOTween.Sequence();
                var tweener1 = spriteRenderer.material.DOFloat(1f, "_FlashAmount", hurtFlashDuartion);
                var tweener2 = spriteRenderer.material.DOFloat(0f, "_FlashAmount", hurtFlashDuartion);
                sequence.Append(tweener1);
                sequence.Append(tweener2);
                sequence.OnComplete(() => isFlashing = false);
            }
            GameManager.Instance.PlayHit();
        }
        
        if (hp <= 0)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        if (sequence != null && !sequence.IsComplete())
        {
            sequence.Kill();
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
    }

}
