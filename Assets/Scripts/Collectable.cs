using UnityEngine;

public class Collectable : Projectile
{

    // public float collectSpeedLerp;

    // TriggerArea2D collectArea;
    // Transform target;

    // protected override void Update() 
    // {
    //     base.Update();
    //     if (target)
    //     {
    //         transform.position = Vector3.Lerp(transform.position, target.position, collectSpeedLerp * Time.deltaTime);
    //     }
    // }

    // protected override void Start() 
    // {
    //     collectArea.TriggerEnter2D += OnCollectTriggerEnter;
    // }

    // protected virtual void OnCollectTriggerEnter(Collider2D other)
    // {
    //     if (!other.isTrigger)
    //     {
    //         var livingEntity = other.GetComponent<LivingEntity>();
    //         if (livingEntity)
    //         {
    //             target = livingEntity.transform;
    //         }
    //     }
    // }
    
}
