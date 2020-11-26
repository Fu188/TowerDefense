using UnityEngine;

public class KkMailFlagsDto
{
    [Newtonsoft.Json.JsonProperty]
    private long kkMailId;
    [Newtonsoft.Json.JsonProperty]
    private byte flags;

    public KkMailFlagsDto()
    {
    }

    public KkMailFlagsDto(long kkMailId, byte flags)
    {
        this.kkMailId = kkMailId;
        this.flags = flags;
    }

    public long GetKkMailId()
    {
        return kkMailId;
    }

    public KkMailFlagsDto SetKkMailId(long kkMailId)
    {
        this.kkMailId = kkMailId;
        return this;
    }

    public byte GetFlags()
    {
        return flags;
    }

    public KkMailFlagsDto SetFlags(byte flags)
    {
        this.flags = flags;
        return this;
    }

    public override string ToString()
    {
        return "KkMailFlagsDto{" +
                "kkMailId=" + kkMailId +
                ", flags=" + flags +
                '}';
    }
}
