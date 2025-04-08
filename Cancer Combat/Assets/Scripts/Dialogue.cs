using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public GameObject textObj; // Reference to the text object in the UI
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI talkerNameText;
    public float textDelay = 0.05f; // Adjust typing speed

    private string fullText;
    private int charIndex = 0;
    private bool isTyping = false;
    private float targetFontSize;

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
            enabled = false;
        }

        textObj.SetActive(false); // Hide the dialogue initially

        StartDialog("Hello\nWorld\nBye!"); // Example text to start with
        setTalkerName("Player"); // Example talker name
    }

    void StartDialog(string textToType)
    {
        fullText = textToType;
        charIndex = 0;

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
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        while (charIndex < fullText.Length)
        {
            textComponent.text += fullText[charIndex];
            charIndex++;
            yield return new WaitForSeconds(textDelay);
        }
        isTyping = false;
    }

    public void SkipText()
    {
        if (isTyping)
        {
            StopCoroutine(TypeText());
            textComponent.text = fullText;
            isTyping = false;
            charIndex = fullText.Length; // Mark as fully displayed
        }
    }

    public void setTalkerName(string name)
    {
        talkerNameText.text = name;
    }



}