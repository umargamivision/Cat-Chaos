using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image avatar;
    public Sprite granny, cat, pigeon;
    public TMP_Text dialogueText;
    [InspectorButton]
    public void SetDialogue(string dialogue)
    {
        //if first letter is G then it is granny
        //if first letter is C then it is cat
        //if first letter is P then it is pigeon
        if(dialogue[0] == 'G')
        {
            avatar.sprite = granny;
        }
        else if(dialogue[0] == 'C')
        {
            avatar.sprite = cat;
        }
        else if(dialogue[0] == 'P')
        {
            avatar.sprite = pigeon;
        }
        dialogue = dialogue.Substring(2);
        dialogueText.text = dialogue;
    }
}
