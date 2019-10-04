using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
    public virtual void UpdateNormal() { }
    public virtual void UpdateFixed() { }
    public virtual void UpdateLate() { }

    protected virtual void Awake()
    {
        UpdateManager.AddBehaviour(this);
    }
}
