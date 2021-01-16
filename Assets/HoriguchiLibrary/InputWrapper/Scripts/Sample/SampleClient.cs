using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.InputWrapper.Sample
{
    public class SampleClient : MonoBehaviour
    {
        private void Start()
        {
            Inputter.GetInputTrigger<SampleInputTriggerCore>().Pressed += () => { Debug.Log("TriggerPressEvent"); };
            Inputter.GetInputTrigger<SampleInputTriggerCore>().Released += () => { Debug.Log("TriggerReleaseEvent"); };
            Inputter.GetInputTrigger<SampleInputTriggerCore>().Clicked += () => { Debug.Log("TriggerClickEvent"); };
            Inputter.GetInputPointer<SampleInputPointerCore>().Started += () => { Debug.Log("PointerStartEvent"); };
            Inputter.GetInputPointer<SampleInputPointerCore>().Stopped += () => { Debug.Log("PointerStopEvent"); };
        }
        private void Update()
        {
            if (Inputter.GetTriggerState<SampleInputTriggerCore>() == InputTriggerState.Pressed)
            {
                Debug.Log("GetStateIsPressed is true");
            }
            if (Inputter.IsTriggerDown<SampleInputTriggerCore>())
            {
                Debug.Log("TriggerDown is true");
            }
            if (Inputter.IsTriggerUp<SampleInputTriggerCore>())
            {
                Debug.Log("TriggerUp is true");
            }
            if (Inputter.IsTriggerHoldingDown<SampleInputTriggerCore>())
            {
                Debug.Log("TriggerHoldingDown is true");
            }

            if (Inputter.IsPointerMoving<SampleInputPointerCore>())
            {
                Debug.Log("PointerMoving is true");
            }
        }
    }
}