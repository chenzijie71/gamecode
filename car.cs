using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{   
    public Transform[] wheelMeshes;
    public WheelCollider[] wheelColliders;

    public int rotateSpeed;
    public int rotationAngle;
    public int wheelRotateSpeed;   
    
    public Transform[] grassEffects;
    public Transform[] skidMarkPivots;


    public GameObject skidMark;

    public float skidMarkSize;
    public float skidMarkDelay;
    private int targetRotation;

    private WorldGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindObjectOfType<WorldGenerator>();
        StartCoroutine(SkidMark());
    }
    /// <summary>
    /// 打滑印记函数
    /// </summary>
    IEnumerator SkidMark(){
        while (true){
            yield return new WaitForSeconds(skidMarkDelay);
        
            for(int i = 0;i<skidMarkPivots.Length;i++){
                GameObject newskidMark = Instantiate(skidMark,skidMarkPivots[i].position,skidMarkPivots[i].rotation);
            
                newskidMark.transform.parent = generator.GetWorldPiece();

                newskidMark.transform.localScale = new Vector3(1,1,4)*skidMarkSize;

            }
         
        
        
        }
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        for(int i = 0;i<wheelMeshes.Length;i++){
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos,out quat);
            
            wheelMeshes[i].position = pos;

            wheelMeshes[i].Rotate(Vector3.right*Time.deltaTime*wheelRotateSpeed);
        }

        if(Input.GetMouseButton(0)||Input.GetAxis("Horizontal")!=0){
            UpdateTargetRotation();
        }else if(targetRotation!=0){
            targetRotation = 0;
        }
        Vector3 rotation = new Vector3(transform.localEulerAngles.x,targetRotation,transform.localEulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(rotation),rotateSpeed*Time.deltaTime);
    }
    
    /// <summary>
    /// 更新小车角度
    /// </summary>
    void UpdateTargetRotation(){
        if(Input.GetAxis("Horizontal")==0){
            if(Input.mousePosition.x > Screen.width * 0.5f){
                //如果鼠标在屏幕右侧就右转
                targetRotation = rotationAngle;
            }else{
                //在左侧就左转
                targetRotation = -rotationAngle;
            }
        }else{
            //如果按住ad或者左右箭头
            targetRotation = (int)(rotationAngle*Input.GetAxis("Horizontal"));
        }
    }
}
