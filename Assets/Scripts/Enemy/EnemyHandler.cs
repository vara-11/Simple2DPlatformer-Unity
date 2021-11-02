using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            animator.SetTrigger("isDestroy");
            Invoke("DestroyEnemy", 1f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}