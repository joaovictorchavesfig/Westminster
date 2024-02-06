using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandelabroTrigger : MonoBehaviour
{

    private CandelabroParent candelabroParent;
    private bool onRange = false;
    private bool toggled = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        candelabroParent = GetComponentInParent<CandelabroParent>();
        onRange = false;
    }

    private void Update()
    {
        if (onRange && Input.GetButtonDown("Interact"))
        {
            if(toggled)
            {
                anim.SetBool("Ispulled", false);
                toggled = false;
                candelabroParent.CallReset();
            }
            else
            {
                anim.SetBool("Ispulled", true);
                toggled = true;
                candelabroParent.CallFall();
            }
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
