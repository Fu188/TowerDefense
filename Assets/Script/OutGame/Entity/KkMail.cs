using UnityEngine;

/// <summary>
/// KKMP
/// </summary>
public class KkMail
{
    [Newtonsoft.Json.JsonProperty]
    private long kkMailId;
    [Newtonsoft.Json.JsonProperty]
    private long senderId;
    [Newtonsoft.Json.JsonProperty]
    private long receiverId;

    /**
     * sender name
     */
    [Newtonsoft.Json.JsonProperty]
    private string sender;
    /**
     * receiver name
     */
    [Newtonsoft.Json.JsonProperty]
    private string receiver;
    [Newtonsoft.Json.JsonProperty]
    private string subject;
    [Newtonsoft.Json.JsonProperty]
    private string content;
    [Newtonsoft.Json.JsonProperty]
    private string attachment;
    [Newtonsoft.Json.JsonProperty]
    private string sentTime;
    [Newtonsoft.Json.JsonProperty]
    private byte flags;
    [Newtonsoft.Json.JsonProperty]
    private string optional;


    private readonly byte readFlag = 0b0000001;
    private readonly byte deletedFlag = 0b00000010;
    private readonly byte obtainedAttachmentFlag = 0b00000100;

    public KkMail()
    {
    }

    public KkMail(long kkMailId)
    {
        this.kkMailId = kkMailId;
    }

    public long GetKkMailId()
    {
        return kkMailId;
    }

    public KkMail SetKkMailId(long kkMailId)
    {
        this.kkMailId = kkMailId;
        return this;
    }

    public long GetSenderId()
    {
        return senderId;
    }

    public KkMail SetSenderId(long senderId)
    {
        this.senderId = senderId;
        return this;
    }

    public long GetReceiverId()
    {
        return receiverId;
    }

    public KkMail SetReceiverId(long receiverId)
    {
        this.receiverId = receiverId;
        return this;
    }

    public string GetSender()
    {
        return sender;
    }

    public KkMail SetSender(string sender)
    {
        this.sender = sender;
        return this;
    }

    public string GetReceiver()
    {
        return receiver;
    }

    public KkMail SetReceiver(string receiver)
    {
        this.receiver = receiver;
        return this;
    }

    public string GetSubject()
    {
        return subject;
    }

    public KkMail SetSubject(string subject)
    {
        this.subject = subject;
        return this;
    }

    public string GetContent()
    {
        return content;
    }

    public KkMail SetContent(string content)
    {
        this.content = content;
        return this;
    }

    public string GetAttachment()
    {
        return attachment;
    }

    public KkMail SetAttachment(string attachment)
    {
        this.attachment = attachment;
        return this;
    }

    public string GetSendTime()
    {
        return sentTime;
    }

    public KkMail SetSendTime(string sendTime)
    {
        this.sentTime = sendTime;
        return this;
    }

    public byte GetFlags()
    {
        return flags;
    }

    public KkMail SetFlags(byte flags)
    {
        this.flags = flags;
        return this;
    }


    public string GetOptional()
    {
        return optional;
    }

    public KkMail SetOptional(string optional)
    {
        this.optional = optional;
        return this;
    }

    public bool CheckRead()
    {
        return CheckFlag(flags,readFlag);
    }

    public KkMail SetRead(bool read)
    {
        SetFlag(readFlag, read);
        return this;
    }

    public bool CheckDeleted()
    {
        return CheckFlag(flags,deletedFlag);
    }

    public KkMail SetDeleted(bool deleted)
    {
        SetFlag(deletedFlag, deleted);
        return this;
    }


    public bool CheckObtainedAttachment()
    {
        return CheckFlag(flags, obtainedAttachmentFlag);
    }

    public KkMail SetObtainedAttachment(bool obtained)
    {
        SetFlag(obtainedAttachmentFlag, obtained);
        return this;
    }


    public override string ToString()
    {
        return "KkMail{" +
                "kkMailId=" + kkMailId +
                ", senderId=" + senderId +
                ", receiverId=" + receiverId +
                ", sender='" + sender + '\'' +
                ", receiver='" + receiver + '\'' +
                ", subject='" + subject + '\'' +
                ", content='" + content + '\'' +
                ", attachment='" + attachment + '\'' +
                ", sentTime=" + sentTime +
                ", flags=" + flags +
                ", optional='" + optional + '\'' +
                '}';
    }

    #region Private Function
    private bool CheckFlag(byte flags, byte flag)
    {
        return (flags & flag) != 0;
    }

    private KkMail SetFlag(byte flag, bool set)
    {
        flags = set ? (byte)(flags | flag) : (byte)(flags & ~flag);
        return this;
    }
    #endregion
}
