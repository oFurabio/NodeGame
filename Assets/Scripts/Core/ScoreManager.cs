using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }

    private void OnEnable() {
        Zombie.OnZombieKilled += AddScore;
    }

    private void OnDisable() {
        Zombie.OnZombieKilled -= AddScore;
    }

    private void AddScore(object sender, EventArgs e) {
        Score += 10;
    }

    // chama isso quando o player morrer
    public void SendScoreToServer() {
        StartCoroutine(UploadScore());
    }

    private IEnumerator UploadScore() {
        ScorePayload payload = new ScorePayload {
            nickname = MainMenu.nick,
            score = Score
        };

        string json = JsonUtility.ToJson(payload);

        using UnityEngine.Networking.UnityWebRequest request =
            UnityEngine.Networking.UnityWebRequest.PostWwwForm(
                "http://localhost:3004/score",
                json
            );

        request.SetRequestHeader("Content-Type", "application/json");

        request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(
            System.Text.Encoding.UTF8.GetBytes(json)
        );

        request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            Debug.Log("Score enviado!");
        else
            Debug.LogError(request.error);

        SceneManager.UnloadSceneAsync("Main");
    }

    [System.Serializable]
    public class ScorePayload {
        public string nickname;
        public int score;
    }
}
