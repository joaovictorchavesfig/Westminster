using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animTime;
    public Animator animFade;
    public Animator animwhiteFade;
    public float transitionTime;

    private GameObject fade;
    private GameObject travelTime;
    private GameObject whiteFade;

    private static LevelLoader instance;

    private void Start()
    {
        fade = transform.Find("FadeTransitionEffect").gameObject;
        travelTime = transform.Find("PortalTransitionEffect").gameObject;
    }

    public static LevelLoader Instance
    {
        get { return instance; }
    }

    public void TravelTime()
    {
        fade.SetActive(false);
        travelTime.SetActive(true);
        animTime.SetTrigger("Start");
        if ((SceneManager.GetActiveScene().buildIndex % 2) == 0)
        {
            StartCoroutine(TimeTravelLoadLevel(SceneManager.GetActiveScene().buildIndex + 1, transitionTime));
        }
        else
        {
            StartCoroutine(TimeTravelLoadLevel(SceneManager.GetActiveScene().buildIndex - 1, transitionTime));
        }
    }

    public void ChangeScene(int levelIndex)
    {
        StartCoroutine(DeleteAll());
        fade.SetActive(true);
        travelTime.SetActive(false);
        animFade.SetTrigger("Start");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + levelIndex, 1f));
    }

    public void ChangeSceneFixed(int levelIndex)
    {
        StartCoroutine(DeleteAll());
        fade.SetActive(true);
        travelTime.SetActive(false);
        animFade.SetTrigger("Start");
        StartCoroutine(LoadLevel(levelIndex, 1f));
    }

    public void CallFinalScene()
    {
        fade.SetActive(false);
        travelTime.SetActive(false);
        whiteFade = transform.Find("WhiteFadeTransitionEffect").gameObject;
        whiteFade.SetActive(true);
        animwhiteFade.SetTrigger("Start");
        StartCoroutine(DeleteAll());
        StartCoroutine(LoadFinalScene(22, 1.8f));
    }


    public IEnumerator LoadLevel(int levelIndex, float time)
    {
        FindObjectOfType<HitStopController>().Stop(1f);
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }

    public IEnumerator TimeTravelLoadLevel(int levelIndex, float time)
    {
        FindObjectOfType<HitStopController>().Stop(1f);
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
        FindObjectOfType<HitStopController>().Stop(0.5f);
        Time.timeScale = 1f;
        StartCoroutine(UnloadScene(sceneIndex));
    }

    public IEnumerator LoadFinalScene(int levelIndex, float time)
    {
        FindObjectOfType<HitStopController>().Stop(2f);
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }


    private IEnumerator DeleteAll()
    {
        yield return new WaitForSecondsRealtime(1f);
        MovingPlataform.ClearExistingPositions();
        CogMovement.ClearExistingPositions();
        FallingPlatform.ClearExistingPositions();
        CandelabroParent.ClearExistingPositions();
        RoomManager.ClearExistingPositions();

        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o.scene.name == "DontDestroyOnLoad" && !o.CompareTag("AudioManager") && !o.CompareTag("Player"))
            {
                o.transform.parent = this.transform;
            }
            
        }
    }

    private IEnumerator UnloadScene(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

}
