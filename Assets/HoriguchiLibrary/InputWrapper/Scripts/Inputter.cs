using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力管理機構
    /// </summary>
    public class Inputter : MonoBehaviour
    {
        #region// singleton
        private static Inputter instance;

        public static Inputter Instance => instance = instance ?? FindObjectOfType<Inputter>() ?? new GameObject(typeof(Inputter).Name).AddComponent<Inputter>();
        
        private bool checkInstance()
        {
            if (Instance != this)
            {
                Destroy(this);
                return false;
            }
            DontDestroyOnLoad(this);
            return true;
        }
        #endregion

        // 入力を感知するトリガーのリスト
        private List<InputTrigger> triggerList = new List<InputTrigger>();
        // 入力を感知するポインターのリスト
        private List<InputPointer> pointerList = new List<InputPointer>();


        /* public methods */

        /// <summary>
        /// トリガーを取得
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static InputTrigger GetInputTrigger<T>()
            where T : IInputTriggerCore, new()
            => Instance.triggerList.Find(item => item.Core is T) ?? getNewAddedTrigger<T>();

        /// <summary>
        /// ポインターを取得
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static InputPointer GetInputPointer<T>()
            where T : IInputPointerCore, new()
            => Instance.pointerList.Find(item => item.Core is T) ?? getNewAddedPointer<T>();

        /// <summary>
        /// 指定したトリガーの状態を取得
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static InputTriggerState GetTriggerState<T>()
            where T : IInputTriggerCore, new()
            => GetInputTrigger<T>().State;

        /// <summary>
        /// 指定したポインターの状態を取得
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static InputPointerState GetPointerState<T>()
            where T : IInputPointerCore, new()
            => GetInputPointer<T>().State;

        /// <summary>
        /// トリガーが押されたか
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static bool IsTriggerDown<T>()
            where T : IInputTriggerCore, new()
            => GetInputTrigger<T>().IsPressed;

        /// <summary>
        /// トリガー離されたか
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static bool IsTriggerUp<T>()
            where T : IInputTriggerCore, new()
            => GetInputTrigger<T>().IsReleased;

        /// <summary>
        /// トリガーが押されているか
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static bool IsTriggerHoldingDown<T>()
            where T : IInputTriggerCore, new()
            => GetInputTrigger<T>().IsHoldingDown;

        /// <summary>
        /// トリガーがクリックされたか
        /// </summary>
        /// <typeparam name="T">取得するトリガーの種類</typeparam>
        /// <returns></returns>
        public static bool IsTriggerClicked<T>()
            where T : IInputTriggerCore, new()
            => GetInputTrigger<T>().IsClicked;

        /// <summary>
        /// ポインターの座標を取得
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static Vector3 GetPoint<T>()
            where T : IInputPointerCore, new()
            => GetInputPointer<T>().Point;

        /// <summary>
        /// ポインターが移動しているか
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static bool IsPointerMoving<T>()
            where T : IInputPointerCore, new()
            => GetInputPointer<T>().IsMoving;

        /// <summary>
        /// ポインターが移動し始めたか
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static bool IsPointerStarted<T>()
            where T : IInputPointerCore, new()
            => GetInputPointer<T>().IsStarted;

        /// <summary>
        /// ポインターが移動し止まったか
        /// </summary>
        /// <typeparam name="T">取得するポインターの種類</typeparam>
        /// <returns></returns>
        public static bool IsPointerStopped<T>()
            where T : IInputPointerCore, new()
            => GetInputPointer<T>().IsStopped;


        /* private methods */

        // 新しくトリガーを作り取得
        private static InputTrigger getNewAddedTrigger<T>()
            where T : IInputTriggerCore, new()
        {
            var trigger = new InputTrigger(new T());
            Instance.triggerList.Add(trigger);
            return trigger;
        }

        // 新しくポインターを作り取得
        private static InputPointer getNewAddedPointer<T>()
            where T : IInputPointerCore, new()
        {
            var pointer = new InputPointer(new T());
            Instance.pointerList.Add(pointer);
            return pointer;
        }


        /* unity call back methods */

        private void Start()
        {
            checkInstance();
        }

        private void Update()
        {
            for (int i = 0; i < triggerList.Count; ++i) triggerList[i]?.Update();
            for (int i = 0; i < pointerList.Count; ++i) pointerList[i]?.Update();
        }
    }
}
