using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldGenerator : MonoBehaviour
{   

    public Material meshMaterial;

    public Vector2 dimensions;
    public float scale;
    public float perlinScale;

    public float offset;    
    public float waveHeight;
    //管道速度
    public float globalSpeed;
    GameObject[] pieces = new GameObject[2];
    void Start()
    {
        //CreateCylinder();
        for(int i = 0;i<2;i++){
            GenerateWorldPiece(i);
        }
    }
    /// <summary>
    /// 刷新地图
    /// </summary>
    private void LateUpdate(){
        if(pieces[0]&&pieces[1].transform.position.z<=0){
            StartCoroutine(UpdateWorldPieces());
        }
    }
    /// <summary>
    /// 要插入新管道的创建 和 旧管道的销毁
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateWorldPieces(){
        Destroy(pieces[0]);

        pieces[0] = pieces[1];

        pieces[1] = CreateCylinder();
        
        pieces[1].transform.position = pieces[0].transform.position + Vector3.forward*(dimensions.y*scale*Mathf.PI);
        pieces[1].transform.rotation = pieces[0].transform.rotation;

        UpdateSinglePiece(pieces[1]);

        yield return 0;
    }
    /// <summary>
    /// 圆柱体拼接
    /// </summary>
    /// <param name="i"></param>
    void GenerateWorldPiece(int i){
        //生成圆柱体存入数组
        pieces[i] = CreateCylinder();
        //根据他的索引去摆正圆柱体的位置
        pieces[i].transform.Translate(Vector3.forward*(dimensions.y*scale*Mathf.PI)*i);
        //再写一个函数 标记尾部的位置 将来移动用
        UpdateSinglePiece(pieces[i]);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="piece"></param>
    void UpdateSinglePiece(GameObject piece){
        //增加移动
        BasicMovement movement = piece.AddComponent<BasicMovement>();
        movement.movespeed = -globalSpeed;

        //创建结束点
        GameObject endPoint = new GameObject();
        endPoint.transform.position = piece.transform.position+Vector3.forward*(dimensions.y*scale*Mathf.PI);
        endPoint.transform.parent = piece.transform;
        endPoint.name = "End Point";
    
    }   
    /// <summary>
    /// 创建管道函数
    /// </summary>
    public GameObject CreateCylinder(){
        //代码绘制东西都是用网格来的mesh
        //meshfilter绘制网格
        //meshrender把网格渲染出来

        //创建gamerobject 名字是世界片
        GameObject newCylinder = new GameObject();
        newCylinder.name = "World piece";
        
        //给世界片上加入网格和网格渲染
        MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();

        //meshfilter需要加入网格 render需要添加材质
        meshRenderer.material = meshMaterial;
        meshFilter.mesh = Generate();

        //添加完网格和渲染后 把世界片的碰撞盒加上去
        newCylinder.AddComponent<MeshCollider>();

        return newCylinder;
    }

    /// <summary>
    /// 创建网格函数
    /// </summary>
    /// <returns></returns>
    Mesh Generate(){
        Mesh mesh = new Mesh();
        mesh.name = "MESH";

        //uv 顶点 三角形
        Vector3[] vertics = null;
        Vector2[] uvs = null;
        int[] triangles = null;
        
        //创建形状
        CreateShape(ref vertics,ref uvs,ref triangles);

        //赋值
        mesh.vertices = vertics;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        return mesh;
    }

    void CreateShape(ref Vector3[] vertices,ref Vector2[] uvs,ref int[] triangles){
        
        //向x里面延伸 xy是横截面
        int xCount = (int)dimensions.x;
        int zCount = (int)dimensions.y;

        //初始化顶点和uv数组，通过定义的尺寸
        vertices = new Vector3[(xCount+1)*(zCount+1)];
        uvs = new Vector2[(xCount+1)*(zCount+1)];
        
        int index = 0;
        //半径计算
        float radius = xCount*scale*0.5f;
        
        //通过双循环 设置顶点和uv
        for(int x = 0;x<=xCount;x++){
            for(int z = 0;z<=zCount;z++){
                //首先获得圆柱体的角度 根据x的位置
                float angle = x * Mathf.PI*2f/xCount;
                //通过角度计算顶点的值
                vertices[index] = new Vector3(Mathf.Cos(angle)*radius,Mathf.Sin(angle)*radius,z*scale*Mathf.PI);
                //接下来可以计算出uv的值
                uvs[index] = new Vector2(x*scale,z*scale);
                //现在,我们可以用之前的柏林噪声了
                float pX = (vertices[index].x*perlinScale)+offset;
                float pZ = (vertices[index].z*perlinScale)+offset;
                //需要一个中心点和当前顶点做减法然后归一化 再去计算柏林噪声
                Vector3 center = new Vector3(0,0,vertices[index].z);
                vertices[index] += (center-vertices[index]).normalized*Mathf.PerlinNoise(pX,pZ)*waveHeight;

                index++;
            }
        }

        //初始化三角形数组 
        triangles = new int[xCount*zCount*6];

        //创建一个数组 存6个三角形顶点
        int[] boxBase = new int[6];
        int current = 0;
        for(int x = 0; x<xCount;x++){
            //每次重新赋值  根据x的变化
            boxBase = new int[]{
                x*(zCount+1),
                x*(zCount+1)+1,
                (x+1)*(zCount+1),
                x*(zCount+1)+1,
                (x+1)*(zCount+1)+1,
                (x+1)*(zCount+1),
            };
            for(int z = 0;z<zCount;z++){
            //增长一下这个索引 方便计算下一个正方形
            for(int i = 0;i<6;i++){
                boxBase[i] = boxBase[i]+1;
            }
            //把6个顶点放到具体的三角形中去
            for(int j = 0;j<6;j++){
                triangles[current+j] = boxBase[j]-1;
            }

            current+=6;
        }
        }
        
    }

    public Transform GetWorldPiece(){
        return pieces[0].transform;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
