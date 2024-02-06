using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private int SceneAdd;
    [SerializeField] private GameObject panel;
    private bool onRange = false;

    private void Awake()
    {
        onRange = false;
    }

    private void Update()
    {
        if (onRange && Input.GetButtonDown("Interact"))
        {
            if(Key.pickedKey)
            {
                FindObjectOfType<PlayerSceneManagement>().clearInstance();
                FindObjectOfType<LevelLoader>().ChangeScene(SceneAdd);
            }
            else
            {
                StartCoroutine(avisoTrancado());
            }
        }
    }

    private IEnumerator avisoTrancado()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        panel.SetActive(false);
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
