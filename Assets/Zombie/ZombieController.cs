using System;
using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour, Damage
{

    private Animator anim;
    private EnemyCounter counter;
    [SerializeField] private float Speed;
    private NavMeshAgent agent;
    private float life = 100;
    private CharControl target;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = FindFirstObjectByType<EnemyCounter>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        target = FindFirstObjectByType<CharControl>();
    }

    // Update is called once per frame
    void Update()
    {
       
        agent.SetDestination(target.transform.position);
        if (agent.pathPending) return;
        anim.SetFloat("Speed",agent.velocity.magnitude);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            LookTarget();
            startAttack();
        }

    }

    private void startAttack()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", true);
    }

    private void Attack()
    {
        Collider[] colliderHit = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider coll in colliderHit)
        {
            if (coll.TryGetComponent(out Damage damage))
            {
                damage.recieveDamage(50);
            }
        }
    }

    private void LookTarget()
    {
        Vector3 TargetDistance = (target.transform.position - transform.position).normalized;
        TargetDistance.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(TargetDistance);
        transform.rotation = lookRotation;
    }

    private void AttackEnd()
    {
        agent.isStopped = false;
        anim.SetBool("Attack", false);
    }

    public void recieveDamage(float damage)
    {

        life -= damage;
        if (life <= 0)
        {
            Destroy(this.gameObject);
            counter.enemiesKilled();

        }
    }

        private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position,attackRange);

    }
}

