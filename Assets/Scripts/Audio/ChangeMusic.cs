using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] private string nextSong;
    public static string currentSong;

    private void Start()
    {
        if(currentSong != nextSong)
        {
            AudioManager.StopStatic(currentSong);
            AudioManager.PlayStatic(nextSong);

            currentSong = nextSong;
        }
    }
}