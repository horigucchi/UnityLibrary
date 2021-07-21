using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン追加読込実行器
    /// </summary>
    public class SceneAdditionalLoadingRunner : ISceneChangingRunner
    {
        public string SceneName { get; protected set; }


        public AsyncOperation SceneLoadingOperation { get; protected set; }

        public float Progress => SceneLoadingOperation?.progress ?? 0;

        public SceneAdditionalLoadingRunner(string sceneName)
        {
            SceneName = sceneName;
        }

        public IEnumerator RunSceneChanging()
        {
            yield return SceneLoadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// 複数シーン追加読込実行器
    /// </summary>
    public class MultipleSceneAdditionalLoadingRunner : ISceneChangingRunner
    {
        public string[] SceneNames { get; private set; }

        public AsyncOperation[] SceneLoadingOperations { get; protected set; }

        public float Progress
        {
            get
            {
                if (SceneLoadingOperations?.Length <= 0) return 1;
                return SceneLoadingOperations?.Average(operation => operation?.progress ?? 0) ?? 0;
            }
        }


        public MultipleSceneAdditionalLoadingRunner(params string[] sceneNames)
        {
            SceneNames = sceneNames;
        }

        public IEnumerator RunSceneChanging()
        {
            List<AsyncOperation> operationList = new List<AsyncOperation>();
            foreach (var sceneName in SceneNames)
            {
                operationList.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
            }
            SceneLoadingOperations = operationList.ToArray();
            foreach (var operation in operationList)
            {
                yield return operation;
            }
        }
    }
}