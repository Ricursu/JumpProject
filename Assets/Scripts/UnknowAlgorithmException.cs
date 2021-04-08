using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

public  class UnknowAlgorithmException : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Crc16Algorithm Algorithm { get; set; }

    public UnknownAlgorithmException()
    {
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
