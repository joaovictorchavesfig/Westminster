using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private GameObject painel;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null )
        {
            painel = player.transform.Find("Keyboard Up").gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player") && painel != null)
        {
            painel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(collision.CompareTag("Player") && painel != null)
        {
            painel.SetActive(false);
        }
    }
}