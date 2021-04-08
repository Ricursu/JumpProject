using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class LoadingScripts : MonoBehaviour
{
    public GameObject objProcessBar;
    public Text  Loadingprogress;
    float i = 0;
    //设置时钟timer
    System.Timers.Timer timer = new System.Timers.Timer(100);
    //时钟每前进一秒i加一
    private void DoStuff(object sender, System.Timers.ElapsedEventArgs e)
    {
          if(i<100) i++;
    }
    // Start is called before the first frame update
    void Start()
    {
         //时钟前进事件与dostuff方法绑定
         timer.Elapsed += new System.Timers.ElapsedEventHandler(DoStuff);
         timer.AutoReset = true;
         timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
         //i每增加1，slider进度条前进一格
          objProcessBar.GetComponent<Slider>().value = i / 100;
          Loadingprogress.text = "Loading progress   "+i.ToString() + "%";
          if(i==100) timer.Stop();
    }
}
