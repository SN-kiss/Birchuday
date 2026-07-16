using System;
using UnityEngine;

//GOD
//キスしたでって通知用

public class GOD_KissRelay : MonoBehaviour
{
    public static event Action OnKiss;

    public void NotifyKiss()
    {
        OnKiss?.Invoke();
    }
}