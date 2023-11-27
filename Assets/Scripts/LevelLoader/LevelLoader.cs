using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator anim;
    public float transitionTime = 1f;

    private static LevelLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static LevelLoader Instance
    {
        get { return instance; }
    }

    public void TravelTime()
    {
        if ((SceneManager.GetActiveScene().buildIndex % 2) == 0)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("Start");
        FindObjectOfType<HitStopController>().Stop(1f);
        yield return new WaitForSecondsRealtime(transitionTime);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }
}
