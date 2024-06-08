using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject tailPrefab;

    private float TickSpeed = 0.2f; // How often tick and move happens.
    private float MovementPerTick = 1.0f; // How much the snake moves per tick.

    private Vector3 nextTailPosition;

    private MovementDirection CurrentDirection = MovementDirection.None;

    private enum MovementDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Tick), 0.0f, TickSpeed);
    }

    private void Update() 
    {
        // Handle input.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentDirection = MovementDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentDirection = MovementDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentDirection = MovementDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentDirection = MovementDirection.Right;
        }
    }

    private void Tick() 
    {
        Move();
    }

    private void Move() 
    {
        switch (CurrentDirection)
        {
            case MovementDirection.Up:
                transform.position += Vector3.forward * MovementPerTick;
                nextTailPosition = transform.position + Vector3.back * MovementPerTick;
                break;
            case MovementDirection.Down:
                transform.position += Vector3.back * MovementPerTick;
                nextTailPosition = transform.position + Vector3.forward * MovementPerTick;
                break;
            case MovementDirection.Left:
                transform.position += Vector3.left * MovementPerTick;
                nextTailPosition = transform.position + Vector3.right * MovementPerTick;
                break;
            case MovementDirection.Right:
                transform.position += Vector3.right * MovementPerTick;
                nextTailPosition = transform.position + Vector3.left * MovementPerTick;
                break;
            case MovementDirection.None:
                break;
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        var treat = other.GetComponentInParent<Treat>();
        if(treat == null) return;
        Eat(treat);
    }

    private void Eat(Treat treat)
    {
        treat.GetEaten();

        Grow();
    }

    private void Grow()
    {
        Instantiate(tailPrefab, new Vector3(nextTailPosition.x, tailPrefab.transform.position.y, nextTailPosition.z), Quaternion.identity, transform);
    }
}