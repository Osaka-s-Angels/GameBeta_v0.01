using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]public Dialogue dialogue;
    private bool isInRange;
   
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            FindObjectOfType<DialogueManager>().isInDialogue = false;
            Debug.Log("Player in range");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player out of range");
        }
    }
    
    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!FindObjectOfType<DialogueManager>().isInDialogue) { 
                TriggerDialogue();               
            }
            else { FindObjectOfType<DialogueManager>().DisplayNextSentence(); }
            
        }
    }
    

    }


