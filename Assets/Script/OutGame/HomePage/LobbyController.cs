using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour {
    void Start () {

    }

    public void Create_Room () {
        GameObject NewRoom = (GameObject) Instantiate (Resources.Load ("Prefabs/Room_Mini"));
        NewRoom.transform.localScale = new Vector3 (0.9434f, 0.9434f, 0.9434f);
        GameObject Content = GameObject.Find ("Canvas/Panel/DoublePlayer_Panel/Lobby/Scroll View/Viewport/Content");
        NewRoom.transform.parent = Content.transform;

        // Enter_Room ();
    }

    public void Enter_Room () {
        // To be changed
        GameObject Room = GameObject.Find ("Canvas/Panel/DoublePlayer_Panel/Room");
        this.transform.gameObject.SetActive (false);
        Room.SetActive (true);
    }

    public void Return_Lobby () {
        // To be changed
        GameObject Room = GameObject.Find ("Canvas/Panel/DoublePlayer_Panel/Room");
        Room.SetActive (false);
        this.transform.gameObject.SetActive (true);
    }

    // Update is called once per frame
    void Update () {

    }
}