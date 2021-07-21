using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン再読込実行器
    /// </summary>
    public class SceneReloadingRunner : ISceneChangingRunner
    {
        private SceneManagerFacade _sceneManager = new SceneManagerFacade();

        private MultipleSceneUnloadingRunner _unloadingRunner;
        private MultipleSceneAdditionalLoadingRunner _loadingRunner;


        public float Progress => (_unloadingRunner.Progress + _loadingRunner.Progress) * 0.5f;


        public SceneReloadingRunner(params string[] subsidiarySceneNames)
        {
            _unloadingRunner = new MultipleSceneUnloadingRunner(subsidiarySceneNames);
            _loadingRunner = new MultipleSceneAdditionalLoadingRunner(subsidiarySceneNames);
        }

        public IEnumerator RunSceneChanging()
        {
            string[] subsidiarySceneNames = _sceneManager.GetSubsidiarySceneNames();
            yield return _unloadingRunner.RunSceneChanging();
            yield return _loadingRunner.RunSceneChanging();
        }
    }
}