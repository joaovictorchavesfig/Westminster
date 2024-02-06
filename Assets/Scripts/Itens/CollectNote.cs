using UnityEngine;

public class CollectNote : MonoBehaviour
{
    [SerializeField] private GameObject carta;
    [SerializeField] private int index;
    public static CollectNote instance;

    private void Start()
    {
        instance = this;

        if(index == 1 && NoteMenu.carta1)
        {
            gameObject.SetActive(false);
        }
        if(index == 2 && NoteMenu.carta2)
        {
            gameObject.SetActive(false);
        }
        if(index == 3 && NoteMenu.carta3)
        {
            gameObject.SetActive(false);
        }
        if(index == 4 && NoteMenu.carta4)
        {
            gameObject.SetActive(false);
        }
        if(index == 5 && NoteMenu.carta5)
        {
            gameObject.SetActive(false);
        }
        if(index == 6 && NoteMenu.carta6)
        {
            gameObject.SetActive(false);
        }
        if(index == 7 && NoteMenu.carta7)
        {
            gameObject.SetActive(false);
        }
        if(index == 8 && NoteMenu.carta8)
        {
            gameObject.SetActive(false);
        }
        if(index == 9 && NoteMenu.carta9)
        {
            gameObject.SetActive(false);
        }
        if(index == 10 && NoteMenu.carta10)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            Time.timeScale = 0f;
            PauseMenu.gamePaused = true;

            carta.SetActive(true);
            PauseMenu.noteActive = true;

            switch(index)
            {
                case 1:

                    NoteMenu.carta1 = true;

                break;

                case 2:

                    NoteMenu.carta2 = true;
                    
                break;

                case 3:

                    NoteMenu.carta3 = true;
                    
                break;

                case 4:

                    NoteMenu.carta4 = true;
                    
                break;

                case 5:

                    NoteMenu.carta5 = true;

                break;

                case 6:

                    NoteMenu.carta6 = true;
                    
                break;

                case 7:

                    NoteMenu.carta7 = true;
                    
                break;

                case 8:

                    NoteMenu.carta8 = true;
                    
                break;

                case 9:

                    NoteMenu.carta9 = true;
                    
                break;

                case 10:

                    NoteMenu.carta10 = true;
                    
                break;
            }
        }
    }

    public void Fechar()
    {
        carta.SetActive(false);
        PauseMenu.noteActive = false;

        Time.timeScale = 1f;
        PauseMenu.gamePaused = false;

        gameObject.SetActive(false);
    }
}