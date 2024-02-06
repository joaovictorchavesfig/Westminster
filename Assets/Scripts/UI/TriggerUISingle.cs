using UnityEngine;

public class TriggerUISingle : MonoBehaviour
{
    [SerializeField] private GameObject painel;

    private void Start()
    {
        if(!PlayerMovement.firstTimeTravel)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            painel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            painel.SetActive(false);
        }
    }
}