using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float health;
    RagdollManager ragdollManager;
    EnemyAI enemyAI;
    [HideInInspector] public bool isDead;
    public Action onDeath;

    private void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0) EnemyDeath();
            else Debug.Log("Hit");
        }
    }

    void EnemyDeath()
    {
        isDead = true;
        ragdollManager.TriggerRagdoll();
        enemyAI.enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Animator>().enabled = false;
        onDeath?.Invoke();
        Destroy(gameObject, 5f);
        Debug.Log("Death");
    }
}