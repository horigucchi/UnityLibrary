using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン新規読込実行器
    /// </summary>
    public class SceneCreatingRunner : ISceneChangingRunner
    {
        public string SceneName { get; protected set; }
  
        public Scene LoadingScene { get; protected set; }

        public float Progress => LoadingScene.isLoaded ? 1 : 0;

        public SceneCreatingRunner(string sceneName)
        {
            SceneName = sceneName;
        }

        public IEnumerator RunSceneChanging()
        {
            LoadingScene = SceneManager.CreateScene(SceneName);
            yield return new WaitUntil(() => LoadingScene.isLoaded);
        }
    }

    /// <summary>
    /// シーン新規作成可能読込実行器
    /// </summary>
    public class SceneCreatableAdditionalLoadingRunner : ISceneChangingRunner
    {
        private SceneManagerFacade _sceneManager = new SceneManagerFacade();

        public string SceneName { get; protected set; }

        public ISceneChangingRunner ChangingRunner { get; protected set; }

        public float Progress => ChangingRunner?.Progress ?? 0;

        public SceneCreatableAdditionalLoadingRunner(string sceneName)
        {
            SceneName = sceneName;
        }

        public IEnumerator RunSceneChanging()
        {
            if (_sceneManager.IsExistInBuildIndex(SceneName))
            {
                ChangingRunner = new SceneAdditionalLoadingRunner(SceneName);
                yield return ChangingRunner.RunSceneChanging();
            }
            else
            {
                ChangingRunner = new SceneCreatingRunner(SceneName);
                yield return ChangingRunner.RunSceneChanging();
            }
        }
    }
}
