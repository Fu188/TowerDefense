/**
 * 
 * Synchronization and State: https://doc.photonengine.com/en-US/pun/v2/gameplay/synchronization-and-state/Optimization#remote_procedure_call__rpc_
 * 
 * RPCs and RaiseEvent: https://doc.photonengine.com/en-us/pun/v2/gameplay/rpcsandraiseevent
 * 
 * Class List: https://doc-api.photonengine.com/en/pun/v2/annotated.html
 * 
 *    MonoBehaviourPunCallbacks Class Reference: https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html
 *    Room Class Reference: https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_realtime_1_1_room.html#a3fb1bbb640e96bdaa4f24a65e276edbb
 *    PhotonNetwork Class Reference: https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_photon_network.html
 * 
 * Serialization in Photon: https://doc.photonengine.com/en-us/pun/current/reference/serialization-in-photon
**/


public class NetWorkInfo
{

    public static string scheme = "https";
    //public static string host = "127.0.0.1";
    public static string host = "8.129.4.187";
    public static int port = 8080;
    public static string domain = "api.keykeeper.ga";
    public static string backEndBaseUrl;
    public static bool useDomain = false;


    static NetWorkInfo()
    {
        RefreshBaseUrl(useDomain);
    }

    /// <summary>
    /// 刷新 baseUrl
    /// </summary>
    /// <remarks>
    /// 默认使用 https 协议，如需更改请先修改 NetWorkInfo.scheme
    /// </remarks>
    /// <param name="useDomain">useDomain 为 true 时 使用 domain:port 拼接，否则使用 ip:port 的模式拼接</param>
    public static void RefreshBaseUrl(bool useDomain)
    {
        if (useDomain)
        {
            backEndBaseUrl = string.Format("{0}://www.{1}:{2}", scheme, domain, port);
        }
        else
        {
            backEndBaseUrl = string.Format("{0}://{1}:{2}", scheme, host, port);
        }
    }
}