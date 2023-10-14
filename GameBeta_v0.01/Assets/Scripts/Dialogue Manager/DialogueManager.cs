using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    [SerializeField]private GameObject dialoguePanel;
    [HideInInspector]public bool isInDialogue;

    private Queue<string> dialogue;


    void Start()
    {
        dialogue = new Queue<string>();
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.name);//Debug purpose only 
        nameText.text = dialogue.name;
        dialoguePanel.SetActive(true);
        isInDialogue = true;
        this.dialogue.Clear();
        foreach (string sentence in dialogue.dialogue)
        {
            this.dialogue.Enqueue(sentence);//Adds the sentences to the queue
        }
        DisplayNextSentence();//Displays the next sentence
    }

    public void DisplayNextSentence()
    {
        if (dialogue.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = dialogue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;

        }
    }
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isInDialogue = false;
        Debug.Log("End of conversation");
    }

    
}
