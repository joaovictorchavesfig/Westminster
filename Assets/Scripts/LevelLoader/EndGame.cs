using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(13f);
        //transform.Find("WhiteFadeTransitionEffect").gameObject.SetActive(false);
        FindObjectOfType<LevelLoader>().ChangeSceneFixed(23);
    }
}
