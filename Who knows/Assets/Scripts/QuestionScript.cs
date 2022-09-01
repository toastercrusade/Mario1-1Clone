using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionScript : MonoBehaviour
{
    /// <summary>
    /// Controls what happens to the question mark block when it is hit by the player, including which powerups spawn (if any), and how it breaks
    /// </summary>

    public GameObject brokenQuestion; // The 'broken' version of this block, is spawned when this block breaks
    public GameObject mushroom; // The mushroom powerup that is spawned
    public GameObject player; // The player game object
    public GameObject fireFlower; // The fireflower powerup that is spawned
    public GameObject sk; // The scorekeeper
    ScoreKeeperScript sks; // The scorekeeper script
    PlayerScript ps; // The player script

    public bool hasPowerUp = false; // If the question mark block contains a powerup or not, false by default

    void Start()
    {
        ps = player.GetComponent<PlayerScript>(); // Sets ps to the playerscript
        sks = sk.GetComponent<ScoreKeeperScript>(); // Sets sks to the scorekeeper script from sk
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// Determines if the block is broken, updates the score, and spawns the necessary game objects
        /// </summary>
        /// 
        /// <param name="col">The question mark block's collider</param>
        
        if (col.CompareTag("BreakBlock"))
        {
            // Update the score
            sks.score += 15;

            // Determines to spawn a powerup or not, spawns the broken questionmark block, and destroys this game object
            if (!hasPowerUp)
            {
                // Instantiate broke question block
                Instantiate(brokenQuestion, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                // Destroy this object
                Destroy(gameObject);
            } else {
                if (!ps.big & !ps.fireFlower)
                {
                    // Spawn in mushroom if the player has no powerup
                    Debug.Log("Mush");
                    Debug.Log(ps.big);
                    Instantiate(brokenQuestion, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                    Instantiate(mushroom, new Vector3(transform.position.x, transform.position.y + 1.5f, 0), transform.rotation);
                    Destroy(gameObject);
                }
                if (ps.big)
                {
                    // Spawn in fireflower if the player already has the mushroom
                    Debug.Log("Fire");
                    Instantiate(brokenQuestion, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                    Instantiate(fireFlower, new Vector3(transform.position.x, transform.position.y + 1.5f, 0), transform.rotation);
                    Destroy(gameObject);
                }
                
            }
            
        }
    }

}
