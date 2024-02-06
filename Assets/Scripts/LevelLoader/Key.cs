using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    static public bool pickedKey = false;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        if (pickedKey)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickedKey = true;
            StartCoroutine(showText());
            Destroy(gameObject);
        }
    }

    private IEnumerator showText()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        panel.SetActive(false);
    }
}
