using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeperScript : MonoBehaviour
{
    /// <summary>
    /// A script that keeps track of the player's score
    /// </summary>

    public Text txt; // What the score keeper text says
    public GameObject player; // What the player is, needs to be set in the inspector

    public float score = 0f; // the current score

    void Update()
    {
        /// <summary>
        /// Sets the text on the screen to the score
        /// </summary>
        //score = ps.score;

        txt.text = ("Score: " + score.ToString());
    }
}
