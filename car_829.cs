using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{      
    public Rigidbody rb;
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
    public float minRotationDifference;
    private int targetRotation;

    public float grassEffectOffset;

    public Transform back;
    public float constantBackForce;
    private WorldGenerator generator;

    private float lastRotation;

    private bool skidMarkRoutine;
    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindObjectOfType<WorldGenerator>();
        StartCoroutine(SkidMark());
    }

    private void FixedUpdate(){
        //更新车轮痕迹和粒子特效
        UpdateEffects();
    }
    void UpdateEffects(){
        //轮胎在地面上 就不加了 不在就加力
        bool addForce = true;

        bool rotated = Mathf.Abs(lastRotation-transform.localEulerAngles.y)>minRotationDifference;
        for(int i = 0;i<2;i++){
            Transform wheelMesh = wheelMeshes[i+2];

            if(Physics.Raycast(wheelMesh.position,Vector3.down,grassEffectOffset*1.5f)){
                //粒子如果没显示，让他显示出来
                if(!grassEffects[i].gameObject.activeSelf){
                    grassEffects[i].gameObject.SetActive(true);
                }

                float effectHeight = wheelMesh.position.y - grassEffectOffset;
                Vector3 targetPosition = new Vector3(grassEffects[i].position.x,effectHeight,wheelMesh.position.z);
                grassEffects[i].position = targetPosition;
                skidMarkPivots[i].position = targetPosition;
                addForce = false;
            }
            else if(grassEffects[i].gameObject.activeSelf){
                grassEffects[i].gameObject.SetActive(false);
            }
        }
        if(addForce){
            rb.AddForceAtPosition(back.position,Vector3.down*constantBackForce);
            skidMarkRoutine = false;
        }else{
            if(targetRotation!=0){
                if(rotated && !skidMarkRoutine){
                    skidMarkRoutine = true;
                }
                else if(!rotated&& skidMarkRoutine){
                    skidMarkRoutine = false;
                }
            }else{
                skidMarkRoutine = false;
            }
        }

        lastRotation = transform.localEulerAngles.y;


    }
    /// <summary>
    /// 打滑印记函数
    /// </summary>
    IEnumerator SkidMark(){
        
        while (true){
            yield return new WaitForSeconds(skidMarkDelay);

            if(skidMarkRoutine){
                for(int i = 0;i<skidMarkPivots.Length;i++){
                GameObject newskidMark = Instantiate(skidMark,skidMarkPivots[i].position,skidMarkPivots[i].rotation);
            
                newskidMark.transform.parent = generator.GetWorldPiece();

                newskidMark.transform.localScale = new Vector3(1,1,4)*skidMarkSize;

            }
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
