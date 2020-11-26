using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{

    public float fadeSpeed = 3f;
    public bool sceneStarting = true;
    public RawImage rawImage;

    private static SceneFadeInOut _instance { get; set; }

    public static SceneFadeInOut GetSceneFadeInOutInstance()
    {
        return _instance;
    }
    void Awake()
    {
        rawImage.gameObject.SetActive(true);
        _instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if (sceneStarting)
            StartScene();
        if (!sceneStarting && rawImage.enabled==true)
        {
            EndScene();
        }
    }

    private void FadeToClear()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    private void FadeToBlack()
    {
        rawImage.color = Color.Lerp(rawImage.color, new Color32(34, 34, 34, 255), fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        FadeToClear();
        if (rawImage.color.a < 0.05f)
        {
            rawImage.color = Color.clear;
            rawImage.enabled = false;
            sceneStarting = false;
        }
    }

    public void EndScene()
    {
        rawImage.enabled = true;
        FadeToBlack();
        if (rawImage.color.a > 254.9f/255f)
        {
            if (Globe.fadeSceneName != "" && SceneManager.sceneCount < 2)
            {
                SceneManager.LoadSceneAsync(Globe.fadeSceneName, LoadSceneMode.Additive);
                if(Globe.loadSceneName == "InGame")
                {
                    GameObject panel = GameObject.Find("Canvas");
                    panel.SetActive(false);
                    panel = GameObject.Find("face_prefab");
                    panel.SetActive(false);
                }
            }
            else
            {
                if (Globe.loadSceneName == "Main" && SceneManager.sceneCount > 2)
                {
                    SceneManager.UnloadSceneAsync("Login");
                }
            }
        }
    }

    void OnDestroy()
    {

    }
}