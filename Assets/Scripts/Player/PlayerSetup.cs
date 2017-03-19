using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera lobbyCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            // Remember to remove from this script and add to something like a Startup Manager
            lobbyCamera = Camera.main;
            if(lobbyCamera != null)
            {
                lobbyCamera.gameObject.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        if( lobbyCamera != null)
        {
            lobbyCamera.gameObject.SetActive(true);
        }
    }

}