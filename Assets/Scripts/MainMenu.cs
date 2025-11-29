using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private Button exitButton;
    public static string nick;

    [Header("Objects to hide")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Image _panel;

    void Start() {
        playButton.onClick.AddListener(() => {
            if (nicknameField.text != "") {
                nick = nicknameField.text;
                SceneManager.LoadScene("Main", LoadSceneMode.Additive);
                nicknameField.text = "";
                _mainCamera.gameObject.SetActive(false);
                _panel.gameObject.SetActive(false);
            }
        });

        exitButton.onClick.AddListener(() => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        });

        Player.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _mainCamera.gameObject.SetActive(true);
        _panel.gameObject.SetActive(true);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        Player.OnPlayerDeath -= Player_OnPlayerDeath;
    }

}
