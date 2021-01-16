using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.InputWrapper.Sample
{
    public class SampleInputPointerCore : IInputPointerCore
    {
        public Vector3 Point { get => Input.mousePosition; }
    }
}
