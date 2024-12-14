using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Sprite[] images;           // Images with embedded text
    public GameObject dialoguePanel;  // UI Panel for displaying images
    public Image dialogueImage;       // UI Image to display sprites

    private int currentIndex = -1;    // Tracks the current image
    private bool inDialogue = false;  // Tracks if dialogue is active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !inDialogue)
        {
            StartDialogue();
        }
    }

    private void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Skip to next image
            {
                DisplayNext();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) // Skip all images
            {
                EndDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        inDialogue = true;
        currentIndex = -1;
        dialoguePanel.SetActive(true); // Show the panel
        DisplayNext();                 // Start displaying images
    }

    private void DisplayNext()
    {
        currentIndex++;

        if (currentIndex >= images.Length) // Check if we're at the end of the sequence
        {
            EndDialogue();
            return;
        }

        // Set the next image
        dialogueImage.sprite = images[currentIndex];

        // Dynamically adjust the image size based on its native resolution
        RectTransform rectTransform = dialogueImage.GetComponent<RectTransform>();
        float imageAspectRatio = (float)images[currentIndex].texture.width / images[currentIndex].texture.height;
        float screenAspectRatio = (float)Screen.width / Screen.height;

        if (imageAspectRatio > screenAspectRatio) // Image is wider than the screen
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.width / imageAspectRatio);
        }
        else // Image is taller than the screen
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.height * imageAspectRatio, rectTransform.rect.height);
        }

        dialogueImage.enabled = true;
    }


    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide the panel
        inDialogue = false;
    }
}
