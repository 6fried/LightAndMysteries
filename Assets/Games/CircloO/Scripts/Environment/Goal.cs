using System;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGoalReached;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            onGoalReached.Invoke();
            Destroy(gameObject);
        }
    }
}
