using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject howToPlayScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseHowToPlay();
        }
    }

    public void CloseHowToPlay()
    {
        mainMenuScreen.gameObject.SetActive(true);
        howToPlayScreen.gameObject.SetActive(false);
    }
}