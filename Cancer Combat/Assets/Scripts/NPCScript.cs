using UnityEngine;
using System.Collections.Generic;


public class NPCScript : MonoBehaviour
{
    public List<string> dialogueLines;
    public string talkerName; 
    public DialogueBox dialogueUI; // Reference to your Dialogue UI script

    
    public float interactionDistance = 3f; // Distance within which the player can interact
    public string interactionKey = "f"; // Key the player presses to interact

    private Transform playerTransform;
    private bool canInteract = false;

    void Start()
    {
        // Find the player's transform (you might need to adjust this based on your player setup)
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
            enabled = false; // Disable this script if no player is found
        }
    }


    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Check if the player is within the interaction distance
            if (distanceToPlayer <= interactionDistance)
            {
                canInteract = true;
                // Optionally, you can display a UI prompt here to indicate interaction is possible
            }
            else
            {
                canInteract = false;
                // Optionally, you can hide the UI prompt here
            }

            // Check for player interaction input
            if (canInteract && Input.GetKeyDown(interactionKey))
            {
                StartDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        if (dialogueUI != null && dialogueLines != null && dialogueLines.Count > 0)
        {
            dialogueUI.StartDialog(dialogueLines[0]); 
            dialogueUI.setTalkerName(talkerName); // Set the talker name in the dialogue UI
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no dialogue lines or Dialogue UI assigned.");
        }
    }
}
