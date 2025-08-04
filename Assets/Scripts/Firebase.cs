using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Firebase : MonoBehaviour
{
    public string firebaseURL = "https://capstone-70c48-default-rtdb.asia-southeast1.firebasedatabase.app/";

    [System.Serializable]
    public class ScoreData
    {
        public string username;
        public float time;
        public long timestamp;

        public ScoreData(string username, float time)
        {
            this.username = username;
            this.time = time;
            this.timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    public void SubmitScore(string username, float timeTaken, string gameMode, string arenaName)
    {
        ScoreData data = new ScoreData(username, timeTaken);
        string path = $"{gameMode}/{arenaName}";
        StartCoroutine(PostScore(data, path));
    }

    private IEnumerator PostScore(ScoreData scoreData, string path)
    {
        string json = JsonUtility.ToJson(scoreData);
        string fullPath = $"{firebaseURL}leaderboard/{path}.json";

        using (UnityWebRequest www = new UnityWebRequest(fullPath, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error uploading score: " + www.error);
            }
            else
            {
                Debug.Log("Score uploaded successfully!");
            }
        }
    }
}