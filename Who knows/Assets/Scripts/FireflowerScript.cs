using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflowerScript : MonoBehaviour
{
    /// <summary>
    /// This script basically just makes the fire flower not get moved by anything else
    /// </summary>

    Rigidbody2D rb; // The physics controller for the fireflower
    CapsuleCollider2D cc; // The collider

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Sets rb to the fireflower's rigidbody
        cc = GetComponent<CapsuleCollider2D>(); // Sets cc to the fireflower's collider
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// The flower does not collide (for movement) with the player, the enemies, and other powerups
        /// This is so that the fireflower does not get pushed around
        /// </summary>
        
        if (col.CompareTag("Enemy") ^ col.CompareTag("PowerUp") ^ col.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(col, cc, true);
        }
    }
}
