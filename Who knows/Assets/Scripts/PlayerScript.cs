using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    /// <summary>
    /// Controls all the movement, powerups, direction, if it is alive, and trigger colliders for the player
    /// </summary>

    Rigidbody2D rb; // The physics controller for the player

    string direction; // Whcih direction the sprite needs to face
    public LayerMask groundlayer; // The sprite layer of the ground
    public LayerMask blocklayer; // The sprite layer where the bricks and question mark blocks are
    public GameObject capsule; // The capsule that makes up the body of the player
    public GameObject fireball; // The fireball the player spawns when they use the fire flower
    public GameObject scoreKeeper; // The scorekeeper game object
    public GameObject cameraBoi; // The camera game objecct
    ScoreKeeperScript sks; // The scorekeeper script

    public float fallMultiplier = 7f; // How fast the player falls |TODO: jump stills a little floaty
    public float lowJumpModifier = 6.5f; // The fall multiplier if the player only jumps a little
    private float distToGround = 1.25f; // How far the player can be from the ground before they are able to jump again
    private bool canJump = true; // Can the player jump
    public float runSpeed = 2500.0f; // How fast the player moves
    public float jumpForce = 20; // How much vertical force is added to the player when they jump
    bool facingRight = true; // Is the player facing right
    bool mushroomCheck = false; // Does the player have a mushroom, if so only update the score once
    bool fireFlowerCheck = false; // Does the player have a fireflower, if so only update the score once
    public bool big = false; // Does the player have a mushroom
    public bool fireFlower = false; // Does the player have fire flower
    bool killable = true; // Can the player currently be killed

    void Start()
    {
        /// <summary>
        /// Sets rb and sks to their corresponding game object, and tells the game which direction the player starts facing
        /// </summary>
        
        rb = GetComponentInChildren<Rigidbody2D>(); // Sets rb to the player's rigidbody
        sks = scoreKeeper.GetComponent<ScoreKeeperScript>(); // Sets sks to the scorekeeper(sk)'s script

        direction = "right"; // Tells the ggame the player is facing right
    }

    void Update()
    {
        /// <summary>
        /// Controls player movement, which way the player's sprite is facing, and what powerup they have.
        /// </summary>
        
        // Checks to see if the player is pressing one of the buttons for movement
        if (Input.GetKey(KeyCode.A))
        {
            // Starts moving left by adding force to the player
            rb.AddForce(new Vector2(-runSpeed * Time.deltaTime, 0f), ForceMode2D.Force);
            // Tells the game that the sprite should be facing left
            direction = "left";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Starts moving right by adding force to the player
            rb.AddForce(new Vector2(runSpeed * Time.deltaTime, 0f), ForceMode2D.Force);
            // Tells the game that the sprite should be facing right
            direction = "right";
        }

        if (Input.GetKey(KeyCode.W) & (IsGrounded() ^ IsOnBlock()) & canJump)
        {
            // Causes the player to jump by adding vertical force (Jump force)
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            // The player can no longer jump until they hit the ground
            canJump = false;
        }

        // Is the player pressing the button to use the fireflower
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Does the player have a fire flower
            if (fireFlower)
            {
                // What direction is the player facing
                if (direction == "right")
                {
                    // Create a fireball to the right of the player
                    GameObject fb = Instantiate(fireball, new Vector3(transform.position.x + 1f, transform.position.y, 0), transform.rotation);
                    Rigidbody2D rb = fb.GetComponent<Rigidbody2D>(); // Gets that fireball's rigidbody
                    // Make that fireball move to the right by adding force
                    rb.velocity = new Vector2(7, rb.velocity.y);
                } else
                {
                    // Creates a fireball to the left of the player
                    GameObject fb = Instantiate(fireball, new Vector3(transform.position.x - 1f, transform.position.y, 0), transform.rotation);
                    Rigidbody2D rb = fb.GetComponent<Rigidbody2D>(); // Gets that fireball's rigidbody
                    // Make that fireball move to the left by adding force
                    rb.velocity = new Vector2(-7, rb.velocity.y);
                }
            }
        }

        // If the player is already facing right but they need to face left
        if (direction == "right" & !facingRight)
        {
            // Flip the player (horizontally)
            Flip();
        }
        else if (direction == "left" & facingRight) // If the player is facing left but needs to face right
        {
            // Flip the player (horizontally)
            Flip();
        }

        // If the player is falling
        if (rb.velocity.y < 0)
        {
            // Cause the player to fall faster so that the jump is not floaty
            rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
        else if (rb.velocity.y > 0 & !Input.GetKey(KeyCode.W)) // If the player stops holding the jump button mid-way through their jump
        {
            // Stop rising and start falling
            rb.velocity += (lowJumpModifier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }

        // Did the player just pick up the mushroom
        if (mushroomCheck)
        {
            // Update the score
            sks.score += 100;
            // If the player picks up another mushroom they will update the score again
            mushroomCheck = false;
        }

        // Did the player pick up a fire flower
        if (fireFlowerCheck)
        {
            // Update their score
            sks.score += 200;
            // If the player picks up another fireflower they will update the score again
            fireFlowerCheck = false;
        }
    }

    bool IsGrounded()
    {
        /// <summary>
        /// Controls whether the player can jump or not by seeing if the player is touching the groud
        /// </summary>
        /// 
        /// <returns>
        /// True if the player is touching the ground, false if otherwise
        /// </returns>
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround, groundlayer); // A ray that is cast from the bottom of the player to see if it is touching the ground

        // Is the player touching the ground and they are not rising?
        if (hit == true & rb.velocity.y <= 0f)
        {
            // They can jump
            canJump = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsOnBlock()
    {
        /// <summary>
        /// Controls whether the player can jump or not by seeing i the player is standing on a block
        /// </summary>
        /// 
        /// <returns>
        /// True if the player is actively standing on a block, false if otherwise
        /// </returns>

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.6f, blocklayer); // A ray that is cast from the bottom of the player to see if it is touching a block (shorter than the ground one)
        
        // Is the player standing on the block and not rising?
        if (hit == true & rb.velocity.y <= 0f)
        {
            // The player can jump
            canJump = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Flip()
    {
        /// <summary>
        /// Controls which direction the player's sprite is facing
        /// </summary>
        
        Vector3 currentScale = gameObject.transform.localScale; // The current scale of the player's sprite
        // Flips the sprite to the right
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        // Tells the game which way the sprite is facing
        facingRight = !facingRight;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        /// <summary>
        /// Controls what happens to the player when it enters various trigger colliders, from changing powerup state to restarting the level
        
        // Does the player currently have a fire floewr
        if (fireFlower & killable)
        {
            // Did the player touch an enemy where it would get hurt
            if (col.CompareTag("Enemy"))
            {
                // The player no longer has a fire flower
                fireFlower = false;
                // Show that the player no longer has the fire flower
                capsule.GetComponent<SpriteRenderer>().color = Color.blue;
                // Make the player temporarily killable
                killable = false;
                Invoke(nameof(MakeKillable), 1f);
            }
        } else if (!big & killable) // If the player does not have a mushroom and can be hurt
        {
            // Did the player touch an enemy where it would get hurt
            if (col.CompareTag("Enemy"))
            {
                // Restart the level
                SceneManager.LoadScene("1-1");
            }
        } else if (big & killable) // Does the player have a mushroom and can be hurt
        {
            // Did the player touch an enemy where it would get hurt
            if (col.CompareTag("Enemy"))
            {
                // The player no longer has a mushroom
                big = false;
                // The player's sprite shows this change
                capsule.GetComponent<SpriteRenderer>().color = Color.red;
                // Temporarily make the player not killable
                killable = false;
                Invoke(nameof(MakeKillable), 1f);
            }
        }

        // Did the player enter a trigger collider that should instantly kill it (basically if they fall off the map)
        if (col.CompareTag("Death"))
        {
            // Restart the level
            SceneManager.LoadScene("1-1");
        }

        // Did the player pick up a mushroom
        if (col.CompareTag("MushroomTrigger"))
        {
            // Tell the game the player picked up a mushroom
            mushroomCheck = true;
            big = true;
            // Change the player's sprite to reflect this
            capsule.GetComponent<SpriteRenderer>().color = Color.blue;
            // Destroy the mushroom game object
            Destroy(col.gameObject.transform.parent.gameObject);
        } else if (col.CompareTag("FireFlowerTrigger"))
        {
            // Tell the game the player picked up a fireflower
            fireFlowerCheck = true;
            fireFlower = true;
            // Change the player's sprite to reflect this
            capsule.GetComponent<SpriteRenderer>().color = Color.yellow;
            // Destroy the fireflower game object
            Destroy(col.gameObject.transform.parent.gameObject);
        }

        // Did the player touch somewhere that should make it enter a pipe |TODO What if the player enters a sideways pipe
        if (col.CompareTag("TransportIn") & Input.GetKey(KeyCode.S))
            {
                // Move the player to where the pipe ends |TODO: this only allows for the one pipe, this should be standardized
                transform.position = new Vector2(120, -38);
            }  else if (col.CompareTag("TransportOut") & Input.GetKey(KeyCode.D)) // Did the player touch somewhere that should cause it to enter the room from a pipe
            {
                // Move the player to where the pipe ends
                transform.position = new Vector2(259, 3);
            }

    }

    private void MakeKillable()
    {
        /// <summary>
        /// Makes the player killable after being temporarily invincible
        /// </summary>
        
        killable = true;
    }
}