using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinMax<T> where T : IComparable {
    private bool isSet;
    private T min;
    private T max;

    public void Set(T value)
    {
        if (!isSet){
            this.min = value;
            this.max = value;
            isSet = true;
        }
        if(value.CompareTo(min) < 0)
        {
            this.min = value;
        }
        else if (value.CompareTo(max) > 0)
        {
            this.max = value;
        }

    }

    public T GetMin()
    {
        return this.min;
    }

    public T GetMax()
    {
        return this.max;
    }
}