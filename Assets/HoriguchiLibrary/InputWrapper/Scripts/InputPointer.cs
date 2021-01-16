using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// ポインター
    /// </summary>
    public class InputPointer
    {
        // 動かし始めたと判定される距離
        public const float MOVEMENT_START_DISTANCE = 0.01f;

        // 依存する入力のポインター
        public readonly IInputPointerCore Core;


        /* events */

        public event Action Started;
        public event Action Stopped;
        public event Action Moving;


        /* fields */

        public InputPointerState State => status.State;
        public float StateTime => status.DurationTime;

        #region// Trigger Checker            
        public bool IsStarted => status.State == InputPointerState.Started;
        public bool IsStopped => status.State == InputPointerState.Stopped;
        public bool IsMoving => status.State == InputPointerState.Moving;
        #endregion

        /// <summary>
        /// 入力されている座標
        /// </summary>
        public Vector3 Point { get; private set; } = Vector3.zero;

        // 状態
        private readonly InputStatus<InputPointerState> status;
        // ステートで分岐するための配列
        private readonly Action[] inputEventTable;

        // ポインターの速度の長さ
        private float velocityLength = 0;


        /* methods */

        // コンストラクタ
        public InputPointer(IInputPointerCore core)
        {
            Core = core;
            status = new InputStatus<InputPointerState>(getState);
            inputEventTable = new Action[]{
                null,
                () => { Stopped?.Invoke(); },
                () => { Moving?.Invoke(); },
                () => { Started?.Invoke(); }
            };
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update() => inputEventTable[(int)status.Update().State]?.Invoke();  // 状態にあったイベントが存在していたら呼び出す


        // 状態取得
        private InputPointerState getState()
        {
            var point = Point;
            Point = Core.Point;
            var isMovingNow = velocityLength + (velocityLength = (Point - point).sqrMagnitude) > MOVEMENT_START_DISTANCE;
            if (!isMovingNow) return (IsMoving) ? InputPointerState.Stopped : InputPointerState.Idle;
            if (IsMoving) return InputPointerState.Moving;
            return IsStarted ? InputPointerState.Moving : InputPointerState.Started;
        }
    }
}
