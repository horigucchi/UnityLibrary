using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力のトリガーの実現
    /// </summary>
    public interface IInputTriggerCore
    {
        bool IsPressed { get; }
        bool IsReleased { get; }
    }
}
