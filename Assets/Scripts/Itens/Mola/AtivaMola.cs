using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AtivaMola : MonoBehaviour
{
    PlayerMovement playerMovement;
    private Animator anim;

    [SerializeField] private float xVelocity;
    [SerializeField] private float yVelocity;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(triggerMola());
            playerMovement.isOnMola = true;
            col.attachedRigidbody.velocity = new Vector2(0, 0);
            col.attachedRigidbody.AddForce(new Vector2(xVelocity, yVelocity), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(triggerMola());
            playerMovement.isOnMola = true;
            col.attachedRigidbody.velocity = new Vector2(0, 0);
            col.attachedRigidbody.AddForce(new Vector2(xVelocity, yVelocity), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerMovement.isOnMola = false;

        }
    }

    private IEnumerator triggerMola()
    {
        anim.SetBool("Triggered", true);
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Triggered", false);
    }

}
