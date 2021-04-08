using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.LuaClient.luaState.DoFile("TestPlayer.lua");
        LuaManager.Instance.LuaClient.CallFunc("TestPlayer.Start", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        LuaManager.Instance.LuaClient.CallFunc("TestPlayer.Update", this.gameObject);
    }
}
