/*
 * 2021/07/11
 * Y.Horiguchi
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン変換動作
    /// </summary>
    public class SceneChangingBehaviour : MonoBehaviour
    {
        #region// singleton

        private static SceneChangingBehaviour s_instance;

        /// <summary>
        /// シングルトンされたインスタンス
        /// </summary>
        public static SceneChangingBehaviour Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = FindObjectOfType<SceneChangingBehaviour>();
                    if (!s_instance)
                    {
                        // Error: アタッチされているGameObjectはありません。
                        var obj = new GameObject("SceneSwitcher");
                        s_instance = obj.AddComponent<SceneChangingBehaviour>();
                        Debug.LogWarningFormat("{0} is not found.\n So an instance was created.", typeof(SceneChangingBehaviour).ToString());
                    }
                }
                DontDestroyOnLoad(s_instance);
                return s_instance;
            }
        }

        // インスタンスの正規化
        private bool normalizeInstance()
        {
            if (Instance == this) { return true; }
            else
            {
                Destroy(this);
            }
            return false;
        }

        #endregion


        /* private field */

        [SerializeField, Tooltip("シーン変換器")]
        private SceneChanger _changer = new SceneChanger();


        /* public field */

        /// <summary>
        /// シーンを変換中
        /// </summary>
        public bool IsChanging => _changer?.IsLoading ?? false;

        /// <summary>
        /// 変更の進行状況
        /// </summary>
        public float Progress => _changer?.Progress ?? 0;


        /* public method */

        /// <summary>
        /// 任意の方法でシーンを替える
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="preProcesser"></param>
        /// <param name="postProcesser"></param>
        public void ChangeScene(ISceneChangingRunner runner, ISceneChangingPreProcesser preProcesser = null, ISceneChangingPostProcesser postProcesser = null)
        {
            StartCoroutine(_changer.RunToChangeScene(runner, preProcesser, postProcesser));
        }

        /// <summary>
        /// シーンを切り替える
        /// </summary>
        /// <param name="data"></param>
        /// <param name="preProcess"></param>
        /// <param name="postProcess"></param>
        public void SwitchScene(ChangingSceneData data, ISceneChangingPreProcesser preProcess = null, ISceneChangingPostProcesser postProcess = null)
        {
            var runner = new SceneSwitchingRunner(data);
            ChangeScene(runner, preProcess, postProcess);
        }

        /// <summary>
        /// シーンを入れ替える
        /// </summary>
        /// <param name="data"></param>
        /// <param name="preProcess"></param>
        /// <param name="postProcess"></param>
        public void SwapScene(ChangingSceneData data, ISceneChangingPreProcesser preProcess = null, ISceneChangingPostProcesser postProcess = null)
        {
            var runner = new SceneSwappingRunner(data);
            ChangeScene(runner, preProcess, postProcess);
        }


        /* private/protected method */

        /* unity callback */

        protected virtual void Awake()
        {
            if (_changer == null) _changer = new SceneChanger();
        }
    }
}
