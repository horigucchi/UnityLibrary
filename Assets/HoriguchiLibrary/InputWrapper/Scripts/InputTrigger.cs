using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// トリガー
    /// </summary>
    public class InputTrigger
    {
        // クリックと判定される押してから離すまでの時間
        public const float CLICKABLE_TIME_INTERVAL = 0.2f;

        // 依存する入力のトリガー
        public readonly IInputTriggerCore Core;


        /* events */

        public event Action Pressed;
        public event Action Released;
        public event Action HoldingDown;
        public event Action Clicked;


        /* fields */

        public InputTriggerState State => status.State;
        public float StateTime => status.DurationTime;

        #region// Trigger Checker
        public bool IsPressed => status.State == InputTriggerState.Pressed;
        public bool IsReleased => (status.State & (InputTriggerState)0b011) == InputTriggerState.Released;
        public bool IsHoldingDown => status.State == InputTriggerState.HoldingDown;
        public bool IsClicked => status.State == InputTriggerState.Clicked;
        #endregion

        // 状態
        private readonly InputStatus<InputTriggerState> status;
        // ステートで分岐するための配列
        private readonly Action[] inputEventTable;


        /* methods */

        // コンストラクタ
        public InputTrigger(IInputTriggerCore core)
        {
            Core = core;
            status = new InputStatus<InputTriggerState>(getState);
            inputEventTable = new Action[]{
                null,
                () => { Released?.Invoke(); },
                () => { HoldingDown?.Invoke(); },
                () => { Pressed?.Invoke(); },
                null,
                () => { Clicked?.Invoke(); }
            };
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update() => inputEventTable[(int)status.Update().State]?.Invoke();  // 状態にあったイベントが存在していたら呼び出す


        // 状態取得
        private InputTriggerState getState()
        {
            if (Core.IsPressed) return InputTriggerState.Pressed;
            if (Core.IsReleased) return (IsHoldingDown && status.DurationTime < CLICKABLE_TIME_INTERVAL) ?
                    InputTriggerState.Clicked : InputTriggerState.Released;
            if (IsPressed || IsHoldingDown) return InputTriggerState.HoldingDown;
            return InputTriggerState.Idle;
        }
    }
}
