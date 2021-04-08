using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.LuaClient.luaState.DoFile("BoxController");
        LuaManager.Instance.LuaClient.CallFunc("BoxController.Awake",this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        LuaManager.Instance.LuaClient.CallFunc("BoxController.Update", this.gameObject);
    }

    //public bool IsInView(Vector3 worldPos)
    //{
    //    Transform camTransform = Camera.main.transform;
    //    Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
    //    Vector3 dir = (worldPos - camTransform.position).normalized;
    //    float dot = Vector3.Dot(camTransform.forward, dir);     //判断物体是否在相机前面


    //    if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
    //        return true;
    //    else
    //        return false;
    //}


}
