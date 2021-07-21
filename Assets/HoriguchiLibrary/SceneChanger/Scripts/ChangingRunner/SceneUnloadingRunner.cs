using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン破棄実行器
    /// </summary>
    public class SceneUnloadingRunner : ISceneChangingRunner
    {
        private SceneManagerFacade _sceneManager = new SceneManagerFacade();
        public Scene Scene { get; protected set; }

        public AsyncOperation SceneLoadingOperation { get; protected set; }

        public float Progress => SceneLoadingOperation?.progress ?? 0;

        public SceneUnloadingRunner(Scene scene)
        {
            Scene = scene;
        }

        public SceneUnloadingRunner(string sceneName)
        {
            Scene = _sceneManager.GetLoadedScene(sceneName);
        }

        public IEnumerator RunSceneChanging()
        {
            yield return SceneLoadingOperation = SceneManager.UnloadSceneAsync(Scene);
        }
    }

    /// <summary>
    /// 複数シーン破棄実行器
    /// </summary>
    public class MultipleSceneUnloadingRunner : ISceneChangingRunner
    {
        private SceneManagerFacade _sceneManager = new SceneManagerFacade();
        public Scene[] Scenes { get; protected set; }

        public AsyncOperation[] SceneLoadingOperations { get; protected set; }

        public float Progress
        {
            get
            {
                if (SceneLoadingOperations?.Length <= 0) return 1;
                return SceneLoadingOperations?.Average(operation => operation?.progress ?? 0) ?? 0;
            }
        }


        public MultipleSceneUnloadingRunner(params Scene[] scenes)
        {
            Scenes = scenes;
        }

        public MultipleSceneUnloadingRunner(params string[] sceneNames)
        {
            Scenes = _sceneManager.ConvertSceneFromNames(sceneNames);
        }

        public IEnumerator RunSceneChanging()
        {
            var operationList = new List<AsyncOperation>();
            foreach (var scene in Scenes)
            {
                operationList.Add(SceneManager.UnloadSceneAsync(scene));
            }

            SceneLoadingOperations = operationList.ToArray();
            foreach (var operation in operationList)
            {
                yield return operation;
            }
        }
    }
}