using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{

    public static GameNetworkManager Instance;
    public static bool playing;
    // Start is called before the first frame update
    void Start()
    {
        playing = true;
        GameNetworkManager.KkInstantiate(Resources.Load("KKhero1") as GameObject, new Vector3(-13.3f, -2.4f, -5.4f), Quaternion.identity);
    }

    public static GameObject KkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        if (PhotonNetwork.IsConnected)
        {

            return (GameObject)PhotonNetwork.Instantiate(prefab.name, position, rotation, group, data);
        }
        else
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
    }

    public static void KkDestroy(GameObject o)
    {
        if (PhotonNetwork.IsConnected)
        {

            PhotonNetwork.Destroy(o);
        }
        else
        {
            GameObject.Destroy(o);
        }
    }



    public static void GameEnd()
    {
        playing = false;
        if (!PhotonNetwork.IsConnected)
        {
            int lives = GameManager.Instance.Lives;
            if (lives > 8)
            {
                UserInfoManager.UpdateNikeCoin(1000);
                UserInfoManager.UpdateCredit(50);
            }
            else if (lives > 5)
            {
                UserInfoManager.UpdateNikeCoin(800);
                UserInfoManager.UpdateCredit(30);
            }
            else if (lives > 0)
            {
                UserInfoManager.UpdateNikeCoin(500);
                UserInfoManager.UpdateCredit(10);
            }
            else
            {
                UserInfoManager.UpdateUserHairAndTime(6);
                UserInfoManager.UpdateUserBackEndInfo(2);
                RankDataService.UpdateHairRankData(4);
                return;
            }
            UserInfoManager.UpdateUserBackEndInfo(3);
            RankDataService.UpdateHairRankData(10);
        }
    }

}
