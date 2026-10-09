using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private int damage;
    private float knockback;
    private List<Collider> alreadyCollidedwith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedwith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider)
        {
            return;
        }
        if (alreadyCollidedwith.Contains(other))
        {
            return;
        }
        alreadyCollidedwith.Add(other);
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }
    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
