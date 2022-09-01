using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaScript : MonoBehaviour
{
    /// <summary>
    /// Controls if the goomba should be moving, if the goomba should be killed, and updates score accordingly
    /// </summary>
    
    float velocityDirection = -3.5f; // Movement direction, left by default
    Rigidbody2D rb; // The physics controller for the goomba
    Renderer rend; // The game renderer
    public GameObject sk; // The scorekeeper game object
    ScoreKeeperScript sks; // The scorekeeper script

    bool check = false; // Stops the goomba from changing direction over and over
    bool move = false; // Should the goomba be moving or not

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>(); // Sets rb to the goomba's rigidbody
        rend = GetComponentInChildren<SpriteRenderer>(); // Sets rend to the game renderer
        sks = sk.GetComponent<ScoreKeeperScript>(); // Sets sks to the scorekeeper script from sk
    }

    void Update()
    {
        /// <summary>
        /// Checks to see if the goomba is currently rendered, the first time it becomes rendered it starts to move
        /// </summary>
        
        // Checks if the goomba is rendered
        if (rend.isVisible)
        {
            // Makes the goomba start moving
            move = true;
        }

        // Checks if the goomba is moving
        if (move)
        {
            // Starts the goomba movement
            rb.velocity = new Vector2(velocityDirection, rb.velocity.y);
        }
        
        // Checks to see if the goomba is ready to change direction
        if (check)
        {
            // Changes movement direction of goomba
            rb.velocity = new Vector2(velocityDirection, rb.velocity.y);
            check = false;
        }   
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// When the goomba enters a trigger collider, changes direction or dies when it needs to
        /// </summary>
        /// 
        /// <param name="col">The goomba's collider</param>
        
        // Checks to see if the trigger collider is one that should cause the goomba to change direction
        if ((col.CompareTag("ChangeDirection" ) ^ (col.CompareTag("Enemy"))) & !check)
        {
            // Change direction and sets check to false
            velocityDirection = -velocityDirection;
            check = true;
        }

        // Checks to see if the trigger collider is one that should cause the goomba to be destroyed
        if (col.CompareTag("Stomp") ^ col.CompareTag("Death"))
        {
            // Updates score and destroys the goomba
            sks.score += 50;
            Destroy(gameObject);
        }

        // Checks to see if the trigger collider is one that should cause the goomba to be destroyed
        if (col.CompareTag("Fireball"))
        {
            // Updates score and destroys the goomba
            sks.score += 50;
            Destroy(gameObject);
        }

    }

}
