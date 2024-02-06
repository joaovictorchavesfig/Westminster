using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;
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
    }

    public static void ClearExistingPositions()
    {
        existingPositions.Clear();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}
