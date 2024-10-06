using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishCaughtText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI remainingHooksText; // New field for displaying remaining hooks
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private int maxHooks = 5;

    private int remainingHooks;
    private bool isWinning = false;

    private void Start()
    {
        remainingHooks = maxHooks;
        gameOverText.text = "";
        UpdateRemainingHooksText(); // Update the UI at the start
    }

    public void DisplayFishCaught(string fishName)
    {
        StartCoroutine(ShowFishCaughtMessage(fishName));
    }

    private IEnumerator ShowFishCaughtMessage(string fishName)
    {
        fishCaughtText.text = $"Caught: {fishName}";
        yield return new WaitForSeconds(displayDuration);
        fishCaughtText.text = "";
    }

    public void DecreaseHookCount()
    {
        remainingHooks--;
        UpdateRemainingHooksText(); // Update the remaining hooks text

        if (remainingHooks <= 0)
        {
            GameOver();
        }
    }

    private void UpdateRemainingHooksText()
    {
        remainingHooksText.text = $"Remaining Hooks: {remainingHooks}"; // Update UI text
    }

    private void GameOver()
    {
        if (isWinning)
        {
            gameOverText.text = "You Win!";
        }
        else
        {
            gameOverText.text = "Game Over!";
        }
    }

    public bool IsGameOver()
    {
        return remainingHooks <= 0 || isWinning;
    }

    public void Win()
    {
        isWinning = true;
        GameOver();
    }
}
