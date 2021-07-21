using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン変換実行器
    /// </summary>
    public interface ISceneChangingRunner
    {
        float Progress { get; }

        IEnumerator RunSceneChanging();
    }
}
