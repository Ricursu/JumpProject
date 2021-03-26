using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;

public class Login : MonoBehaviour
{
    public LuaState luaState;

    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.LuaClient.luaState.DoFile("Login.lua");
        LuaManager.Instance.LuaClient.CallFunc("Login.Awake", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
