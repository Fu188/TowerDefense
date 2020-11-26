using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterConfirm : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private bool isSelect = false;
    private EventSystem system;
    public Button confirmBtn;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
    }

    private void GetEnterDown()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isSelect)
        {
            confirmBtn.onClick.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        GetEnterDown();
    }

    /// <summary>
    /// 实现ISelectHandler接口
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        isSelect = true;
    }
    /// <summary>
    /// 实现IDeselectHandler接口
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        isSelect = false;
    }
}
