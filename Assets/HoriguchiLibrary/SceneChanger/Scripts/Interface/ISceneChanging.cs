using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン変換前処理
    /// </summary>
    public interface ISceneChangingPreProcesser
    {
        IEnumerator RunPreProcessing();
    }

    /// <summary>
    /// シーン変換後処理
    /// </summary>
    public interface ISceneChangingPostProcesser
    {
        IEnumerator RunPostProcessing();
    }

    /// <summary>
    /// シーン変換前後処理
    /// </summary>
    public interface ISceneChangingPrePostProcesser
        : ISceneChangingPreProcesser
        , ISceneChangingPostProcesser
    {
    }
}
