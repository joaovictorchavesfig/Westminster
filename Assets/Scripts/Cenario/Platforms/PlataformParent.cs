using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
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
}
