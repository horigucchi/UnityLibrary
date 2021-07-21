using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン入替実行器
    /// </summary>
    public class SceneSwappingRunner : ISceneChangingRunner
    {
        protected ChangingSceneData _data;

        protected SceneManagerFacade _sceneManager = new SceneManagerFacade();

        protected MultipleSceneUnloadingRunner _unloadingRunner;
        protected MultipleSceneAdditionalLoadingRunner _loadingRunner;

        public float Progress => (_unloadingRunner?.Progress + _loadingRunner?.Progress) * 0.5f ?? 0;


        public SceneSwappingRunner(ChangingSceneData data)
        {
            _data = data;
        }

        public IEnumerator RunSceneChanging()
        {
            //string[] loadedSceneNames = _sceneManager.ConvertSceneNamesFromScene(currentScenes);
            Scene[] loadedScenes = _sceneManager.GetSubsidiaryScenes();
            // 差分シーン取得
            //string[] reducedSceneNames = _sceneManager.GetDifferenceSceneNames(loadedSceneNames, _data.SubsidiarySceneNames.Append(_data.MainSceneName).ToArray());
            string[] allSceneNames = _data.SubsidiarySceneNames.Append(_data.MainSceneName).ToArray();
            Scene[] reducedSceneNames = _sceneManager.GetDifferenceScenes(loadedScenes, allSceneNames);
            string[] additionalSceneNames = _sceneManager.GetDifferenceSceneNames(allSceneNames, _sceneManager.ConvertSceneNamesFromScene(loadedScenes));

            _unloadingRunner = new MultipleSceneUnloadingRunner(reducedSceneNames);
            _loadingRunner = new MultipleSceneAdditionalLoadingRunner(additionalSceneNames);

            // 存在シーン破棄
            yield return _unloadingRunner.RunSceneChanging();
            // 目的シーン読込
            yield return _loadingRunner.RunSceneChanging();
            // 目的シーンアクティブ化
            _sceneManager.SwitchMainScene(_data.MainSceneName);
        }
    }
}
