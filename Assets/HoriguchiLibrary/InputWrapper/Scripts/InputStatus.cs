using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力の状態
    /// </summary>
    /// <typeparam name="TState">状態（enum）</typeparam>
    public class InputStatus<TState> where TState : Enum
    {
        /* fields */

        /// <summary>
        /// 状態
        /// </summary>
        public TState State { get; private set; } = default;
        /// <summary>
        /// 持続時間
        /// </summary>
        public float DurationTime { get; private set; } = 0;

        // ステートの取得関数
        private Func<TState> stateGetter;


        /* methods */

        // コンストラクタ
        public InputStatus(Func<TState> stateGetter)
        {
            this.stateGetter = stateGetter;
            DurationTime = 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public InputStatus<TState> Update()
        {
            var state = stateGetter();                  // トリガーの状態取得
            if (!state.Equals(State)) DurationTime = 0; // 状態が変わっていたら経過時間をリセット
            State = state;                              // 状態反映
            DurationTime += Time.deltaTime;             // 状態の時間を加算
            return this;
        }
    }
}
