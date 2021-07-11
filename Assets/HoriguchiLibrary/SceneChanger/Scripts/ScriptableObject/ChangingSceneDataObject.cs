using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// 変換するシーン名のデータのオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "ChangingSceneData000", menuName = "Ex/ScriptableObject/ChangingSceneData")]
    public class ChangingSceneDataObject : ScriptableObject
    {
        [field: SerializeField, Tooltip("データ")]
        public ChangingSceneData Data { get; private set; }
    }
}
