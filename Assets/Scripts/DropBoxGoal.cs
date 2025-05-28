using UnityEngine;
using TMPro;

public class DropBoxGoal : MonoBehaviour
{
    public WinConditionFetcher winFetcher;
    public TextMeshProUGUI winText;

    private int correctCubesInBox = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (winFetcher == null) return;

        string requiredTag = winFetcher.targetColor.ToLower() + "cube"; // e.g. "greencube"

        if (other.CompareTag(requiredTag))
        {
            correctCubesInBox++;

            Debug.Log($"Correct cube entered! Total: {correctCubesInBox}/{winFetcher.targetCount}");

            if (correctCubesInBox >= winFetcher.targetCount)
            {
                winText.text = "You Win!!!";
            }
        }
    }
}
