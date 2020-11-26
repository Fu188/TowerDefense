using System;
using System.Security.Cryptography;


public class AESUtil
{
    /// <summary>
    /// 32位 默认秘钥：前后端 秘钥 应相同
    /// </summary>
    private const string PUBLICKEY = "KeyKeeper@SUSTech-Tower-Defense^";

    /// <summary>
    /// 16位 默认向量：前后端向量应相同
    /// </summary>
    private static string IV = "SUSTech@OOAD2020";

    private static string MD5SALT = "~!#%&(@$^*)";


    private const string ENCRYPYMODE = "CBC";
    private static PaddingMode PADDINGMODE = PaddingMode.PKCS7;


    private static string CHARSET = "UTF-8";
    readonly static System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(CHARSET);



    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="rawData">需要加密的原始数据</param>
    /// <param name="key">32位密钥</param>
    /// <param name="encryptMode"> ECB / CBC </param>
    /// <returns>加密后的字符串</returns>
    public static string AESEncrypt(object rawData, string key = PUBLICKEY, string encryptMode = ENCRYPYMODE)
    {
        ICryptoTransform encryptor = GetRijndaelManaged(key, encryptMode).CreateEncryptor();

        byte[] toEncryptArray = encoding.GetBytes(rawData.ToString());

        byte[] encrypted = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        // 最后用base64加密
        return Convert.ToBase64String(encrypted, 0, encrypted.Length);
    }


    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptedData">被加密的数据</param>
    /// <param name="key">32位密钥</param>
    /// /// <param name="encryptMode"> ECB / CBC </param>
    /// <returns>解密后的字符串</returns>
    public static string AESDecrypt(object encryptedData, string key = PUBLICKEY, string encryptMode = ENCRYPYMODE)
    {
        ICryptoTransform decryptor = GetRijndaelManaged(key, encryptMode).CreateDecryptor();

        //先用base64解密
        byte[] encrypted1 = Convert.FromBase64String(encryptedData.ToString());

        byte[] resultArray = decryptor.TransformFinalBlock(encrypted1, 0, encrypted1.Length);

        return encoding.GetString(resultArray);
    }


    /// <summary>
    /// 根据秘钥和加密模式获得 RijndaelManaged
    /// </summary>
    /// <param name="key">32位秘钥</param>
    /// <param name="encryptMode">ECB / CBC</param>
    /// <returns>RijndaelManaged</returns>
    private static RijndaelManaged GetRijndaelManaged(string key, string encryptMode)
    {

        var rijndael = new RijndaelManaged
        {
            Key = encoding.GetBytes(key),
            Padding = PADDINGMODE
        };

        if (encryptMode.Equals("CBC"))
        {//使用CBC模式，需要一个向量iv，可增加加密算法的强度
            rijndael.Mode = CipherMode.CBC;
            rijndael.IV = encoding.GetBytes(IV);
        }
        else
        {
            rijndael.Mode = CipherMode.ECB;
        }

        return rijndael;
    }


    /// <summary>
    /// MD5　32位加密
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetMD5(string data)
    {

        string md5Data = "";

        MD5 md5 = MD5.Create();
        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] buffer = md5.ComputeHash(encoding.GetBytes(string.Format("{0}/{1}", data, MD5SALT)));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < buffer.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符

            md5Data += buffer[i].ToString("x");

        }
        return md5Data;
    }
}
