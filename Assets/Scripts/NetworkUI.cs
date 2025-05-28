using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    void Start()
    {
        hostButton.onClick.AddListener(() => {
            Debug.Log("Starting as Host...");
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() => {
            Debug.Log("Starting as Client...");
            NetworkManager.Singleton.StartClient();
        });
    }
}
