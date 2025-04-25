using UnityEngine;
using System.Collections.Generic;


public class Level3BossDialogue : MonoBehaviour
{
    public List<string> dialogueLines;
    public int dialogueIndex = 0; // Index to track the current dialogue line
    public string talkerName; 
    public DialogueBox dialogueUI; // Reference to your Dialogue UI script

    
    

    public BossAI bossScript;





    void Update()
    {
        
        if (bossScript.enabled && dialogueIndex == 0){
            print("BOSS 1");
            StartDialogue();
            dialogueIndex++;
        }

        else if (bossScript.enabled && dialogueIndex == 1 && bossScript.currentHealth <= 100){
            print(bossScript.currentHealth);
            StartDialogue();
            dialogueIndex++;
        }

        
    }

    public void StartDialogue()
    {
        if (dialogueUI != null && dialogueLines != null && dialogueLines.Count > 0)
        {
            dialogueUI.StartDialog(dialogueLines[dialogueIndex]); 
            dialogueUI.setTalkerName(talkerName); // Set the talker name in the dialogue UI
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no dialogue lines or Dialogue UI assigned.");
        }
    }
}
