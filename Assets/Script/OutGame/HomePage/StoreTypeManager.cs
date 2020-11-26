using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTypeManager : MonoBehaviour
{
    public GameObject Card_Store;
    public GameObject Prop_Store;
    public GameObject Daily_Store;
    public GameObject Rare_Store;
    public Button Card_btn;
    public Button Prop_btn;
    public Button Daily_btn;
    public Button Rare_btn;
    public GameObject[] Stores;
    public Button[] btns;
    // Start is called before the first frame update
    void Start()
    {
        Stores = new GameObject[]{Card_Store, Prop_Store, Daily_Store, Rare_Store};
        btns = new Button[]{Card_btn, Prop_btn, Daily_btn, Rare_btn};
    }

    public void setTypeActive(int index){
        for(int i = 0; i < Stores.Length; i++){
            if(i == index){
                Stores[i].SetActive(true);
                btns[i].interactable = false;
            }
            else{
                Stores[i].SetActive(false);
                btns[i].interactable = true;
            }
        }
	}

    public void card(){
        setTypeActive(0);
    }

    public void prop(){
        setTypeActive(1);
    }

    public void daily(){
        setTypeActive(2);
    }

    public void rare(){
        setTypeActive(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
