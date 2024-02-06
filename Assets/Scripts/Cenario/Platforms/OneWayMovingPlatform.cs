using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneWayMovingPlatform : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;

    PlayerMovement playerMovement;

    public GameObject ways;
    public Transform[] wayPoints;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;
    private float movement;

    public float waitDuration;
    private bool waiting;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }

    }

    private void Start()
    {
        pointIndex = 1;
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;
        DirectionCalculate();
        waiting = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex % 2 == 0)
        {
            movement = 0f;
        }
        else if(Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            NextPoint();
        }
        else if(!waiting)
        {
            DirectionCalculate();
        }

    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, movement);
    }

    private void NextPoint()
    {
        transform.position = targetPos;
        movement = 0f;

        if(pointIndex == pointCount - 1) //Ultimo ponto
        {
            direction = -1;
        }

        if(pointIndex == 0) //Primeiro ponto
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
        waiting = false;
    }


    void DirectionCalculate()
    {
        movement = speed/10;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!playerMovement.isDashing)
            {
                collision.transform.parent = this.transform;
            }
            playerMovement.isOnPlatform = true;
            playerMovement.platformSpeed = speed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
            playerMovement.isOnPlatform = false;
            GameObject.DontDestroyOnLoad(collision.gameObject);
        }
    }

}
