using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Image profilePicture;
    [SerializeField] private TextMeshProUGUI textName;

    [Header("Text Speech")]
    [SerializeField] private TextMeshProUGUI textSpeech;
    [SerializeField] private float typingSpeed;
    private string fullText;
    private bool finishedTyping = true;
    private Coroutine coroutine;

    [Header("Dialogue")]
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private Dialogue dialogue;
    [HideInInspector] public bool finishedDialogue = false;
    private int index = 0;

    private IEnumerator TypeText()
    {
        finishedTyping = false;

        textSpeech.text = fullText;
        textSpeech.maxVisibleCharacters = 0;

        for(int i = 0; i <= fullText.Length; i++)
        {
            textSpeech.maxVisibleCharacters = i;          
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        finishedTyping = true;
    }

    public void AdvanceDialogue()
    {
        if(!finishedTyping)
        {
            StopCoroutine(coroutine);
            textSpeech.maxVisibleCharacters = fullText.Length;
            finishedTyping = true;
        }
        else
        {
            if(index == 0) //inicio do dialogo
            {
                finishedDialogue = false;
                panelDialogue.SetActive(true);
                Time.timeScale = 0f;
                PauseMenu.gamePaused = true;
            }

            if(index < dialogue.fullDialogue.Count) //dialogo em andamento
            {
                profilePicture.sprite = dialogue.fullDialogue[index].characterPicture;
                textName.text = dialogue.fullDialogue[index].characterName;
                fullText = dialogue.fullDialogue[index].speechText;

                coroutine = StartCoroutine(TypeText());

                index++;
            }
            else //dialogo finalizado
            {
                finishedDialogue = true;
                panelDialogue.SetActive(false);
                index = 0;
                Time.timeScale = 1f;
                PauseMenu.gamePaused = false;
            }
        }
    }
}