using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
// object p = System.Web.HttpUtility.UrlEncode("heart", System.Text.Encoding.UTF8);
public class HttpUtil
{

    private static readonly object locker = new object();

    private static HttpUtil _instance;

    private HttpUtil() { }

    public static HttpUtil GetHttpUtilInstance()
    {
        if (_instance == null)
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new HttpUtil();
                }
            }
        }
        return _instance;
    }


    static HttpUtil()
    {
        AcceptHTTPS();
    }



    // Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
    public class UnityWebRequestAcceptAllCertificate: UnityEngine.Networking.CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // 总是接受
        }
    }


    public static void AcceptHTTPS()
    {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
        {
            return true; //总是接受  
        });
        
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
    }

    public static string GetResponseString(HttpWebResponse webresponse)
    {
        StreamReader reader = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8);
        // HttpUtility.UrlDecode(reader.ReadToEnd())
        string result = reader.ReadToEnd();
        reader.Close();
        return result;
    }


    public static string Get(string url)
    {
        UnityEngine.Debug.Log(url);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string result = GetResponseString(response);
        response.Close();
        return result;
    }



    public static string Post(string data, string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(data);

        request.Method = "POST";
        request.ContentLength = buf.Length;
        request.ContentType = "application/json";
        request.MaximumAutomaticRedirections = 1;
        request.AllowAutoRedirect = true;

        Stream stream = request.GetRequestStream();
        stream.Write(buf, 0, buf.Length);
        stream.Close();


        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string result = GetResponseString(response);
        response.Close();
        return result;

    }

    public static string Put(string data, string url)
    {

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(data);

        request.Method = "PUT";
        request.ContentLength = buf.Length;
        request.ContentType = "application/json";
        request.MaximumAutomaticRedirections = 1;
        request.AllowAutoRedirect = true;

        Stream stream = request.GetRequestStream();
        stream.Write(buf, 0, buf.Length);
        stream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string result = GetResponseString(response);
        response.Close();
        return result;

    }

    //request.ContentType = MimeMapping.GetMimeMapping(filePath);

    private static string PutMultipartFile(byte[] content, string boundary, string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        request.Method = "PUT";
        request.Pipelined = true;
        request.ContentLength = content.Length;
        request.ContentType = "multipart/form-data; boundary=" + boundary;
        request.MaximumAutomaticRedirections = 1;
        request.AllowAutoRedirect = true;

        Stream stream = request.GetRequestStream();
        stream.Write(content, 0, content.Length);
        stream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string result = GetResponseString(response);
        response.Close();
        return result;

    }

    public static string PutOneMultipartFile(string filePath, string url)
    {
        string boundary = "------" + System.DateTime.Now.Ticks.ToString("x");
        byte[] content = GetNewContent(boundary, filePath);
        return PutMultipartFile(content, boundary, url);
    }

    public static string PutOneMultipartFile(byte[] data, long userId, string url)
    {
        string boundary = "------" + System.DateTime.Now.Ticks.ToString("x");
        byte[] content = GetNewContent(userId, data, boundary);
        return PutMultipartFile(content, boundary, url);
    }


    /**
     * boundary can be customized
     * HTTP header Content-Type change to:
     *    Content-Type: multipart/form-data; boundary={boundary}
     * multipart/form-data data(content) format

     --{boundary}
     Content-Disposition: form-data; name="{Form data item identifier}"
  
     {text data}
     --{boundary}
     Content-Disposition: form-data; name="{Form data item identifier}"; filename="mypng.png"
     Content-Type: image/png
 
     {mypng.png data}
     --{boundary}--
     **/
    private static byte[] GetNewContent(string boundary, string filePath)
    {
        //System.Net.Http.MultipartFormDataContent client = new System.Net.Http.MultipartFormDataContent(boundary);
        //client.Add(new ByteArrayContent(buf), "avatarFile", "ooad-together.png");
        //System.Threading.Tasks.Task task = client.CopyToAsync(stream);
        string begin = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"avatarFile\"; filename=\"{1}\"\r\n\r\n", boundary, Path.GetFileName(filePath));

        System.Collections.Generic.List<byte> mulFormData = new System.Collections.Generic.List<byte>(Encoding.UTF8.GetBytes(begin));
        mulFormData.AddRange(File.ReadAllBytes(filePath));
        mulFormData.AddRange(Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n"));

        return mulFormData.ToArray();
    }

    private static byte[] GetNewContent(long userId, byte[] image, string boundary)
    {
        //System.Net.Http.MultipartFormDataContent client = new System.Net.Http.MultipartFormDataContent(boundary);
        //client.Add(new ByteArrayContent(buf), "avatarFile", "ooad-together.png");
        //System.Threading.Tasks.Task task = client.CopyToAsync(stream);
        string begin = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"avatarFile\"; filename=\"user-avatar-{1}\"\r\n\r\n", boundary, userId);

        System.Collections.Generic.List<byte> mulFormData = new System.Collections.Generic.List<byte>(Encoding.UTF8.GetBytes(begin));
        mulFormData.AddRange(image);

        mulFormData.AddRange(Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n"));

        return mulFormData.ToArray();
    }

    public static string Delete(string data, string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(data);

        request.Method = "DELETE";
        request.ContentLength = buf.Length;
        request.ContentType = "application/json";
        request.MaximumAutomaticRedirections = 1;
        request.AllowAutoRedirect = true;

        Stream stream = request.GetRequestStream();
        stream.Write(buf, 0, buf.Length);
        stream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string result = GetResponseString(response);
        response.Close();
        return result;

    }

}
