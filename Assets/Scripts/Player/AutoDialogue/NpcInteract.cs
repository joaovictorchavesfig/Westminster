using System.Collections;
using UnityEngine;

public class NpcInteract : MonoBehaviour
{
    private PlayerMovement player;
    private DialogueManager dialogue;
    private bool reference = false;
    private bool onRadious = false;
    private bool canSkip = false;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if(reference && player != null && dialogue != null)
        {
            if((Mathf.Abs(transform.position.x - player.transform.position.x) <= 2f)
            && (Mathf.Abs(transform.position.y - player.transform.position.y) <= 1f))
            {
                onRadious = true;
            }
            else
            {
                onRadious = false;
            }

            if(onRadious)
            {
                if(canSkip)
                {
                    if(Input.GetButtonDown("Interact") || Input.GetButtonDown("Jump"))
                    {
                        dialogue.AdvanceDialogue();
                    }

                    if(dialogue.finishedDialogue)
                    {
                        canSkip = false;
                    }
                }
                else
                {
                    if(Input.GetButtonDown("Interact"))
                    {
                        dialogue.AdvanceDialogue();
                        canSkip = true;
                    }
                }
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        player = FindObjectOfType<PlayerMovement>();
        dialogue = FindObjectOfType<DialogueManager>();

        reference = true;
    }
}