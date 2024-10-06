using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject howToPlayScreen;
    [SerializeField]
    private GameObject creditScreen;

    public void Play()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void OpenHowToPlay()
    {
        howToPlayScreen.gameObject.SetActive(true);
        mainMenuScreen.gameObject.SetActive(false);
    }

    public void OpenCredit()
    {
        creditScreen.gameObject.SetActive(true);
        mainMenuScreen.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

}