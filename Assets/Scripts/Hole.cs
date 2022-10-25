using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
    float holeRadius;
    BallDestroyEvent ballDestroyEvent;

    void Start()
    {
        // Get radius of hole
        ballDestroyEvent = new();
        holeRadius = GetComponent<CircleCollider2D>().radius;
        EventManager.AddBallDestroyInvoker(this);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Check if object center is inside hole
        if (Vector2.Distance(transform.position, other.transform.position) <= holeRadius)
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<Rigidbody2D>().angularVelocity = 0;
            other.transform.position = transform.position;
            ballDestroyEvent.Invoke(other.gameObject);
        }
    }
    public void AddBallDestroyListener(UnityAction<GameObject> listener) { ballDestroyEvent.AddListener(listener); }
}
