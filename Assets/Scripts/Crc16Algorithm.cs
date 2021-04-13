using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Crc16Algorithm
{
    Standard,
    Ccitt,
    CcittKermit,
    CcittInitialValue0xFFFF,
    CcittInitialValue0x1D0F,
    Dnp
}
