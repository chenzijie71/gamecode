using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// playerprefs数据管理类
/// </summary>

public class PlayerPrefsDataMgr 
{   
    //用单例模式写
    //单例模式 两个静态一个私有 一个公开 还有一个私有的构造函数 不让外部实例化
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();
    public static PlayerPrefsDataMgr Instance{
        get { return instance; }
    }
    private PlayerPrefsDataMgr(){

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="keyname">存储名字</param>
    public void SaveData(object data , string keyname){
        //获取传入数据对象的所有字段 用type反射获取
        Type datatype = data.GetType();
        //获取这个对象的所有字段
        FieldInfo[] infos = datatype.GetFields();//返回一个字段数组
        for(int i = 0; i < infos.Length; i++){
            Debug.Log(infos[i]);
        }
        //现在要往里面存了 所以要保持key的唯一性 一个key对应一个value
        //所以就要定义一个key的规则 进行数据存储
        //key的规则:keyname_数据类型_字段类型_字段名
        string savekeyname = "";
        FieldInfo info;
        for(int i = 0;i<infos.Length;i++){
            info = infos[i];
            savekeyname = keyname+"_"+datatype.Name+"_"+info.FieldType+"_"+info.Name;
            //Debug.Log(savekeyname);
            //遍历这些字段 进行存储
            SaveValue(info.GetValue(data), savekeyname);
            //info.GetValue(data) 返回的是object 是value的值
        }
        

    }

    /// <summary>
    /// 外部封装一个函数 用来save数据
    /// </summary>
    /// <param name="value">传进来的数据</param>
    /// <param name="keyname">savekeyname</param>
    private void SaveValue(object value,string keyname) {
        //传进来这个value 不知道什么类型 playerprefs只能存三种类型 所以需要先判断
        //switch只能判断常量 所以用ifelse
        Type fieldType= value.GetType();
        if(fieldType==typeof(int)){
            Debug.Log("存储int"+keyname); 
            PlayerPrefs.SetInt(keyname, (int)value);
        }else if(fieldType==typeof(string)){
            Debug.Log("存储string"+keyname); 
            PlayerPrefs.SetString(keyname, value.ToString());
            //直接tostring为什么更好?
            //使用 (string)value 不如 value.ToString() 的原因：
            //安全性：(string)value 对于 null 会抛出异常，而 ToString() 更安全，通常返回 "null" 或其他合适的默认值。
            //类型多样性：ToString() 支持更多数据类型，避免类型转换问题。
            //自定义格式：某些类型可以通过重写 ToString() 方法提供自定义格式输出。
        }else if(fieldType==typeof(float)){
            Debug.Log("存储float"+keyname); 
            PlayerPrefs.SetFloat(keyname, (float)value);
        }else if(fieldType==typeof(bool)){
            Debug.Log("存储bool"+keyname); 
            PlayerPrefs.SetFloat(keyname, (bool)value ? 1 : 0);
        }//还有List Dic 
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keyname"></param>
    /// <returns></returns>
    public object LoadData(Type type , string keyname){
        //传进来的是个type 传出去的是个object
        //先用反射创造一个object
        object data = Activator.CreateInstance(type);//反射动态创建
        //创造出来这个对象之后 往这个要返回的对象里面填我们所需要取出的数据
        //想得到需要填哪些数据 就得得到数据所对应的字段 和对应的keyname
        FieldInfo[] infos = type.GetFields();
        string loadkeyname = "";
        FieldInfo info;
        for(int i = 0;i<infos.Length;i++){
            info = infos[i];
            loadkeyname = keyname+"_"+type.Name+"_"+info.FieldType+"_"+info.Name;//和存进去的规则保持一致
            //用反射往对应字段里填充 参数1:给谁填充 参数2:填充物
            info.SetValue(data, LoadValue(info.FieldType,loadkeyname));
        }
        return data;
    }

    /// <summary>
    /// 封装一个装数据方法
    /// </summary>
    /// <param name="fieldtype">字段类型 判断用哪个api</param>
    /// <param name="keyname"></param>
    /// <returns></returns>
    private object LoadValue(Type fieldtype, string keyname){

        if(fieldtype==typeof(int)){
          
            return PlayerPrefs.GetInt(keyname, 0);//值类型装到引用类型 装箱 存在性能消耗
        }else if(fieldtype==typeof(string)){
            
            return PlayerPrefs.GetString(keyname, "未输入");
            
        }else if(fieldtype==typeof(float)){
            
            return PlayerPrefs.GetFloat(keyname, 0);
        }else if(fieldtype==typeof(bool)){
            
            return PlayerPrefs.GetInt(keyname, 0)==1?true:false;
        }//还有List Dic 
        return null;
    }
}
