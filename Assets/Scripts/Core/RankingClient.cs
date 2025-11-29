using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RankingClient : MonoBehaviour {
    private const string baseUrl = "http://localhost:3004";

    public IEnumerator EnviarScore(int playerId, int score) {
        var url = $"{baseUrl}/ranking";
        var json = JsonUtility.ToJson(new ScoreRequest { playerId = playerId, score = score });

        using (var www = new UnityWebRequest(url, "POST")) {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError("Erro ao enviar score: " + www.error);
            else
                Debug.Log("Score enviado com sucesso: " + www.downloadHandler.text);
        }
    }

    [System.Serializable]
    private class ScoreRequest {
        public int playerId;
        public int score;
    }
}
