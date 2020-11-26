using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject BuffProperty;
    public float width;
    public float height;
    public float widthOffset;
    public float heightOffset;
    [SerializeField]

    private bool isIn;

    private void Start () {
        BuffProperty = GameObject.Find ("Canvas/BasePanel/BuffProperty");
        widthOffset = 20f;
        heightOffset = 20f;
    }
    public void OnPointerEnter (PointerEventData eventData) {
        width = BuffProperty.GetComponent<RectTransform> ().rect.width;
        height = BuffProperty.GetComponent<RectTransform> ().rect.height;
        isIn = true;
        BuffProperty.GetComponentsInChildren<Text> () [0].text =
            "property example" + "\n\r" +
            "Yiqiooadma" + "\n\r" +
            "This is cold or scared debuff. They both decreases" + "\n\r" +
            "the speed you move. They can be caused by props or" + "\n\r" +
            "weathers like raining or thunder. " + "\n\r" +
            "\n\r" +
            "\n\r" +
            "test blank lines";

    }

    public void OnPointerExit (PointerEventData eventData) {

        isIn = false;
        // BlocksProperty.enabled=false;
        HideUIProperty ();
    }
    /// <summary>
    ///显示详细信息
    /// </summary>
    private void ShowUIProperty () {
        BuffProperty.SetActive (true);
        float x = Input.mousePosition.x - Screen.width * 0.5f + this.width * 0.5f + widthOffset;
        float y = Input.mousePosition.y - Screen.height * 0.5f - this.height * 0.5f - heightOffset;
        Vector2 localPoint = new Vector2 (x, y);
        BuffProperty.transform.localPosition = localPoint;
    }
    /// <summary>
    /// 内容归为空
    /// </summary>
    private void HideUIProperty () {
        BuffProperty.GetComponentsInChildren<Text> () [0].text = string.Empty;
        BuffProperty.SetActive (false);

    }
    private void Update () {

        if (isIn) {
            ShowUIProperty ();
        }
    }

}