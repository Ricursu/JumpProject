using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.LuaClient.luaState.DoFile("Login.lua");
        LuaManager.Instance.LuaClient.CallFunc("Login.Awake", this.gameObject);
        //LuaManager.Instance.LuaClient.CallFunc("Login.Awake2", "231321");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
