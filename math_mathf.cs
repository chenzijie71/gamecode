using UnityEngine;

public class lesson1_math_mathf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region math和mathf
        //math是C#里的一个类 mathf是unityengine中的一个结构体
        //他们都封装了很多的计算方法 mathf涵盖了math中的方法 还添加了很多对游戏有关的计算方法
        //游戏开发中 我们优先选用mathf
        #endregion

        #region 常用的mathf方法
        //pi 3.1415...
        print(Mathf.PI);
        //abs 取绝对值
        print(Mathf.Abs(-100));
        //ceiltoint 向上取整 (type)强转默认向下取整
        print(Mathf.CeilToInt(0.5f));
        //floortoint 向下取整 向上向下都不四舍五入
        print(Mathf.FloorToInt(0.4f));
        //clamp 钳制函数 夹子函数
        print(Mathf.Clamp(1,5,10));//5
        print(Mathf.Clamp(11,5,10));//10
        print(Mathf.Clamp(7,5,10));//7
        //max min 取最大值最小值
        print(Mathf.Max(1,2,3,4,5,6,7,8,9,10));//10
        print(Mathf.Min(1,2,3,4,5,6,7,8,9,10));//1
        //pow 取一个数的n次幂
        print(Mathf.Pow(3,2));//9
        //roundtoint 四舍五入
        print(Mathf.RoundToInt(1.4f));//1
        print(Mathf.RoundToInt(1.5f));//2
        //sqrt 求一个数的平方根
        print(Mathf.Sqrt(2));//4
        //ispoweroftwo 判断一个数是不是2的n次方
        print(Mathf.IsPowerOfTwo(4));//true
        print(Mathf.IsPowerOfTwo(3));//false
        //sign 判断正负数 正数返回1 负数返回-1
        print(Mathf.Sign(-1));
        print(Mathf.Sign(1));
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
