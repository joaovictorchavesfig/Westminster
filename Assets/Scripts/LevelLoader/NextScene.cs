using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    [SerializeField] int SceneAdd;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            FindObjectOfType<PlayerSceneManagement>().clearInstance();
            FindObjectOfType<LevelLoader>().ChangeScene(SceneAdd);
        }
    }

}
