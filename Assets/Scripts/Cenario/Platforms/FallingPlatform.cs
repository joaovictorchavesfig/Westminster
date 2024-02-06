using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay;
    [SerializeField] private float gravity;
    [SerializeField] private float fallSpeed;

    [SerializeField] private Rigidbody2D body;
    Vector3 initPos;
    PlayerMovement playerMovement;

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
    }

    public static void ClearExistingPositions()
    {
        existingPositions.Clear();
    }


    private void Start()
    {
        initPos = transform.position;
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().buildIndex % 2 == 0)
        {
            body.bodyType = RigidbodyType2D.Kinematic;
            body.velocity = Vector3.zero;
        }
    }

    public void resetPos()
    {
        body.bodyType = RigidbodyType2D.Kinematic;
        body.velocity = Vector3.zero;
        transform.position = initPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().buildIndex % 2 != 0)
        {
            playerMovement.fallingPlatform = this;
            StartCoroutine(Fall());
        }
    }


    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = gravity;
        body.velocity = new Vector2(body.velocity.x, -fallSpeed);
    }
}
