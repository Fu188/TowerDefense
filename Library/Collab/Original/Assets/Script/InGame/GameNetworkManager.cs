using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{

    public static GameNetworkManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        GameNetworkManager.KkInstantiate(Resources.Load("KKhero1") as GameObject, new Vector3(-13.3f, -2.4f, -5.4f), Quaternion.identity);

    }

    public static GameObject KkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            
            return PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);
        }
        else
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
    }

}
