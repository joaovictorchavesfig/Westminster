using System.Collections;
using UnityEngine;

public class InteractionNextScene : MonoBehaviour
{
    [SerializeField] int SceneAdd;
    private bool onRange = false;

    private void Awake()
    {
        onRange = false;
    }

    private void Update()
    {
        if (onRange && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<PlayerSceneManagement>().clearInstance();
            FindObjectOfType<LevelLoader>().ChangeScene(SceneAdd);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            onRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            onRange = false;
        }

    }

}
