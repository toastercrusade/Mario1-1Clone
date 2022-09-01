using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    /// <summary>
    /// A script that controls the camera movement
    /// </summary>
    
    public GameObject player; // The player gameobject
    public Vector3 offset; // How much to offset the camera position from the players position, set in the inspector

    void Start()
    {
        /// <summary>
        /// Sets player to the player game object
        /// </summary>
        
        player = GameObject.Find("Player");
    }

    void Update()
    {
        /// <summary>
        /// Changes the position of the camera based off of the player's position
        /// </summary>
        transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, offset.z);
    }
}
