using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDeathCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCounterText;

    private void Awake()
    {
        deathCounterText.text = PauseMenu.deathCounter.ToString();
    }
}
