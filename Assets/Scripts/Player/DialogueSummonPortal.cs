using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSummonPortal : MonoBehaviour
{
    private DialogueManager dialogue;
    [SerializeField] private GameObject portal;
    private bool reference = false;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if(reference && dialogue.finishedDialogue)
        {
            portal.SetActive(true);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        dialogue = FindObjectOfType<DialogueManager>();

        reference = true;
    }

}
