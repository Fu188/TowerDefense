using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Globe
{
    public static string loadSceneName;
    public static string fadeSceneName;
}

public class AsyncLoadScene : MonoBehaviour
{
    public Text loadingText;
    public Image progressBar;
    public Image RotatingImage;

    private int curImage = 0;

    private AsyncOperation op;

    void Start()
    {
        //开启协程
        StartCoroutine("loginMy");
        InvokeRepeating("switchImage", 0.05f, 0.05f);
    }

    void switchImage()
    {
        string path = "Textures/Loader/" + (curImage + 1).ToString();
        Texture texture = Resources.Load(path) as Texture;
        RotatingImage.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        curImage = (curImage + 1) % 40;
    }

    void Update()
    {

    }
    IEnumerator loginMy()
    {
        yield return new WaitForSeconds(1f);
        int displayProgress = 0;
        int toProgress = 0;
        op = SceneManager.LoadSceneAsync(Globe.loadSceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)  //此处如果是 <= 0.9f 则会出现死循环所以必须小0.9
        {
            toProgress = (int)(op.progress * 100);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();//ui渲染完成之后
            }
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        Globe.fadeSceneName = "";
        SceneFadeInOut.GetSceneFadeInOutInstance().EndScene();
        Invoke("SetTrue", 1);

    }
    private void SetLoadingPercentage(int displayProgress)
    {
        progressBar.fillAmount = displayProgress / 100f;
        loadingText.text = displayProgress.ToString() + "%";
    }

    private void SetTrue()
    {
        //SceneManager.UnloadSceneAsync("Loading");
        op.allowSceneActivation = true;
    }
}