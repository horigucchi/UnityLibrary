using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン切替実行器
    /// </summary>
    public class SceneSwitchingRunner : ISceneChangingRunner
    {
        protected ChangingSceneData _data;

        protected SceneManagerFacade _sceneManager = new SceneManagerFacade();

        protected MultipleSceneUnloadingRunner _unloadingRunner;
        protected MultipleSceneAdditionalLoadingRunner _loadingRunner;


        public float Progress => (_unloadingRunner?.Progress + _loadingRunner?.Progress) * 0.5f ?? 0;

        public SceneSwitchingRunner(ChangingSceneData data)
        {
            _data = data;
        }

        public IEnumerator RunSceneChanging()
        {
            // 破棄するシーンを取得
            Scene[] loadedScenes = _sceneManager.GetSubsidiaryScenes();

            _unloadingRunner = new MultipleSceneUnloadingRunner(loadedScenes);
            _loadingRunner = new MultipleSceneAdditionalLoadingRunner(_data.GetAllSceneNames());

            // 存在シーン破棄
            yield return _unloadingRunner.RunSceneChanging();
            // 目的シーン読込
            yield return _loadingRunner.RunSceneChanging();
            // 目的シーンアクティブ化
            _sceneManager.SwitchMainScene(_data.MainSceneName);
        }
    }
}
