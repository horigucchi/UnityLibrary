using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力のトリガーの実現
    /// </summary>
    public interface IInputPointerCore
    {
        Vector3 Point { get; }
    }
}
