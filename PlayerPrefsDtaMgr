using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerInfo{
    public int age = 10;
    public float height = 185.5f;
    public string name = "陈子杰";
    public bool sex = false;
}
public class ppf_test : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo p = new PlayerInfo();
        //存进去 用单例模式调用
        PlayerPrefsDataMgr.Instance.SaveData(p,"player1");
        //读出来
        PlayerInfo p1 = PlayerPrefsDataMgr.Instance.LoadData(typeof(PlayerInfo),"player1")as PlayerInfo;//这里返回的是个object类型 as成playerinfo类型
        Debug.Log(""+p1.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
