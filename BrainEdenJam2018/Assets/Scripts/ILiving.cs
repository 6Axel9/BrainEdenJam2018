using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiving<T>
{
    void Kill();
    void Heal(T amount);
    void Damage(T amount);
    bool IsDead();
}

public partial class Humanoid
{
    [SerializeField] private float m_health = 100;
    [SerializeField] private float m_maxHealth = 100;
    [SerializeField] private bool m_isDead = false;

    public float Health {
        get { return m_health; }
    }

    public bool IsDead {
        get { return m_isDead; }
    }

    public void Kill() {
        m_isDead = true;
    }

    public void Heal(float amount) {
        m_health += amount;
        if(m_health > m_maxHealth) {
            m_health = m_maxHealth;
        }
    }

    public void Damage(float amount) {
        m_health -= amount;
    }
}

public partial class Enemy
{
    [SerializeField] private float m_health = 100;
    [SerializeField] private float m_maxHealth = 100;
    [SerializeField] private bool m_isDead = false;

    public float Health
    {
        get { return m_health; }
    }

    public bool IsDead
    {
        get { return m_isDead; }
    }

    public void Kill()
    {
        m_isDead = true;
    }

    public void Heal(float amount)
    {
        m_health += amount;
        if (m_health > m_maxHealth)
        {
            m_health = m_maxHealth;
        }
    }

    public void Damage(float amount)
    {
        m_health -= amount;
    }
}
