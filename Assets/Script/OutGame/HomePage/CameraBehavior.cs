using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

// refer to blog online
public class CameraBehavior : MonoBehaviour
{
    private float angle;

    private float maxRotAngle;

    private float minRotAngle;

    private Vector3 minPos;

    private Vector3 maxPos;
    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
        maxRotAngle = 60;
        minPos = new Vector3(-80, 40, 140);
        maxPos = new Vector3(140, 420, 300);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        CameraFOV(mouseY);
        CameraRotate(mouseX, mouseY);
        CameraMove(mouseX, mouseY);
    }
/*
    public void OnClickRankList()
    {
        Vector3 v = new Vector3(0, 0, 10);
        this.transform.position += v;
        this.transform.rotation
    }*/
    public void CameraFOV(float mouseY)
    {
        
        //Debug.Log(Input.GetMouseButton(0));
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
        {
            float zoom = mouseY * Time.deltaTime * 2000;
            Debug.Log(zoom);
            transform.Translate(Vector3.forward * zoom);
            transform.position = ClipPos(transform.position);
        }
        /*//获取鼠标滚轮的滑动量
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 5000;

        //改变相机的位置
        transform.Translate(Vector3.forward * wheel);
        transform.position = ClipPos(transform.position);*/
    }
    
    public void CameraRotate(float mouseX,float mouseY)
    {
        
        if (Input.GetMouseButton(1))
        {
            float rotateSpeed = Time.deltaTime * 200;
           
            Vector3 centerPoint = transform.position;
            
            //控制相机绕中心点(centerPoint)水平旋转
            transform.RotateAround(centerPoint, Vector3.up, mouseX * rotateSpeed); 
            
            transform.RotateAround(centerPoint, transform.right, mouseY * rotateSpeed);
        }
    }
    public void CameraMove(float mouseX, float mouseY)
    {
        if (Input.GetMouseButton(2))
        {

            //相机位置的偏移量（Vector3类型，实现原理是：向量的加法）
            Vector3 moveDir = (mouseX * -transform.right + mouseY  * -transform.up);

            Vector3 movePosition = transform.position + moveDir * 9.0f;
            movePosition = ClipPos(movePosition);
            transform.position = movePosition;

        }
    }

    public Vector3 ClipPos(Vector3 mp)
    {
        if (mp.x < minPos.x)
            mp.x = minPos.x;
        else if (mp.x > maxPos.x)
            mp.x = maxPos.x;
        if (mp.y < minPos.y)
            mp.y = minPos.y;
        else if (mp.y > maxPos.y)
            mp.y = maxPos.y;
        if (mp.z < minPos.z)
            mp.z = minPos.z;
        else if (mp.z > maxPos.z)
            mp.z = maxPos.z;
        return mp;
    }
}
