using UnityEngine;

public class SteveFirst : MonoBehaviour
{
    void Start()
    {
        if(PlayerMovement.firstTimeTravel)
        {
            gameObject.SetActive(false);
        }
    }
}