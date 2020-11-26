using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour {
	public GameObject Card_Panel;
	public GameObject Friend_Panel;
	public GameObject Achievement_Panel;
	public GameObject Store_Panel;
	public GameObject Inventory_Panel;
	public GameObject Handbook_Panel;
	public GameObject StartMask;
	public GameObject Mainplot_Panel;
	public GameObject Daily_Panel;
	public GameObject DoublePlayer;
	public GameObject Preparation_Panel;
	public GameObject OnlinePreparationPanel;
	public GameObject RankListPanel;
	public GameObject MailBoxPanel;
	public GameObject[] Panels;
	public GameObject ReturnBtn;

	public GameObject mainCamera;

	public Image Avatar;
	public Text Level;
	public Text Nickname;
	public Text Exp;
	public int nowExp = 0;
	public int requireExp = 0;
	public Image ExpBar;
	public Text Hair;
	public Text HairTime;

	public int HairNum;
	public int HairMax;
	public Text NKB;
	public Text Credit;

	public GameObject[] Card;
	public List<GameObject> Achievement = new List<GameObject>();
	public GameObject[] StoreCard;
	public GameObject[] StoreProp;
	public GameObject[] StoreDaily;
	public GameObject[] StoreRare;
	public GameObject[] Inventory;
	public GameObject[] Handbook;

	public GameObject LoadingPanel;
	public Image RotatingImage;
	private int curImage = 0;

	public User user;

	public int DungeonNumber;
	private static HomePageManager _instance { get; set; }

	public static HomePageManager GetHomePageManagerInstance()
	{
		return _instance;
	}

	public void Awake()
	{
		// Avatar.sprite
		_instance = this;
		user = User.GetUserInstance();
	}

	public void Start() {
		this.Panels = new GameObject[]{Card_Panel, Friend_Panel, Achievement_Panel
		, Store_Panel, Inventory_Panel, Handbook_Panel, StartMask, Mainplot_Panel
		, Daily_Panel, DoublePlayer, RankListPanel, MailBoxPanel};

		UserInfoInitialize();
		
		SceneManager.UnloadSceneAsync("Loading");
	}


	#region panelControl
	public void card() {
		setPanelActive(0);
		ReturnBtn.SetActive(true);
	}

	public void friend() {
		setPanelActive(1);
		ReturnBtn.SetActive(true);
	}

	public void achievement() {
		setPanelActive(2);
		ReturnBtn.SetActive(true);
	}

	public void store() {
		setPanelActive(3);
		ReturnBtn.SetActive(true);
	}

	public void inventory() {
		setPanelActive(4);
		ReturnBtn.SetActive(true);
	}

	public void handbook() {
		setPanelActive(5);
		ReturnBtn.SetActive(true);
	}

	public void startmask() {
		setPanelActive(6);
		StartMask.transform.GetChild(1).gameObject.SetActive(true);
		StartMask.transform.GetChild(2).gameObject.SetActive(false);
	}

	public void mainplot() {
		setPanelActive(7);
		PreparationPanelInit(Preparation_Panel);
	}

	public void daily() {
		setPanelActive(8);
	}

	public void doubleplayer() {
		setPanelActive(9);
	}

	public void rankList()
	{
		setPanelActive(10);
		ReturnBtn.SetActive(true);
	}

	public void mailBox()
	{
		setPanelActive(11);
		ReturnBtn.SetActive(true);
	}

	public void preparation() {
		Preparation_Panel.SetActive(true);
	}

	public void preparation_disable() {
		Preparation_Panel.SetActive(false);
	}

	public void OnlinePreparation()
	{
		OnlinePreparationPanel.SetActive(true);
	}

	public void OnlinePreparationDisable()
	{
		OnlinePreparationPanel.SetActive(false);
	}

	public void returnMain() {
		setPanelActive(Panels.Length);
		ReturnBtn.SetActive(false);
	}

	public void setPanelActive(int index) {
		if (index == Panels.Length) {
			for (int i = 0; i < Panels.Length; i++) {
				Panels[i].SetActive(false);
			}
		}
		else {
			for (int i = 0; i < Panels.Length; i++) {
				if (i == index) {
					Panels[i].SetActive(true);
				}
				else {
					Panels[i].SetActive(false);
				}
			}
		}
	}

	public void OnClickAvatar()
	{
		ImageUtil.LoadLocalImage();
	}
	#endregion

	#region initialize
	public void UserInfoInitialize()
	{
		Level.text = "Lv. " + UserInfoManager.CurrentLevel;
		if (UserInfoManager.CurrentLevel == 45)
		{
			Exp.text = "MAX / MAX";
		}
		else
		{
			Exp.text = UserInfoManager.LevelExp.ToString("N0") + " / " + UserInfoManager.RequireExp.ToString("N0");
			ExpBar.fillAmount = (float)UserInfoManager.LevelExp / UserInfoManager.RequireExp;
		}

		Nickname.text = user.GetNickname();
		HairMax = UserInfoManager.HairNumLimit;
		HairNum = UserInfoManager.CurrentHairNum;

		NKB.text = UserInfoManager.CurrentNikeCoin.ToString("N0");
		Credit.text = UserInfoManager.CurrentCredit.ToString("N0");

		AchievementInitialize();
		CardInitialize();
		PropInitialize();
		HandbookInitialize();
		KkMailManager.GetKkMailManagerInstance().KkMailInitialize();
		RankListPanelManager.GetRankListPanelManagerInstance().RankListInitialize();
		string url = user.GetAvatar();
		//string url = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1605294864125&di=5c3767a20971f33de041d0a39be7c863&imgtype=0&src=http%3A%2F%2Fimg1.doubanio.com%2Fview%2Fgroup_topic%2Fl%2Fpublic%2Fp148093158.jpg";
		ShowImage(url);
		OnlineController.GetOnlineControllerInstance().ConnectInfos = PhotonNetWorkManager.GetConnectInfos();
	}

	public void AchievementInitialize()
	{
		List<Achievement> achievements = GeneralManager.Instance.achievements;
		int AchievedIndex = 0;
		int NotAchievedIndex = 0;

		for (int i = 0; i < achievements.Count; i++)
		{
			Achievement a = achievements[i];
			GameObject NewAchievement = (GameObject)Instantiate(Resources.Load("Prefabs/Achievement_Example"));
			string now = "";
			bool isAchieved = false;
			if (!a.IsRewarded)
			{
				if (a.Now > a.Target)
				{
					now = a.Target.ToString();
					isAchieved = true; AchievedIndex++;
				}
				else
				{
					now = a.Now.ToString();
					NotAchievedIndex++;
				}
			}
			else
			{
				now = a.Target.ToString();
			}
			string reward = "奖励：";
			for (int j = 0; j < a.RewardType.Length; j++)
			{
				reward += a.RewardNum[j].ToString("N0");
				switch (a.RewardType[j])
				{
					case RewardType.Exp:
						reward += "经验值";
						break;
					case RewardType.NKB:
						reward += "你科币";
						break;
					case RewardType.Credit:
						reward += "学分";
						break;
				}
				if (j < a.RewardType.Length - 1)
				{
					reward += "，";
				}
			}

			if (isAchieved)
			{
				NewAchievement.GetComponent<Button>().interactable = true;
				Achievement.Insert(AchievedIndex - 1, NewAchievement);
			}
			else if (!a.IsRewarded)
			{
				Achievement.Insert(AchievedIndex + NotAchievedIndex - 1, NewAchievement);
			}
			else
			{
				Achievement.Add(NewAchievement);
			}
			Text[] texts = NewAchievement.transform.GetComponentsInChildren<Text>();
			texts[0].text = a.Name;
			texts[1].text = a.Description;
			texts[2].text = now + " / " + a.Target;
			texts[3].text = reward;
			Achievement.Add(NewAchievement);
		}
		GameObject AchievementContent = GameObject.Find("Canvas/Panel/Achievement_Panel/Achievement_Display_Panel/Scroll View/Viewport/Content");
		for (int i = 0; i < AchievementContent.transform.childCount; i++)
		{
			Destroy(AchievementContent.transform.GetChild(i).gameObject);
		}
		foreach (GameObject a in Achievement)
		{
			a.transform.SetParent(AchievementContent.transform);
		}
	}

	public GameObject CardDetailPanel;

	public void CardInitialize()
    {
		GameObject CardContent = GameObject.Find("Canvas/Panel/CardPanel/CardDisplayPanel/Scroll View/Viewport/Content");
		for (int i = 0; i < CardContent.transform.childCount; i++)
		{
			Destroy(CardContent.transform.GetChild(i).gameObject);
		}

		List<Card> cards = GeneralManager.Instance.cards;
		for(int i = 0; i < cards.Count; i++)
        {
			Card card = cards[i];
            if (card.IsObtained)
            {
				GameObject NewCard = Instantiate(Resources.Load("Prefabs/CardItem"), CardContent.transform) as GameObject;
				NewCard.transform.GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
				NewCard.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = card.Name;
				NewCard.transform.GetChild(1).gameObject.SetActive(false);
				NewCard.GetComponent<Button>().interactable = true;
				NewCard.GetComponent<Button>().onClick.AddListener(delegate ()
				{
					CardDetailPanel.SetActive(true);
					CardDetailPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
					Text[] texts = CardDetailPanel.transform.GetChild(0).GetChild(1).GetComponentsInChildren<Text>();
					texts[0].text = card.Name;
					texts[1].text = card.CreateCost.ToString();
					texts[2].text = card.Description;
					CardDetailPanel.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = card.Attack / 200f;
					CardDetailPanel.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = 1 / card.ShootWaitTime / 2f;
				});
            }
            else
            {
				GameObject NewCard = Instantiate(Resources.Load("Prefabs/CardItem"), CardContent.transform) as GameObject;
				NewCard.transform.GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
				NewCard.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = card.Name;
				NewCard.transform.GetChild(1).gameObject.SetActive(true);
				//NewCard.GetComponent<Button>().interactable = false;
				NewCard.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "NKB: " + card.Price.ToString("N0");
				NewCard.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate ()
				{
					if (UserInfoManager.CurrentNikeCoin >= card.Price)
					{
						UserInfoManager.UpdateNikeCoin(-card.Price);
						UserInfoManager.UpdateUserBackEndInfo(1);
						GeneralManager.Instance.cards[card.Id].IsObtained = true;
						NewCard.transform.GetChild(1).gameObject.SetActive(false);
						User.GetUserInstance().SetCard(JsonConvert.SerializeObject(GeneralManager.Instance.cards));
						UserInfoManager.UpdateUserBackEndInfo(3);
						SendNotice("Success! You already have " + card.Name + ". ");
					}
					else
					{
						SendNotice("You do not have enough NikeCoin!!!");
					}
				});
				NewCard.GetComponent<Button>().onClick.AddListener(delegate ()
				{
					CardDetailPanel.SetActive(true);
					CardDetailPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
					Text[] texts = CardDetailPanel.transform.GetChild(0).GetChild(1).GetComponentsInChildren<Text>();
					texts[0].text = card.Name;
					texts[1].text = card.CreateCost.ToString();
					texts[2].text = card.Description;
					CardDetailPanel.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = card.Attack / 200f;
					CardDetailPanel.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = 1 / card.ShootWaitTime / 2f;
				});
			}
        }
	}

	public void CardPanelDisable()
    {
		CardDetailPanel.SetActive(false);
	}

	public void PropInitialize()
	{
		GameObject PropContent = GameObject.Find("Canvas/Panel/InventoryPanel/InventoryDisplayPanel/Scroll View/Viewport/Content");
		for (int i = 0; i < PropContent.transform.childCount; i++)
		{
			Destroy(PropContent.transform.GetChild(i).gameObject);
		}

		List<Prop> props = GeneralManager.Instance.props;
		int index = 0;
		for (int i = 0; i < props.Count; i++)
		{
			Prop prop = props[i];
			if (prop.IsObtained)
			{
				index++;
				GameObject NewProp = Instantiate(Resources.Load("Prefabs/InventoryItem"), PropContent.transform) as GameObject;
				NewProp.transform.GetComponent<Image>().sprite = Resources.Load(prop.ImgPath, typeof(Sprite)) as Sprite;
			}
		}
		//print(PropContent.transform.childCount);
		for (int i = index; i < 64; i++)
		{
			GameObject NewProp = Instantiate(Resources.Load("Prefabs/InventoryItem"), PropContent.transform) as GameObject;
		}
	}

	public void HandbookInitialize()
	{
		GameObject HandbookContent = GameObject.Find("Canvas/Panel/HandbookPanel/HandbookDisplayPanel/Scroll View/Viewport/Content");
		for (int i = 0; i < HandbookContent.transform.childCount; i++)
		{
			Destroy(HandbookContent.transform.GetChild(i).gameObject);
		}
        //print(GeneralManager.Instance.enemies);
        List<EnemyInfo> enemies = GeneralManager.Instance.enemyInfos;
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInfo enemy = enemies[i];
            GameObject NewEnemy = Instantiate(Resources.Load("Prefabs/CardItem"), HandbookContent.transform) as GameObject;
			NewEnemy.transform.GetChild(1).gameObject.SetActive(false);
            if (enemy.isObtained)
            {
                NewEnemy.transform.GetComponent<Image>().sprite = Resources.Load(enemy.ImgPath_160_468, typeof(Sprite)) as Sprite;
                NewEnemy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = enemy.Name;
            }
            else
            {
                NewEnemy.transform.GetComponent<Image>().sprite = Resources.Load("Textures/Enemies/Default", typeof(Sprite)) as Sprite;
                NewEnemy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "???";
            }
        }
    }
	#endregion

	#region showUrlImage
	public void ShowImage(Image image, string Url)
	{
		StartCoroutine(GetFullSprite(image, Url));
	}

	public void ShowImage(string Url)
	{
		StartCoroutine(GetFullSprite(Avatar, Url));
	}
	IEnumerator GetFullSprite(Image image, string Url)
	{
		yield return StartCoroutine(GetTexture(image, Url));
	}
	/// <summary>
	/// 通过Url获取到Image
	/// </summary>
	/// <param name="Url"></param>
	/// <returns></returns>
	IEnumerator GetTexture(Image image, string Url)
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url);
		print(Url);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			image.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			print(texture);
		}
	}
    #endregion

    #region loading
    public void StartLoading()
	{
		LoadingPanel.SetActive(true);
		InvokeRepeating("switchImage", 0.05f, 0.05f);
	}

	public void EndLoading()
	{
		CancelInvoke();
		LoadingPanel.SetActive(false);
	}

	public void CancelLoading()
    {
		PhotonNetWorkManager.Disconnect();
		EndLoading();
    }

	void switchImage()
	{
		string path = "Textures/Loader/" + (curImage + 1).ToString();
		Texture texture = Resources.Load(path) as Texture;
		RotatingImage.sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		curImage = (curImage + 1) % 40;
	}
	#endregion

	#region prepare choose

	public GameObject ChooseReturnBtn;
	public GameObject ChooseCardPanel;
	public GameObject ChooseCardContainer;
	public GameObject ChoosePropPanel;
	public GameObject ChoosePropContainer;
	public List<int> isObtainedCards = new List<int>();
	public bool[] isChosen;
	public int[] isChosenCards;
	public int CardIndex;
	public List<int> isObtainedProps = new List<int>();
	public bool[] propIsChosen;
	public int[] isChosenProps;
	public int PropIndex;

	public void PreparationPanelInit(GameObject Preparation_Panel)
	{
		isObtainedCards = new List<int>();
		for (int i = 0; i < ChooseCardContainer.transform.childCount; i++)
		{
			Destroy(ChooseCardContainer.transform.GetChild(i).gameObject);
		}
		List<Card> cards = GeneralManager.Instance.cards;
		isChosen = new bool[cards.Count];
		isChosenCards = new int[8];
		for(int i = 0; i < 8; i++)
        {
			isChosenCards[i] = -1;
        }
		for (int i = 0; i < cards.Count; i++)
		{
			Card card = cards[i];
			if (card.IsObtained)
			{
				isObtainedCards.Add(i);
				GameObject NewCard = Instantiate(Resources.Load("Prefabs/CardItem"), ChooseCardContainer.transform) as GameObject;
				
				NewCard.transform.GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
				NewCard.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = card.Name;
				NewCard.transform.GetChild(1).gameObject.SetActive(false);
				NewCard.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
				NewCard.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
				int index = i;
				NewCard.GetComponent<Button>().onClick.AddListener(delegate ()
				{
					isChosen[card.Id] = true;
					if(isChosenCards[CardIndex] > -1)
                    {
						isChosen[isChosenCards[CardIndex]] = false;
                    }
					isChosenCards[CardIndex] = index;
					Preparation_Panel.transform.GetChild(1).GetChild(2).GetChild(CardIndex).GetComponent<Image>().sprite = Resources.Load(card.ImgPath_large, typeof(Sprite)) as Sprite;
					Preparation_Panel.transform.GetChild(1).GetChild(2).GetChild(CardIndex).GetChild(0).gameObject.SetActive(false);
					for (int j = 0; j < isObtainedCards.Count; j++)
                    {
                        if (isChosen[isObtainedCards[j]])
                        {
							ChooseCardContainer.transform.GetChild(j).GetComponent<Button>().interactable = false;
						}
                        else
                        {
							ChooseCardContainer.transform.GetChild(j).GetComponent<Button>().interactable = true;
						}
                    }
					ChooseReturn();
				});
			}
		}
		for(int i = 0; i < 8; i++)
        {
			int index = i;
			Preparation_Panel.transform.GetChild(1).GetChild(2).GetChild(i).GetComponent<Image>().sprite = null;
			Preparation_Panel.transform.GetChild(1).GetChild(2).GetChild(i).GetChild(0).gameObject.SetActive(true);
			Preparation_Panel.transform.GetChild(1).GetChild(2).GetChild(i).GetComponent<Button>().onClick.AddListener(delegate ()
			{
				ChooseCard(index);
			});
        }
		//cards above
		//props below
		isObtainedProps = new List<int>();
		for (int i = 0; i < ChoosePropContainer.transform.childCount; i++)
		{
			Destroy(ChoosePropContainer.transform.GetChild(i).gameObject);
		}
		List<Prop> props = GeneralManager.Instance.props;
		propIsChosen = new bool[props.Count];
		isChosenProps = new int[8];
		for (int i = 0; i < 8; i++)
		{
			isChosenProps[i] = -1;
		}
		for (int i = 0; i < props.Count; i++)
		{
			Prop prop = props[i];
			if (prop.IsObtained)
			{
				isObtainedProps.Add(i);
				GameObject NewProp = Instantiate(Resources.Load("Prefabs/InventoryItem"), ChoosePropContainer.transform) as GameObject;

				NewProp.transform.GetComponent<Image>().sprite = Resources.Load(prop.ImgPath, typeof(Sprite)) as Sprite;
				int index = i;
				NewProp.GetComponent<Button>().onClick.AddListener(delegate ()
				{
					propIsChosen[prop.Id] = true;
					if (isChosenProps[PropIndex] > -1)
					{
						propIsChosen[isChosenProps[PropIndex]] = false;
					}
					isChosenProps[PropIndex] = index;
					Preparation_Panel.transform.GetChild(1).GetChild(3).GetChild(PropIndex).GetComponent<Image>().sprite = Resources.Load(prop.ImgPath, typeof(Sprite)) as Sprite;
					Preparation_Panel.transform.GetChild(1).GetChild(3).GetChild(PropIndex).GetChild(0).gameObject.SetActive(false);
					for (int j = 0; j < isObtainedProps.Count; j++)
					{
						if (propIsChosen[isObtainedProps[j]])
						{
							ChoosePropContainer.transform.GetChild(j).GetComponent<Button>().interactable = false;
						}
						else
						{
							ChoosePropContainer.transform.GetChild(j).GetComponent<Button>().interactable = true;
						}
					}
					ChooseReturn();
				});
			}
		}
		for (int i = 0; i < 8; i++)
		{
			int index = i;
			Preparation_Panel.transform.GetChild(1).GetChild(3).GetChild(i).GetComponent<Image>().sprite = null;
			Preparation_Panel.transform.GetChild(1).GetChild(3).GetChild(i).GetChild(0).gameObject.SetActive(true);
			Preparation_Panel.transform.GetChild(1).GetChild(3).GetChild(i).GetComponent<Button>().onClick.AddListener(delegate ()
			{
				ChooseProp(index);
			});
		}
	}
	public void ChooseCard(int i)
    {
		CardIndex = i;
		ChooseReturnBtn.SetActive(true);
		ChooseCardPanel.SetActive(true);

	}
	public void ChooseProp(int i)
    {
		PropIndex = i;
		ChooseReturnBtn.SetActive(true);
		ChoosePropPanel.SetActive(true);
	}

	public void ChooseReturn()
    {
		ChooseReturnBtn.SetActive(false);
		ChooseCardPanel.SetActive(false);
		ChoosePropPanel.SetActive(false);
    }

    #endregion

	public void ChooseDungeon(int index)
    {
		this.DungeonNumber = index;
		Preparation_Panel.transform.GetChild(1).GetChild(6).GetComponent<Text>().text = "Dungeon NO." + (this.DungeonNumber + 1);
    }

    public void StartGame()
	{
		if (UserInfoManager.CurrentHairNum >= 10)
		{
			if (Preparation_Panel.activeInHierarchy)
			{
				Preparation_Panel.SetActive(false);
			}
			mainCamera.GetComponent<AudioListener>().enabled = false;
			//SceneManager.LoadSceneAsync("InGame", LoadSceneMode.Additive);

			UserInfoManager.LoseHair(10);
			UserInfoManager.UpdateUserBackEndInfo(2);

			Globe.loadSceneName = "InGame";//目标场景名称
			Globe.fadeSceneName = "Loading";
			SceneFadeInOut.GetSceneFadeInOutInstance().EndScene();
		}
        else
        {
			SendNotice("Your hair is not enough!!!!!!!");
        }
	}
	public void Update()
    {
		HairTime.text = UserInfoManager.hairTimeText;
		Hair.text = UserInfoManager.CurrentHairNum + " /" + UserInfoManager.HairNumLimit;
		NKB.text = UserInfoManager.CurrentNikeCoin.ToString("N0");
		Credit.text = UserInfoManager.CurrentCredit.ToString("N0");
	}

	#region notice
	public GameObject NoticePanel;
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

    #endregion

}
