using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Crc16Algorithm
    {
        /// <summary>
        /// 定义要使用哪种CRC16算法的枚举
        /// </summary>

        /// <summary>
        /// 使用初始CRC值为0的x^16+x^15+x^2+1多项式执行CRC 16
        /// </summary>
        Standard,

        /// <summary>
        /// 使用x^16+x^15+x^2+1多项式的CRC 16 CCITT实用程序，初始CRC值为0
        /// </summary>
        Ccitt,

        /// <summary>
        /// 使用初始CRC值为0的反向x^16+x^15+x^2+1多项式执行CRC 16 CCITT
        /// </summary>
        CcittKermit,

        /// <summary>
        /// 使用初始CRC值为0xffff的x^16+x^15+x^2+1多项式执行CRC 16 CCITT
        /// </summary>
        CcittInitialValue0xFFFF,

        /// <summary>
        /// 使用初始CRC值为0x1D0F的x^16+x^15+x^2+1多项式执行CRC 16 CCITT
        /// </summary>
        CcittInitialValue0x1D0F,

        /// <summary>
        /// 使用初始CRC值为0的反向x^16+x^13+x^12+x^11+x^10+x^8+x^6+x^5+x^2+1（0xA6BC）执行CRC 16分布式网络协议（DNP）
        /// </summary>
        Dnp
    }
}
