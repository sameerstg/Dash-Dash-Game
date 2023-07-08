using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlag : MonoBehaviour
{
    public string nextSceneName;
    public bool lastLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // did the player enter our trigger?
        if (other.CompareTag("PlayerMovement"))
        {
            // is this the last level?
            if (lastLevel == true)
            {
                SceneManager.LoadScene(0);
            }
            // is there another scene/level we can go to?
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
