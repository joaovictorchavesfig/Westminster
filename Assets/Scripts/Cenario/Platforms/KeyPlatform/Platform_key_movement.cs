using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform_key_movement : MonoBehaviour
{
    public float speed;
    PlayerMovement playerMovement;
    private Rigidbody2D playerRb;
    private float prevGravity;

    public GameObject TargetLocation;
    private bool waiting;
    public float waitTime;
    private Vector3 initPos;
    [HideInInspector] public bool start;
    private bool starting = false;


    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        waiting = true;
        initPos = this.transform.position;
        start = false;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex % 2 != 0 && !waiting)
        {
            transform.position = Vector2.MoveTowards(transform.position, TargetLocation.transform.position, speed / 10f);
        }
        else if (start && !starting)
        {
            StartCoroutine(startMoving());
        }
    }

    public IEnumerator startMoving()
    {
        starting = true;
        yield return new WaitForSeconds(waitTime);
        if(starting)
        {
            waiting = false;
            starting = false;
        }
    }

    public void resetPos()
    {
        starting = false;
        start = false;
        waiting = true;
        this.transform.position = initPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playerMovement.isDashing)
            {
                collision.transform.parent = this.transform;
            }
            playerMovement.isOnPlatform = true;
            playerMovement.platformSpeed = speed;
            prevGravity = playerRb.gravityScale;
            playerRb.gravityScale = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
            playerMovement.isOnPlatform = false;
            GameObject.DontDestroyOnLoad(collision.gameObject);
            playerRb.gravityScale = prevGravity;
        }
    }
}
