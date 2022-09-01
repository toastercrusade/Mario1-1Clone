using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{
    /// <summary>
    /// Controls when the mushroom should change direction, and when the game object should be destroyed
    /// </summary>

    Rigidbody2D rb; // The mushroom's physics controller
    BoxCollider2D bc; // The mushroom's collider

    bool check = false; // Should the mushroom be able to change direction
    float velocityDirection = -3.5f; // The movement direction and speed, left by default

    void Start()
    {
        /// <summary>
        /// Sets rb and bc to the correct components, and sets the movement direction
        /// </summary>
        
        rb = GetComponent<Rigidbody2D>(); // Sets rb to the mushroom's RigidBody component
        bc = GetComponent<BoxCollider2D>(); // Sets bc to the mushroom's BoxCollider2D component

        rb.velocity = new Vector2(velocityDirection, rb.velocity.y); // Makes the mushroom start moving, to the left by default
    }

    void Update()
    {
        /// <summary>
        /// If the mushroom can change direction, change it
        /// </summary>
        
        // Can the mushroom change direction?
        if (check)
        {
            // Change it's direction
            rb.velocity = new Vector2(velocityDirection, rb.velocity.y);
            // The mushroom can not change direction anymore
            check = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// When the mushroom enters a trigger collider, either changes direction or destroys the game object if necessary
        /// </summary>
         
        // Checks to see if the trigger collider will cause it to change direction, and if it is allowed to check for this
        if ((col.CompareTag("ChangeDirection") ^ (col.CompareTag("Enemy"))) & !check)
        {
            // Change the direction to be the opposite
            velocityDirection = -velocityDirection;
           // Let the mushroom change direction now
            check = true;

        // Checks to see if this trigger collider should destroy the game object
        } else if (col.CompareTag("Death"))
        {
            // Destroy the game object
            Kill();

        // Checks to see if it collides with an enemy, player, or another powerup
        } else if (col.CompareTag("Enemy") ^ col.CompareTag("PowerUp") ^ col.CompareTag("Player")) 
        {
            // Does not collide with any of those colliders
            Physics2D.IgnoreCollision(col, bc, true);
        }
    }

    private void Kill()
    {
        /// <summary>
        /// Destroys the game object
        /// </summary>
        
        Destroy(gameObject);
    }
}
