using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class ZombieGordo : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;

    private bool isAttacking = false;
    private bool isJumping = false;
    private bool canAttack = true;

    private Transform player;

    [Header("Ataque")]
    public float attackCooldown = 2f;
    public float pushForce = 14f;

    [Header("Movimiento")]
    public float speed = 2f;

    private int health = 2;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        agent.speed = speed;
        agent.autoTraverseOffMeshLink = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        animator.SetBool("isAttacking", isAttacking);
        agent.isStopped = isAttacking;

        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(Jump(agent.currentOffMeshLinkData));
        }
    }

    IEnumerator Jump(OffMeshLinkData link)
    {
        isJumping = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = link.endPos + Vector3.up * agent.baseOffset;

        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 nextPos = Vector3.Lerp(startPos, endPos, elapsed / duration);
            agent.Move(nextPos - transform.position);

            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        isJumping = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            player = other.transform;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;
        isAttacking = true;

        PushPlayer();

        yield return new WaitForSeconds(attackCooldown);

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null && !pm.HasWeapon)
        {
            GameManager.Instance.TakeDamage(1);
        }

        isAttacking = false;

        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }

    void PushPlayer()
    {
        if (player == null) return;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        dir.Normalize();

        Vector3 force = dir * pushForce * 4f;
        force += Vector3.up * (pushForce * 1.0f);

        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
        playerRb.AddForce(force, ForceMode.Impulse);
    }

    public void TakeHit()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            player = null;
        }
    }
}