using UnityEngine;

public class Creditos : MonoBehaviour
{
    public void Voltar()
    {
        FindObjectOfType<LevelLoader>().ChangeSceneFixed(0);
    }
}