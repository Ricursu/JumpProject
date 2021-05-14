using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HotUpdate.ChangeSlider(WebUtils.processSlider);
        HotUpdate.ChangeLoadingprogress(WebUtils.processText);
    }
}
