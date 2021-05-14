using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LuaManager : MonoBehaviour
{
    //private LuaState luastate;
    private static LuaManager instance;
    public static LuaManager Instance
    {
        get { return instance; }
    }
    //LuaClient可以理解成是ToLua内部对自己的一种封装，可以视为tolua环境的一个启动
    private LuaClient luaClient;
    public LuaClient LuaClient
    {
        get { return luaClient; }
    }
    void Start()
    {
        instance = this;
        //luastate = new LuaState();
        DontDestroyOnLoad(this.gameObject);
        //需要将LuaClient中的protected LuaState luaState = null;改为public，
        //同时可以在LuaClient中再封装一个调用Lua模块函数的方法。
        luaClient = this.gameObject.AddComponent<LuaClient>();
        LuaManager.Instance.LuaClient.luaState.DoFile("HotUpdate.lua");
        LuaManager.Instance.LuaClient.CallFunc("HotUpdate.Awake", this.gameObject);
        //重启Lua虚拟机
        //gameObject.GetComponent<LuaClient>().Destroy();
        //gameObject.GetComponent<LuaClient>().Init();
    }

    private void Update()
    {
        LuaManager.Instance.LuaClient.CallFunc("HotUpdate.Update", this.gameObject);
    }

    IEnumerator LuaHotUpdate()
    {
        yield return 0;
        //yield return WebUtils.GetFileFromServer("v1.0-v1.1.zip");
        LuaManager.Instance.LuaClient.luaState.DoFile("HotUpdate.lua");
        LuaManager.Instance.LuaClient.CallFunc("HotUpdate.Awake", this.gameObject);
        //LuaManager.Instance.LuaClient.luaState.DoFile("Login.lua");
        //LuaManager.Instance.LuaClient.CallFunc("Login.Awake", GameObject.Find("Canvas"));
        yield return new WaitForSeconds(5.0f);
        //Debug.LogError(1);
    }
}
