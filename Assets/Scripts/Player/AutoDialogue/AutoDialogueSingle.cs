using UnityEngine;

public class AutoDialogueSingle : MonoBehaviour
{
    private DialogueManager dialogue;
    private bool canSkip = false;

    private void Start()
    {
        if(PlayerMovement.firstTimeTravel)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if((Input.GetButtonDown("Interact") || Input.GetButtonDown("Jump")) && canSkip)
        {
            if(dialogue != null)
            {
                dialogue.AdvanceDialogue();
            }
        }

        if(dialogue != null && dialogue.finishedDialogue)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            dialogue = FindObjectOfType<DialogueManager>();
            dialogue.AdvanceDialogue();
            canSkip = true;
        }
    }
}