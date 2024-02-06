using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabroParent : MonoBehaviour
{
    private static List<Vector3> existingPositions = new List<Vector3>();
    private CandelabroFall candelabroFall;


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
        candelabroFall = GetComponentInChildren<CandelabroFall>();
    }

    public void CallFall()
    {
        StartCoroutine(candelabroFall.Fall());
    }

    public void CallReset()
    {
        candelabroFall.resetPos();
    }

    public static void ClearExistingPositions()
    {
        existingPositions.Clear();
    }

}
