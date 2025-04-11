using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueBox : MonoBehaviour
{
    private CanvasRenderer[] renderers;

    public GameObject textObj; // Reference to the text object in the UI
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI talkerNameText;
    public float textDelay = 0.05f; // Adjust typing speed

    private string fullText;
    private int charIndex = 0;
    private bool isTyping = false;
    private float targetFontSize;

    private Coroutine typingCoroutine;

    private bool isDialogueFinished = false;

    public bool inDialogue = false; // Flag to check if the player is in dialogue



    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
            enabled = false;
        }

        textObj.SetActive(false); // Hide the dialogue initially
        
        renderers = GetComponentsInChildren<CanvasRenderer>();
        HideDialogueBox(); // Hide on start if needed

        //StartDialog("Hello\nWorld\nBye!"); // Example text to start with
        //setTalkerName("Player"); // Example talker name
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                Debug.Log("Skip");
                SkipText();
            }
            else if (isDialogueFinished)
            {
                Debug.Log("Stopping");
                StopDialog();
            }
        }

    }

    public void StartDialog(string textToType)
    {
        inDialogue = true; // Set the inDialogue flag to true when starting a dialogue

        ShowDialogueBox();

        fullText = textToType;
        charIndex = 0;
        isDialogueFinished = false;

        // 1. Temporarily set the full text to trigger auto-sizing
        textComponent.text = fullText;
        textComponent.enableAutoSizing = true;
        textComponent.ForceMeshUpdate(); // Ensure layout is updated immediately

        // 2. Store the calculated font size
        targetFontSize = textComponent.fontSize;

        // 3. Disable Auto Size and set the fixed size
        textComponent.enableAutoSizing = false;
        textComponent.fontSize = targetFontSize;

        textObj.SetActive(true); // Show the Dialogue

        textComponent.text = ""; // Clear any previous text
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    public void StopDialog()
    {
        textObj.SetActive(false);
        HideDialogueBox();
        isDialogueFinished = false;
        inDialogue = false;;
    }


    IEnumerator TypeText()
    {
        isTyping = true;
        for (int i = 0; i < fullText.Length; i++)
        {
            if (fullText[i] == '\\' && i + 1 < fullText.Length && fullText[i + 1] == 'n')
            {
                // Found a newline escape sequence, add the actual newline
                textComponent.text += "\n";
                i++; // Skip the 'n' character as it's already handled
            }
            else
            {
                // Regular character, append it
                textComponent.text += fullText[i];
            }
            yield return new WaitForSecondsRealtime(textDelay);
        }
        isTyping = false;
        isDialogueFinished = true;

    }

    public void SkipText()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            textComponent.text = fullText;
            isTyping = false;
            isDialogueFinished = true; // Mark as finished
            charIndex = fullText.Length; // Mark as fully displayed
        }
        
    }

    public void setTalkerName(string name)
    {
        talkerNameText.text = name;
    }

    public void ShowDialogueBox()
    {
        foreach (var renderer in renderers)
        {
            renderer.SetAlpha(1f);
        }
    }

    public void HideDialogueBox()
    {
        foreach (var renderer in renderers)
        {
            renderer.SetAlpha(0f);
        }
    }



}