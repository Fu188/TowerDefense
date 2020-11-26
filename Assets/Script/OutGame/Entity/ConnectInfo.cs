using UnityEngine;

public class ConnectInfo
{

    [Newtonsoft.Json.JsonProperty]
    private string region;

    [Newtonsoft.Json.JsonProperty]
    private string server;

    [Newtonsoft.Json.JsonProperty]
    private string key;

    [Newtonsoft.Json.JsonProperty]
    private string content;

    [Newtonsoft.Json.JsonProperty]
    private int maxCcu;

    public string GetRegion()
    {
        return region;
    }

    public ConnectInfo SetRegion(string region)
    {
        this.region = region;
        return this;
    }

    public string GetServer()
    {
        return server;
    }

    public ConnectInfo SetServer(string server)
    {
        this.server = server;
        return this;
    }

    public string GetKey()
    {
        return key;
    }

    public ConnectInfo SetKey(string key)
    {
        this.key = key;
        return this;
    }

    public string GetContent()
    {
        return content;
    }

    public ConnectInfo SetContent(string content)
    {
        this.content = content;
        return this;
    }

    public int GetMaxCcu()
    {
        return maxCcu;
    }

    public ConnectInfo SetMaxCcu(int maxCcu)
    {
        this.maxCcu = maxCcu;
        return this;
    }

    
    public override string ToString()
    {
        return "ConnectInfo{" +
                "region='" + region + '\'' +
                ", key='" + key + '\'' +
                ", content='" + content + '\'' +
                ", maxCcu=" + maxCcu +
                '}';
    }
}
