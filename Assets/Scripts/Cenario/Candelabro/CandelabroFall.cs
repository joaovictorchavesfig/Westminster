using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandelabroFall : MonoBehaviour
{
    [SerializeField] private float fallDelay;
    [SerializeField] private float gravity;

    [SerializeField] private Rigidbody2D body;
    private Vector3 initPos;
    private PlayerMovement playerMovement;
    private bool isFalling;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }


    private void Start()
    {
        isFalling = false;
        initPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            if (SceneManager.GetActiveScene().buildIndex % 2 == 0)
            {
                body.bodyType = RigidbodyType2D.Kinematic;
                body.velocity = Vector3.zero;
            }
            else
            {
                body.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public void resetPos()
    {
        isFalling = false;
        body.bodyType = RigidbodyType2D.Kinematic;
        body.velocity = Vector3.zero;
        transform.position = initPos;
    }


    public IEnumerator Fall()
    {
        playerMovement.candelabroFall = this;
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);
        body.bodyType = RigidbodyType2D.Dynamic;
        body.gravityScale = gravity;
    }
}
