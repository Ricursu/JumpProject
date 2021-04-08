using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour
{
    public static void AddButtonOnClick(GameObject obj, LuaFunction func)
    {
        if (obj == null)
        {
            return;
        }
        Button btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            func.Call(obj);
        });

    }

}
