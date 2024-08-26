using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    public Transform camTarget;

    public float height = 5f;

    public float heightDamping = 0.5f;
    public float rotationDamping = 1f;

    public float distance = 6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //update里监听鼠标事件    
    }
    void LateUpdate(){

        if(camTarget==null){
            return;
        }
        //要变换到的角度和高度
        float wantedRotationAngle = camTarget.eulerAngles.y;
        float wantedHeight = camTarget.position.y+height;
        //现在的角度和高度
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle,wantedRotationAngle,rotationDamping*Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight,wantedHeight,heightDamping*Time.deltaTime);
       
       
        Quaternion currentRotation = Quaternion.Euler(0,currentRotationAngle,0);
        
        transform.position = camTarget.position;
        transform.position -= currentRotation*Vector3.forward*distance;

        transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);

        transform.LookAt(camTarget);

    }

}
