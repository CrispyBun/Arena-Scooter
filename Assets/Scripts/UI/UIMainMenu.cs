using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;

    void Start()
    {
        startGameButton.onClick.AddListener(onStartClicked);
    }

    private void onStartClicked()
    {
        SceneManager.LoadScene("Arena");
    }
}
