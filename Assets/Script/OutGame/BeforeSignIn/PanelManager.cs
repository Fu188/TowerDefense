using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [HideInInspector]
	public GameObject SettingPanel;
    [HideInInspector]
    public GameObject AboutUsPanel;
    [HideInInspector]
    public GameObject SignUpPanel;
    [HideInInspector]
    public GameObject ForgetPasswordPanel;
    public GameObject NoticePanel;

    public InputField InputEmail;
    public InputField InputPassword;

    public Button SignUpConfirmBtn;
    public Button ResetPasswordBtn;
    public Button SignUpSendCodeBtn;
    public Button ResetPasswordSendCodeBtn;

    public CountDownTimer SignUpCodeTimer;
    public bool isSignUpCounting;
    public CountDownTimer ResetPasswordCodeTimer;
    public bool isResetPasswordCounting;

    public UserController userController;

    // Start is called before the first frame update
    void Start()
    {
        SettingPanel = GameObject.Find("Canvas/Panel/SettingPanel");
        AboutUsPanel = GameObject.Find("Canvas/Panel/AboutUsPanel");
        SignUpPanel = GameObject.Find("Canvas/Panel/SignUpPanel");
        ForgetPasswordPanel = GameObject.Find("Canvas/Panel/ForgetPasswordPanel");
        userController = UserController.GetUserControllerInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSignUpCounting)
        {
            if (!SignUpCodeTimer.IsStoped)
            {
                int LeftTime = (int)SignUpCodeTimer.CurrentTime;
                SignUpSendCodeBtn.GetComponentInChildren<Text>().text = LeftTime + "s";
            }
            else
            {
                isSignUpCounting = false;
                SignUpSendCodeBtn.GetComponentInChildren<Text>().text = "Get code";
                SignUpSendCodeBtn.gameObject.SetActive(true);
            }
        }
        if (isResetPasswordCounting)
        {
            if (!SignUpCodeTimer.IsStoped)
            {
                int LeftTime = (int)ResetPasswordCodeTimer.CurrentTime;
                ResetPasswordSendCodeBtn.GetComponentInChildren<Text>().text = LeftTime + "s";
            }
            else
            {
                isResetPasswordCounting = false;
                ResetPasswordSendCodeBtn.GetComponentInChildren<Text>().text = "Get code";
                ResetPasswordSendCodeBtn.gameObject.SetActive(true);
            }
        }
    }
    public void OnClickSignIn()
    {
        /*if (InputEmail.text.Trim() == "")
        {
            SendNotice("Blank EMAIL. Please input your EMAIL.");
        }
        else if (!isEmail(InputEmail.text.Trim()))
        {
            SendNotice("Invalid EMAIL format. ");
        }
        else if (InputEmail.text.Trim() == "keepers@keykeeper.ga" && InputPassword.text.Trim() == "admin")
        {
            SceneManager.LoadSceneAsync("Main");
        }
        else
        {
            SendNotice("EMAIL does not exist or PASSWORD is incorrect.");
        }*/

        if (InputEmail.text.Trim() == "")
        {
            SendNotice("Blank EMAIL. Please input your EMAIL.");
        }
        else if (!RegexUtil.IsEmail(InputEmail.text.Trim()))
        {
            SendNotice("Invalid EMAIL format. ");
        }
        else if (userController.SignIn(InputEmail.text.Trim(), InputPassword.text.Trim()))
        {
            //SceneManager.LoadSceneAsync("Main");
            Globe.loadSceneName = "Main";//目标场景名称
            Globe.fadeSceneName = "Loading";
            SceneFadeInOut.GetSceneFadeInOutInstance().EndScene();
        }
        else
        {
            SendNotice("EMAIL does not exist or PASSWORD is incorrect.");
        }
    }

    public void OnClickSignUp()
    {
        SignUpPanel.SetActive(true);
    }

    public void OnClickForgetPassword()
    {
        ForgetPasswordPanel.SetActive(true);
    }

    public void OnCliCkSignUpSendCode()
    {
        string email = SignUpPanel.GetComponentInChildren<InputField>().text.Trim();
        if (!RegexUtil.IsEmail(email))
        {
            SendNotice("Invalid EMAIL format. ");
            return;
        }
        bool sentSuccess = userController.GetSignUpCode(email);
        if (!sentSuccess)
        {
            SendNotice("Code was not sent successfully. Please try again. ");
        }
        else
        {
            SendNotice("Success! Valid time of vertification is 60s. ");
            SignUpConfirmBtn.interactable = true;
            SignUpSendCodeBtn.interactable = false;
            SignUpCodeTimer = new CountDownTimer(60);
            isSignUpCounting = true;
        }
    }

    public void OnClickSignUpConfirm()
    {
        InputField[] inputs = SignUpPanel.GetComponentsInChildren<InputField>();
        string email = inputs[0].text;
        string nickname = inputs[1].text;
        string password = inputs[2].text;
        string passwordConfirm = inputs[3].text;
        string vertificationCode = inputs[4].text;
        if (!password.Equals(passwordConfirm))
        {
            SendNotice("Two passwords are not same.");
        }
        else if (nickname == "")
        {
            SendNotice("Invalid nickname. ");
        }
        else if(userController.SignUp(email, password, nickname, vertificationCode))
        {
            SendNotice("Congratulations! Sign up successfully!");
            Exit();
        }
        else
        {
            SendNotice("Failed to sign up. ");
        }
    }

    public void OnClickForgetPasswordSendCode()
    {
        string email = SignUpPanel.GetComponentInChildren<InputField>().text.Trim();
        if (!RegexUtil.IsEmail(email))
        {
            SendNotice("Invalid EMAIL format. ");
            return;
        }
        bool sentSuccess = userController.GetSignUpCode(email);
        if (!sentSuccess)
        {
            SendNotice("Code was not sent successfully or user does not exist.");
        }
        else
        {
            SendNotice("Success! Valid time of vertification is 60s. ");
            ResetPasswordBtn.interactable = true;
            ResetPasswordSendCodeBtn.interactable = false;
            ResetPasswordCodeTimer = new CountDownTimer(60);
            isResetPasswordCounting = true;
        }
    }

    public void OnClickForgetPasswordConfirm()
    {
        InputField[] inputs = SignUpPanel.GetComponentsInChildren<InputField>();
        string email = inputs[0].text;
        string password = inputs[1].text;
        string passwordConfirm = inputs[2].text;
        string vertificationCode = inputs[3].text;
        if (!password.Equals(passwordConfirm))
        {
            SendNotice("Two passwords are not same.");
        }
        else if (userController.ResetPwd(email, password, vertificationCode))
        {
            SendNotice("Congratulations! Reset successfully!");
            Exit();
        }
        else
        {
            SendNotice("Failed to reset. ");
        }
    }
    public void Setting(){
        SettingPanel.SetActive(true);
        AboutUsPanel.SetActive(false);
    }

    public void AboutUs(){
        SettingPanel.SetActive(false);
        AboutUsPanel.SetActive(true);
    }

    public void Exit(){
        SettingPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        SignUpPanel.SetActive(false);
        ForgetPasswordPanel.SetActive(false);
    }

    public void SendNotice(string message)
    {
        StartCoroutine(FullNotice(message));
    }

    IEnumerator FullNotice(string message)
    {
        NoticePanel.GetComponentsInChildren<Text>()[0].text = message;
        NoticePanel.SetActive(true);
        RectTransform NoticeRect = NoticePanel.transform as RectTransform;
        RectTransform Mask = NoticePanel.transform.GetChild(0).GetChild(0).transform as RectTransform;
        Mask.offsetMax = new Vector2(0, -35);
        Mask.offsetMin = new Vector2(0, 35);
        yield return StartCoroutine(NoticeShow(Mask));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(NoticeClose(Mask));
        NoticePanel.SetActive(false);
    }

    IEnumerator NoticeShow(RectTransform Mask)
    {
        while (Mask.offsetMax[1] < 0)
        {
            Mask.offsetMax = Mask.offsetMax + new Vector2(0, Time.deltaTime * 300);
            Mask.offsetMin = Mask.offsetMin - new Vector2(0, Time.deltaTime * 300);
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator NoticeClose(RectTransform Mask)
    {
        while (Mask.offsetMax[1] > -35)
        {
            Mask.offsetMax = Mask.offsetMax - new Vector2(0, Time.deltaTime * 300);
            Mask.offsetMin = Mask.offsetMin + new Vector2(0, Time.deltaTime * 300);
            yield return new WaitForSeconds(0);
        }
    }

    
}
