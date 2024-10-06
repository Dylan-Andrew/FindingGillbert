using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject creditScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCredit();
        }
    }

    public void CloseCredit()
    {
        mainMenuScreen.gameObject.SetActive(true);
        creditScreen.gameObject.SetActive(false);
    }
}