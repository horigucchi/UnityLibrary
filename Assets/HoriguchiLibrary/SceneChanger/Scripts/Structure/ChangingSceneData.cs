using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// 変換するシーン名のデータ
    /// </summary>
    [System.Serializable]
    public struct ChangingSceneData
    {

        /* public field */

        [field: SerializeField, Tooltip("アクティブで読み込むシーン")]
        public string MainSceneName { get; private set; }
        [field: SerializeField, Tooltip("一緒に読み込むシーン")]
        public string[] SubsidiarySceneNames { get; private set; }

        /// <summary>
        /// 一緒に読み込むシーンの数
        /// </summary>
        public int SubsidiarySceneCount { get; private set; }


        /* constructor */

        public ChangingSceneData(string mainSceneName) : this(mainSceneName, null) { }

        public ChangingSceneData(string mainSceneName, params string[] subSceneNames)
        {
            MainSceneName = mainSceneName;
            SubsidiarySceneNames = subSceneNames;
            SubsidiarySceneCount = subSceneNames?.Length ?? 0;
        }


        /* public method */

        /// <summary>
        /// 全部のシーンの名前を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetAllSceneNames()
            => SubsidiarySceneNames.Append(MainSceneName).ToArray();

        /// <summary>
        /// 全部のシーンの名前を取得
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string[] GetAllSceneNames(ChangingSceneData data)
            => data.GetAllSceneNames();
    }
}
