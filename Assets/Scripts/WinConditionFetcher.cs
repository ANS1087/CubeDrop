using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Unity.Netcode;

public class WinConditionFetcher : NetworkBehaviour
{
    [System.Serializable]
    public class WinCondition
    {
        public string color;
        public int number;
    }

    public string configURL = "https://b65vwvadakwqqofkr4bng36uva0gmujh.lambda-url.ap-south-1.on.aws/";
    public TextMeshProUGUI goalText;

    public string targetColor;
    public int targetCount;

    public override void OnNetworkSpawn()
    {
        if (IsServer || IsClient)
        {
            StartCoroutine(FetchWinCondition());
        }
    }

    private IEnumerator FetchWinCondition()
    {
        UnityWebRequest request = UnityWebRequest.Get(configURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch win condition: " + request.error);
            goalText.text = "Failed to load win condition.";
        }
        else
        {
            string json = request.downloadHandler.text;
            WinCondition data = JsonUtility.FromJson<WinCondition>(json);

            targetColor = data.color.ToLower();
            targetCount = data.number;

            goalText.text = $"Drop {targetCount} {targetColor} cubes in the box to win!";
        }
    }
}
