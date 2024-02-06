using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CogMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float waitTime;
    Rigidbody2D body;
    bool ticking;
    bool waiting;

    PlayerMovement playerMovement;
    private Rigidbody2D playerRb;

    private static List<Vector3> existingPositions = new List<Vector3>();

    private void Awake()
    {
        Vector3 currentPosition = transform.position;

        if (existingPositions.Contains(currentPosition))
        {
            Destroy(gameObject);
            return;
        }

        existingPositions.Add(currentPosition);
        GameObject.DontDestroyOnLoad(gameObject);

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

    }

    public static void ClearExistingPositions()
    {
        existingPositions.Clear();
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ticking = false;
        waiting = false;

    }

    void FixedUpdate()
    {

        if ((SceneManager.GetActiveScene().buildIndex % 2) != 0)
        {
            if(!waiting)
            {
                body.angularVelocity = rotationSpeed;
            }
            else
            {
                body.angularVelocity = 0;
            }
            if(!ticking)
            {
                StartCoroutine(ClockTick());
            }
        }
        else
        {
            body.angularVelocity = 0;
        }
    }

    IEnumerator ClockTick()
    {
        ticking = true;
        yield return new WaitForSeconds(waitTime);
        waiting = !waiting;
        ticking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playerMovement.isDashing && playerRb.position.y >= body.position.y)
            {
                collision.transform.parent = this.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
            GameObject.DontDestroyOnLoad(collision.gameObject);
        }
    }
}
