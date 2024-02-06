using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneManagement : MonoBehaviour
{
    private static PlayerSceneManagement instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void clearInstance()
    {
        instance = null;
        StartCoroutine(clearGameObject());
    }

    private IEnumerator clearGameObject()
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }
}
