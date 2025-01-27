using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandlerBase : MonoBehaviour
{
    public abstract void ResolveReferences();
    public abstract void PerformActions();
    public abstract void ReInitialize();
}
