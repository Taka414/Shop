//
// (C) 2022 Takap.
//

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Takap.Utility
{
    public abstract class T2Event<T1, T2> : UnityEvent<(T1, T2)> { }

    [Serializable] public class Vector2Event : UnityEvent<Vector2> { }
    [Serializable] public class Vector3Event : UnityEvent<Vector3> { }

    [Serializable] public class MyMultiEvent : T2Event<Vector3, Color> { }

    // 基本型
    [Serializable] public class UnityEventString : UnityEvent<string> { }
    [Serializable] public class UnityEventInt : UnityEvent<int> { }
    [Serializable] public class UnityEventFloat : UnityEvent<float> { }
    [Serializable] public class UnityEventBool : UnityEvent<bool> { }
}
