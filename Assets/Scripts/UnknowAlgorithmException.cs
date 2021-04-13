using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable]
internal class UnknowAlgorithmException : Exception
{
    public Crc16Algorithm Algorithm { get; set; }

    public UnknownAlgorithmException(){
    }

    public UnknownAlgorithmException(Crc16Algorithm algorithm)
    {
        this.Algorithm = algorithm;
    }

    public UnknownAlgorithmException(string message) : base(message)
    {
    }

    public UnknownAlgorithmException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected UnknownAlgorithmException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
