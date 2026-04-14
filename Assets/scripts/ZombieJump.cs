using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieJump : MonoBehaviour
{
    public Animator animator;
    public string playerTag = "Player";

    private bool isAttacking = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
    }
}