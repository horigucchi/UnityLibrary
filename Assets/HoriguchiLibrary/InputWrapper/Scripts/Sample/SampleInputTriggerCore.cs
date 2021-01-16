using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.InputWrapper.Sample
{
    public class SampleInputTriggerCore : IInputTriggerCore
    {
        private const KeyCode KEY_CODE = KeyCode.Space;
        public bool IsPressed { get => Input.GetKeyDown(KEY_CODE); }
        public bool IsReleased { get => Input.GetKeyUp(KEY_CODE); }
    }
}
