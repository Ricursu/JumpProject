using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class IPCompare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IPCheck()
    {
        string pattrn = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))";
        string ip = gameObject.GetComponent<Text>().text;
        if (Regex.IsMatch(ip, pattrn))
        {
            Debug.Log("匹配");
            WebUtils.IP = "http://" + ip + "/";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Update");
        }
        else
            Debug.Log("不匹配");
    }
}
