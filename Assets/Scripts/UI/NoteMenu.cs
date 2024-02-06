using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteMenu : MonoBehaviour
{
    [Header("Cartas Coletadas")]
    public static bool carta1 = false;
    public static bool carta2 = false;
    public static bool carta3 = false;
    public static bool carta4 = false;
    public static bool carta5 = false;
    public static bool carta6 = false;
    public static bool carta7 = false;
    public static bool carta8 = false;
    public static bool carta9 = false;
    public static bool carta10 = false;

    [Header("Notas na Parede")]
    [SerializeField] private GameObject notas;
    [SerializeField] private GameObject nota1;
    [SerializeField] private GameObject nota2;
    [SerializeField] private GameObject nota3;
    [SerializeField] private GameObject nota4;
    [SerializeField] private GameObject nota5;
    [SerializeField] private GameObject nota6;
    [SerializeField] private GameObject nota7;
    [SerializeField] private GameObject nota8;
    [SerializeField] private GameObject nota9;
    [SerializeField] private GameObject nota10;

    [Header("Textos Abertos")]
    [SerializeField] private GameObject texto1;
    [SerializeField] private GameObject texto2;
    [SerializeField] private GameObject texto3;
    [SerializeField] private GameObject texto4;
    [SerializeField] private GameObject texto5;
    [SerializeField] private GameObject texto6;
    [SerializeField] private GameObject texto7;
    [SerializeField] private GameObject texto8;
    [SerializeField] private GameObject texto9;
    [SerializeField] private GameObject texto10;

    private void Start()
    {
        if(carta1)
        {
            nota1.SetActive(true);
        }
        if(carta2)
        {
            nota2.SetActive(true);
        }
        if(carta3)
        {
            nota3.SetActive(true);
        }
        if(carta4)
        {
            nota4.SetActive(true);
        }
        if(carta5)
        {
            nota5.SetActive(true);
        }
        if(carta6)
        {
            nota6.SetActive(true);
        }
        if(carta7)
        {
            nota7.SetActive(true);
        }
        if(carta8)
        {
            nota8.SetActive(true);
        }
        if(carta9)
        {
            nota9.SetActive(true);
        }
        if(carta10)
        {
            nota10.SetActive(true);
        }
    }

    public void Abrir1()
    {
        texto1.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar1()
    {
        texto1.SetActive(false);
        notas.SetActive(true);
        
    }

    public void Abrir2()
    {
        texto2.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar2()
    {
        texto2.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir3()
    {
        texto3.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar3()
    {
        texto3.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir4()
    {
        texto4.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar4()
    {
        texto4.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir5()
    {
        texto5.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar5()
    {
        texto5.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir6()
    {
        texto6.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar6()
    {
        texto6.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir7()
    {
        texto7.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar7()
    {
        texto7.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir8()
    {
        texto8.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar8()
    {
        texto8.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir9()
    {
        texto9.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar9()
    {
        texto9.SetActive(false);
        notas.SetActive(true);
    }

    public void Abrir10()
    {
        texto10.SetActive(true);
        notas.SetActive(false);
    }

    public void Fechar10()
    {
        texto10.SetActive(false);
        notas.SetActive(true);
    }


    public void Voltar()
    {
        AudioManager.StopStatic(ChangeMusic.currentSong);

        FindObjectOfType<LevelLoader>().ChangeSceneFixed(PauseMenu.currentSceneIndex);
    }
}