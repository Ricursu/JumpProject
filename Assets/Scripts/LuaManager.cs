using LuaInterface;
using System.Collections;
using System.Collections.Generic;
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
    }

}
