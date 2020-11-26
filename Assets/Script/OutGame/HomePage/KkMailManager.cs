using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KkMailManager : MonoBehaviour
{

    private static KkMailManager _instance;

    private KkMailManager()
    {
    }

    public static KkMailManager GetKkMailManagerInstance()
    {
        if (_instance == null)
            print("null");
        return _instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public GameObject KkMailScrollView;
    public GameObject KkMailBox;
    public GameObject ReadAllBtn;
    public GameObject MailDetail;
    public GameObject ReturnMailBoxBtn;

    public void KkMailInitialize()
    {
        for (int i = 0; i < KkMailBox.transform.childCount; i++)
        {
            Destroy(KkMailBox.transform.GetChild(i).gameObject);
        }
        List<KkMail> mails = GeneralManager.Instance.mails;
        for(int i = 0; i < mails.Count; i++)
        {
            KkMail mail = mails[i];
            GameObject NewMail = (GameObject)Instantiate(Resources.Load("Prefabs/MailItem"));
            bool isRead = mail.CheckRead();
            bool isObtained = mail.CheckObtainedAttachment();
            if (isRead)
            {
                NewMail.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                NewMail.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (isObtained || (string.IsNullOrEmpty(mail.GetAttachment())))
            {
                NewMail.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load("Textures/T_2_letter_open_", typeof(Sprite)) as Sprite;
            }
            else
            {
                NewMail.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load("Textures/T_1_letter_", typeof(Sprite)) as Sprite;
            }

            NewMail.transform.GetChild(1).gameObject.GetComponent<Text>().text = mail.GetSubject();
            NewMail.transform.GetChild(2).gameObject.GetComponent<Text>().text = mail.GetContent();
            NewMail.transform.GetChild(3).gameObject.GetComponent<Text>().text = TimeUtil.FormatTimeString(mail.GetSendTime());
            NewMail.transform.GetChild(4).gameObject.GetComponent<Text>().text = GetAttachmentStr(mail);

            NewMail.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                mail.SetRead(true);
                NewMail.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                KkMailService.GetKkMailServiceInstance().UpdateKkMail(mail);
                KkMailScrollView.SetActive(false);
                ReadAllBtn.SetActive(false);
                MailDetail.SetActive(true);
                GetMailDetail(mail);
            });

            NewMail.transform.SetParent(KkMailBox.transform);
        }
    }

    private string GetAttachmentStr(KkMail mail)
    {
        string s = "Attachments: ";
        string attachment = mail.GetAttachment();
        if ( ! string.IsNullOrEmpty(attachment))
        {
            print(s);
            string[] ats = mail.GetAttachment().Split(';');
            for(int i = 0; i < ats.Length; i++)
            {
                string at = ats[i];
                s += string.Format("{0} x{1}", at.Split(':')[0], int.Parse(at.Split(':')[1]).ToString("N0"));
                if(i != ats.Length - 1)
                {
                    s += ", ";
                }
                print(s);
            }
        }
        return s;
    }

    public GameObject AttachmentListPanel;
    public GameObject DetailGetAllBtn;

    private void GetMailDetail(KkMail mail)
    {
        MailDetail.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = mail.GetSubject();
        //Content;
        MailDetail.transform.GetChild(1).GetChild(3).GetChild(0).GetComponentInChildren<Text>().text = mail.GetContent();

        for (int i = 0; i < AttachmentListPanel.transform.childCount; i++)
        {
            Destroy(AttachmentListPanel.transform.GetChild(i).gameObject);
        }
        if (!string.IsNullOrEmpty(mail.GetAttachment()))
        {
            string[] ats = mail.GetAttachment().Split(';');
            for (int i = 0; i < ats.Length; i++)
            {
                GameObject NewAttachment = (GameObject)Instantiate(Resources.Load("Prefabs/InventoryItem"));
                string atName = ats[i].Split(':')[0];
                string atNum = ats[i].Split(':')[1];
                NewAttachment.GetComponent<Image>().sprite = Resources.Load("Textures/Props/" + atName, typeof(Sprite)) as Sprite;
                if (atName == "Hair" || atName == "NKB" || atName == "Credit" || atName == "Exp")
                {
                    NewAttachment.transform.GetChild(0).gameObject.SetActive(true);
                    NewAttachment.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = " x" + int.Parse(atNum).ToString("N0");
                }
                NewAttachment.transform.SetParent(AttachmentListPanel.transform);
            }
        }
        else
        {
            //TODO
            DetailGetAllBtn.GetComponent<Button>().interactable = false;
        }
        if (mail.CheckObtainedAttachment())
        {
            DetailGetAllBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            DetailGetAllBtn.GetComponent<Button>().onClick.AddListener(delegate()
            {
                DetailGetAllBtn.GetComponent<Button>().interactable = false;
                mail.SetObtainedAttachment(true);
                string[] ats = mail.GetAttachment().Split(';');
                for (int i = 0; i < ats.Length; i++)
                {
                    string atName = ats[i].Split(':')[0];
                    int atNum = int.Parse(ats[i].Split(':')[1]);
                    switch (atName)
                    {
                        case "Hair":
                            UserInfoManager.UpdateUserHairAndTime(atNum);
                            break;
                        case "NKB":
                            UserInfoManager.UpdateNikeCoin(atNum);
                            break;
                        case "Credit":
                            UserInfoManager.UpdateCredit(atNum);
                            break;
                        case "Exp":
                            UserInfoManager.UpdateExp(atNum);
                            break;
                        default:
                            GeneralManager.Instance.props[int.Parse(atName)].IsObtained = true;

                            //TO DO UserInfoManager.UpdateProp();
                            break;
                    }
                }
                User.GetUserInstance().SetProp(JsonConvert.SerializeObject(GeneralManager.Instance.props));
                UserInfoManager.UpdateUserBackEndInfo(1);
            });
        }
    }

    public void ReturnMailBox()
    {
        MailDetail.SetActive(false);
        KkMailScrollView.SetActive(true);
        ReadAllBtn.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
