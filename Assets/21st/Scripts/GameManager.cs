using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fishCaughtText;
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private TextMeshProUGUI loseText;
    [SerializeField]
    private TextMeshProUGUI remainingHooksText;
    [SerializeField]
    private float displayDuration = 3f;
    [SerializeField]
    private int maxHooks = 5;
    [SerializeField]
    private GameObject HookedUI;
    [SerializeField]
    private GameObject HookUI;
    [SerializeField]
    private GameObject WinUI;
    [SerializeField]
    private GameObject LoseUI;

    private int remainingHooks;
    private bool isWinning = false;

    private void Start()
    {
        remainingHooks = maxHooks;
        fishCaughtText.text = "";
        UpdateRemainingHooksText();
    }

    public void DisplayFishCaught(string fishName)
    {
        StartCoroutine(ShowNoobtMessage(fishName));
    }

    private IEnumerator ShowNoobtMessage(string fishName)
    {
        HookedUI.gameObject.SetActive(true);

        string[] messages = new string[]
        {
        $"You think I look like Gilbert? I am {fishName}, dumbass.",
        $"Are you blind? It's {fishName}!",
        $"Do I look like Gilbert to you? Get your eyes checked!",
        $"Look closer! This is obviously {fishName}.",
        $"Seriously? This is {fishName}, not whoever you thought!",
        $"I'm not just a fish; I'm {fishName}! Learn the difference!"
        };

        string randomMessage = messages[Random.Range(0, messages.Length)];

        fishCaughtText.text = randomMessage;

        yield return new WaitForSeconds(displayDuration);
        HookedUI.gameObject.SetActive(false);
        fishCaughtText.text = "";
    }


    public void DecreaseHookCount()
    {
        remainingHooks--;
        UpdateRemainingHooksText();

        if (remainingHooks <= 0)
        {
            GameOver();
        }
    }

    private void UpdateRemainingHooksText()
    {
        remainingHooksText.text = remainingHooks.ToString();
    }

    private void GameOver()
    {
        string[] winMessages = new string[]
        {
            "I caught Gilbert! Guess we are eating fish tonight",
            "I snagged Gilbert! Hope I did not forget my frying pan!",
            "Look who got harpooned! It’s Gilbert the Gilled! Time for a fish fry!",
        };
        string randomWinMessage = winMessages[Random.Range(0, winMessages.Length)];

        string[] loseMessages = new string[]
        {
            "Oh no! I lost Gilbert! What am I supposed to eat now?",
            "Where did Gilbert go? I'm starving over here!",
            "Noooo! Gilbert slipped away! My stomach is growling!"
        };
        string randomLoseMessage = loseMessages[Random.Range(0, loseMessages.Length)];

        if (isWinning)
        {
            HookUI.gameObject.SetActive(false);
            WinUI.gameObject.SetActive(true);
            winText.text = randomWinMessage;
        }
        else
        {
            HookUI.gameObject.SetActive(false);
            LoseUI.gameObject.SetActive(true);
            loseText.text = randomLoseMessage;
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
