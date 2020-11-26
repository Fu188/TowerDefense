using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class KkMailService
{

    private static KkMailService _instance;

    private static readonly object locker = new object();

    private string baseUrl = NetWorkInfo.backEndBaseUrl + "/v1";


    private KkMailService()
    {
    }

    public static KkMailService GetKkMailServiceInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new KkMailService();
                }
            }
        }
        return _instance;
    }


    public List<KkMail> GetReceivedKkMails(long receiverId)
    {
        string result = HttpUtil.Get(string.Format("{0}/kkMail/getReceivedKkMails/{1}", baseUrl, receiverId));
        return JsonConvert.DeserializeObject<List<KkMail>>(result);
    }


    public List<KkMail> GetSentKkMails(long senderId)
    {
        string result = HttpUtil.Get(string.Format("{0}/kkMail/getSentKkMails/{1}", baseUrl, senderId));
        return JsonConvert.DeserializeObject<List<KkMail>>(result);
    }

    public bool SendKkMail(KkMail kkMail)
    {
        return HttpUtil.Post(JsonConvert.SerializeObject(kkMail), string.Format("{0}/kkMail/sendKkMail", baseUrl)).Equals("true");
    }


    public bool UpdateKkMail(KkMail kkMail)
    {
        return HttpUtil.Put(JsonConvert.SerializeObject(kkMail), string.Format("{0}/kkMail/updateKkMail", baseUrl)).Equals("true");
    }

    public bool UpdateKkMailBatch(List<KkMail> kkMails)
    {
        return HttpUtil.Put(JsonConvert.SerializeObject(kkMails), string.Format("{0}/kkMail/updateKkMailBatch", baseUrl)).Equals("true");
    }

    public bool UpdateKkMailState(List<KkMailFlagsDto> kkMailFlagsDtos)
    {
        return HttpUtil.Put(JsonUtility.ToJson(kkMailFlagsDtos), string.Format("{0}/kkMail/updateKkMailState", baseUrl)).Equals("true");
    }

    public bool UpdateKkMailFlags(long kkMailId, byte flags)
    {
        return HttpUtil.Put(JsonUtility.ToJson(new KkMailFlagsDto(kkMailId,flags)), string.Format("{0}/kkMail/updateKkMailState", baseUrl)).Equals("true");
    }

}
