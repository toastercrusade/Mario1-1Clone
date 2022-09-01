using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    /// <summary>
    /// The script that controls when the fireball should be killed
    /// </summary>
    
    void Start()
    {
        /// <summary>
        /// Kills the fireball if it still exists after ten seconds
        /// </summary>
        
        Invoke(nameof(Kill), 10);   
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// Controls when the fireball should be destroyed
        /// </summary>
        /// <param name="col">The fireball's collider</param>

        // Checks if the trigger collider that was entered should destroy the fireball
        if (col.CompareTag("Death") ^ col.CompareTag("ChangeDirection") ^ col.CompareTag("Enemy"))
        {
            // Destroys the fireball after 0.05 seconds
            Invoke(nameof(Kill), 0.05f);
        }
    }

    private void Kill()
    {
        /// <summary>
        /// Destroys the fireball
        /// </summary>
        
        Destroy(gameObject);
    }

}
