using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public string message;
    public int type;
    public GameObject notice;
    public Button confirm;
    public Button cancel;
    public Button.ButtonClickedEvent clickedEvent;
    public RectTransform Mask;

    public Notice(string message)
    {
        this.message = message;
        this.type = 0;
        this.notice = (GameObject)Instantiate(Resources.Load("Prefabs/NoticeBg"));
        
        this.notice.transform.parent = GameObject.Find("Canvas/Panel").transform;
        this.notice.GetComponentsInChildren<Text>()[0].text = this.message;
        this.Mask = this.notice.transform.GetChild(0).GetChild(0).transform as RectTransform;

        RectTransform panel = this.notice.transform as RectTransform;
        panel.sizeDelta = new Vector2(0, 0);
        panel.offsetMin = new Vector2(0, 0);
        panel.offsetMax = new Vector2(0, 0);

        this.SendNotice();
    }
    public Notice(string message, int type, Button.ButtonClickedEvent clickedEvent)
    {
        this.message = message;
        this.type = type;
        if (type == 0)
        {
            this.notice = (GameObject)Instantiate(Resources.Load("Prefabs/NoticeBg"));
        }
        else
        {
            this.notice = (GameObject)Instantiate(Resources.Load("Prefabs/NoticeConfirmBg"));
            Button[] buttons = this.notice.transform.GetComponentsInChildren<Button>();
            this.confirm = buttons[0];
            this.cancel = buttons[1];
        }
        this.notice.transform.parent = GameObject.Find("Canvas/Panel").transform;
        this.notice.GetComponentsInChildren<Text>()[0].text = this.message;
        this.Mask = this.notice.transform.GetChild(0).GetChild(0).transform as RectTransform;
        this.clickedEvent = clickedEvent;
        this.confirm.onClick = (Button.ButtonClickedEvent)FullNoticeConfirmConfirm();
        this.cancel.onClick = (Button.ButtonClickedEvent)FullNoticeConfirmClose();
        this.SendNotice();
    }

    public void SendNotice()
    {
        if (this.type == 0)
        {
            StartCoroutine(FullNotice());
            //Destroy(this.notice);
        }
        else if (this.type == 1)
        {
            StartCoroutine(FullNoticeConfirm());
        }
    }

    IEnumerator FullNotice()
    {
        this.Mask.offsetMax = new Vector2(0, -35);
        this.Mask.offsetMin = new Vector2(0, 35);
        yield return StartCoroutine(NoticeShow());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(NoticeClose(35));
    }

    IEnumerator FullNoticeConfirm()
    {
        this.Mask.offsetMax = new Vector2(0, -65);
        this.Mask.offsetMin = new Vector2(0, 65);
        yield return StartCoroutine(NoticeShow());
    }

    IEnumerator FullNoticeConfirmConfirm()
    {
        this.clickedEvent.Invoke();
        yield return StartCoroutine(FullNoticeConfirmClose());
    }

    IEnumerator FullNoticeConfirmClose()
    {
        yield return StartCoroutine(NoticeClose(65));
    }

    IEnumerator NoticeShow()
    {
        while (this.Mask.offsetMax[1] < 0)
        {
            this.Mask.offsetMax = this.Mask.offsetMax + new Vector2(0, Time.deltaTime * 300);
            this.Mask.offsetMin = this.Mask.offsetMin - new Vector2(0, Time.deltaTime * 300);
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator NoticeClose(int halfSize)
    {
        while (this.Mask.offsetMax[1] > -halfSize)
        {
            this.Mask.offsetMax = this.Mask.offsetMax - new Vector2(0, Time.deltaTime * 300);
            this.Mask.offsetMin = this.Mask.offsetMin + new Vector2(0, Time.deltaTime * 300);
            yield return new WaitForSeconds(0);
        }
    }
}
