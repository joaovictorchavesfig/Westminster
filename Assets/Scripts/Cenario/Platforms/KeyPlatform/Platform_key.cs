using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_key : MonoBehaviour
{
    PlayerMovement playerMovement;
    public Animator platformAnim;
    private Animator anim;
    private bool lit = false;

    public Platform_key_movement platformMovement;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            lit = true;
            anim.SetBool("Lit", lit);
            platformAnim.SetBool("Lit", lit);
            platformMovement.start = true;
        }
    }

    public void resetPlatform()
    {
        //if(lit)
        //{
            lit = false;
            anim.SetBool("Lit",lit);
            platformAnim.SetBool("Lit",lit);
            platformMovement.resetPos();
        //}
    }
}
