using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class ZombieJump : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;

    private bool isAttacking = false;
    private bool isJumping = false;
    private bool canAttack = true;
    private bool isStunned = false;

    private Transform player;
    private Transform playerRef;

    [Header("Ataque")]
    public float attackCooldown = 1f;
    public float pushForce = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        agent.autoTraverseOffMeshLink = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        animator.SetBool("isAttacking", isAttacking);

        if (isStunned)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (agent.isOnOffMeshLink && !isJumping && !isStunned)
        {
            StartCoroutine(Jump(agent.currentOffMeshLinkData));
        }

        // Rotación suave hacia el jugador
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    IEnumerator Jump(OffMeshLinkData link)
    {
        isJumping = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = link.endPos + Vector3.up * agent.baseOffset;

        float duration = 0.5f;
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
        if (other.CompareTag("Player") && canAttack && !isStunned)
        {
            player = other.transform;
            agent.isStopped = true;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;
        isAttacking = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        animator.Play("Ataque");

        playerRef = player;
        PushPlayer();

        yield return new WaitForSeconds(attackCooldown * 0.5f);

        if (playerRef != null)
        {
            DealDamage();
        }

        yield return new WaitForSeconds(attackCooldown * 0.5f);

        isAttacking = false;
        agent.isStopped = false;

        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }

    public void DealDamage()
    {
        if (playerRef == null) return;

        PlayerMovement pm = playerRef.GetComponent<PlayerMovement>();

        if (pm != null && !pm.HasWeapon)
        {
            GameManager.Instance.TakeDamage(1);
        }
    }

    void PushPlayer()
    {
        if (player == null) return;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        dir.Normalize();

        Vector3 force = dir * pushForce * 3f;
        force += Vector3.up * (pushForce * 0.7f);

        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
        playerRb.AddForce(force, ForceMode.Impulse);
    }

    public void Stun(float duracion)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.Log("Zombie inactivo, no se puede stunear");
            return;
        }
        Debug.Log("Iniciando stun en ZombieJump");
        StartCoroutine(StunRoutine(duracion));
    }

    IEnumerator StunRoutine(float duracion)
    {
        Debug.Log("StunRoutine iniciado");
        isStunned = true;
        canAttack = false;
        isAttacking = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        yield return new WaitForSeconds(duracion);

        Debug.Log("StunRoutine terminado");
        isStunned = false;
        canAttack = true;
        agent.isStopped = false;
    }

    public void TakeHit()
    {
        StartCoroutine(MuerteRoutine());
    }

    IEnumerator MuerteRoutine()
    {
        canAttack = false;
        isAttacking = false;
        agent.isStopped = true;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        animator.Play("Muerte2");

        yield return new WaitForSeconds(2.2f);

        gameObject.SetActive(false);
        GameManager.Instance.CheckWinCondition();
        Destroy(gameObject);
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