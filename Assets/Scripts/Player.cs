using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.LuaClient.luaState.DoFile("Player.lua");
        LuaManager.Instance.LuaClient.CallFunc("Player.Awake", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        LuaManager.Instance.LuaClient.CallFunc("Player.Update", this.gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        LuaManager.Instance.LuaClient.CallFunc("Player.OnCollisionStay", collision.gameObject);
    }
}
