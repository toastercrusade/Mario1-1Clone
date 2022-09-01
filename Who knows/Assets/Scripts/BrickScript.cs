using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    /// <summary>
    ///  This is the class that controls what happens when the brick gets hit by the player
    /// </summary>

    public GameObject sk; // The scorekeeper
    ScoreKeeperScript sks; // The scorekeeper script
    
    public bool hasStar = false; // Whether the brick will release a star or not when broken, false by default

    void Start()
    {
        /// <summary>
        /// Sets sks to the scorekeeper script inside of the scorekeeper, has to be set from the inspector
        /// </summary>
        
        sks = sk.GetComponent<ScoreKeeperScript>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// Destroys the block when it enters the player's trigger collider
        /// </summary>
        /// <param name="col"> The brick's collider </param>
        
        // Checks first to see if the block has a star
        if (!hasStar)
        {
            // Destroy this game object if it enters the breakblock trigger collider
            if(col.CompareTag("BreakBlock"))
            {           
                Invoke(nameof(Kill), 0.05f);
            }
        }
        
    }

    private void Kill()
    {
        /// <summary>
        /// Destroys the block and updates the score
        /// </summary>
        
        sks.score += 10;
        Destroy(gameObject);
    }

}
