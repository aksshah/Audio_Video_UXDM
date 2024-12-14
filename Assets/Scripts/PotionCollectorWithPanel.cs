using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PotionCollectorWithPanel : MonoBehaviour
{
    public Sprite[] potionImages;      // Array to hold the potion images
    public GameObject dialoguePanel;   // UI Panel for displaying images
    public Image dialogueImage;        // UI Image to display sprites
    public Sprite finalImage;          // Final image to show after collecting all potions

    private int potionsCollected = 0;  // Counter for collected potions
    private bool inDialogue = false;   // Is the dialogue panel currently active?

    private void Start()
    {
        dialoguePanel.SetActive(false); // Ensure the panel is hidden initially
    }

    public void CollectPotion(GameObject potion)
    {
        Destroy(potion); // Destroy the collected potion
        ShowPotionImage(); // Show the next image
    }

    private void ShowPotionImage()
    {
        if (potionsCollected < potionImages.Length)
        {
            inDialogue = true;
            dialoguePanel.SetActive(true);

            // Assign the sprite and debug
            dialogueImage.sprite = potionImages[potionsCollected];
            Debug.Log("Displaying potion image: " + potionImages[potionsCollected].name);

            potionsCollected++;
        }
        else
        {
            ShowFinalImage(); // Show final image after all potions are collected
        }
    }


    private void Update()
    {
        if (inDialogue && Input.GetKeyDown(KeyCode.Return)) // Close panel with Enter key
        {
            dialoguePanel.SetActive(false);
            inDialogue = false;

            if (potionsCollected == potionImages.Length)
            {
                ShowFinalImage();
            }
        }
    }

    private void ShowFinalImage()
    {
        dialogueImage.sprite = finalImage; // Display the final image
        Debug.Log("Displaying final image: " + finalImage.name);

        dialoguePanel.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;

        // Start listening for the space key to restart the game
        StartCoroutine(WaitForRestart());
    }

    private System.Collections.IEnumerator WaitForRestart()
    {
        // Unfreeze the game temporarily to allow key detection
        Time.timeScale = 1f;

        // Wait until the space key is pressed
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null; // Wait for the next frame
        }

        // Restart the game
        Time.timeScale = 1f; // Ensure time scale is reset to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
