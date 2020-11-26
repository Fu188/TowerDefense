using System.IO;
using SimpleFileBrowser;
using UnityEngine;

/**
 * https://github.com/yasirkula/UnitySimpleFileBrowser
 * https://github.com/yasirkula/UnityImageCropper
 * 
 * 1. 裁剪图片时的编辑背景
 * 2. 阻塞与协程
 **/
public class ImageUtil
{
/*    Texture texture;
    Sprite sprite;
    bool isOver;*/
    // Start is called before the first frame update

    public static ImageUtil _Instance;

    void Start()
    {
        _Instance = this;
    }


    public static ImageUtil GetImageUtilInstance()
    {
        return _Instance;
    }
    public delegate void OnUpdatedAvatarSuccess(string ts);
    public static void LoadLocalImage()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.ShowLoadDialog(successLoad, cancelLoad);
    }

    private static void successLoad(string[] paths)
    {
        ImageCropper cropper = ImageCropper.Instance;
        if (cropper.IsOpen)
        {
            return;
        }
        Texture2D _tex2 = new Texture2D(128, 128);
        _tex2.LoadImage(File.ReadAllBytes(paths[0]));
        
        cropper.Show(_tex2, cropResult, new ImageCropper.Settings
        {
            markTextureNonReadable = false,
            selectionMinAspectRatio = 1,
            selectionMaxAspectRatio = 1,
            ovalSelection = true,
            imageBackground = Color.white
        });
    }

    private static void cancelLoad()
    {

    }


    private static void cropResult(bool result, Texture originalImage, Texture2D croppedImage)
    {
        if (result)
        {
            User user = User.GetUserInstance();
            UserController.GetUserControllerInstance().ChangeAvatar(user.GetUserId(), (croppedImage).EncodeToPNG());
        }
    }

    public static Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    

}
