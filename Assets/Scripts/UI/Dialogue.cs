using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public struct Speech
{
    public Sprite characterPicture;
    public string characterName;
    [TextArea(5, 10)] public string speechText;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public List<Speech> fullDialogue;
}