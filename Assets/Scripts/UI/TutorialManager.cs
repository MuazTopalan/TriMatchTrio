using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public float duration = 3f;
    private bool tutorialStarted = false;
    private float timer = 0f;
    private int targetBuildIndex = 7;

    void Start()
    {
        if (tutorialPanel == null)
        {
            Debug.LogError("TutorialManager: Please assign the Tutorial Panel GameObject in the Inspector.");
            return;
        }

        // Check if the current build index matches the target
        if (SceneManager.GetActiveScene().buildIndex == targetBuildIndex)
        {
            // Enable the tutorial panel
            tutorialPanel.SetActive(true);
            tutorialStarted = true;
        }
        else
        {
            // If the build index doesn't match, make sure the tutorial panel is disabled
            tutorialPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!tutorialStarted || tutorialPanel == null)
            return;

        // Increment timer if tutorial is active
        timer += Time.deltaTime;

        // Check if the duration has passed
        if (timer >= duration)
        {
            // Disable the tutorial panel
            tutorialPanel.SetActive(false);
            tutorialStarted = false;
        }
    }
}
