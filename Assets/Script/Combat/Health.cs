using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    private bool isInvunerable;
    public event Action OntakeDamege;
    public event Action OnDie;
    // Start is called before the first frame update

    public bool IsDead => health == 0;
    public void SetInvunerable(bool isInvunerable)
    {
        this.isInvunerable = isInvunerable;
    }
    private void Start()
    {
        health = maxHealth;
    }
    public void DealDamage(int damage)
    {
        if (health == 0) { return; }
        if (isInvunerable) { return; }
        health = Mathf.Max(health - damage, 0);

        OntakeDamege?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();
        }
        Debug.Log(health);
    }

}
