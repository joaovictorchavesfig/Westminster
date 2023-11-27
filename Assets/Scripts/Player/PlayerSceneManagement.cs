using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneManagement : MonoBehaviour
{
    private static PlayerSceneManagement instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
