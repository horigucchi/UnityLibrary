using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーン変換装置
    /// </summary>
    [System.Serializable]
    public class SceneChanger
    {
        /// <summary>
        /// 規定のシーンの名前
        /// </summary>
        public const string DEFAULT_LOADING_SCENE_NAME = "LoadingScene";

        // シーン管理オブジェクト
        protected SceneManagerFacade _sceneManager = new SceneManagerFacade();

        private ISceneChangingRunner _changingRunner = null;


        /* public field */

        [field: SerializeField, Tooltip("ロードする際に挟むシーン名")]
        public string LoadingSceneName { get; protected set; } = DEFAULT_LOADING_SCENE_NAME;

        public bool IsLoading => _changingRunner != null;

        public float Progress => _changingRunner?.Progress ?? 0;


        /* constructor */

        public SceneChanger()
        {
            LoadingSceneName = DEFAULT_LOADING_SCENE_NAME;
        }

        public SceneChanger(string sceneName)
        {
            LoadingSceneName = sceneName;
        }


        /* public method */

        /// <summary>
        /// シーン遷移実行
        /// </summary>
        /// <param name="sceneChangingRunner"></param>
        /// <param name="preProcesser"></param>
        /// <param name="postProcesser"></param>
        /// <returns></returns>
        public IEnumerator RunToChangeScene(ISceneChangingRunner sceneChangingRunner, ISceneChangingPreProcesser preProcesser = null, ISceneChangingPostProcesser postProcesser = null)
        {
            // 動作中は中止
            if (IsLoading) yield break;
            _changingRunner = sceneChangingRunner;

            // 名前チェック
            //if (!IsExistSceneNameInBuildSettings(mainSceneName)) yield break;
            //if (!IsExistSceneNameInBuildSettings(subsidiarySceneNames)) yield break;

            // 存在シーン取得
            //Scene[] loadedScenes = _sceneManager.GetAllLoadedScenes();

            // 前処理（フェード等）
            yield return preProcesser?.RunPreProcessing();
            // ローディングシーン読込
            yield return new SceneCreatableAdditionalLoadingRunner(LoadingSceneName).RunSceneChanging();
            // ローディングシーンアクティブ化
            Scene loadingScene = _sceneManager.GetLoadedScene(LoadingSceneName);
            _sceneManager.SwitchMainScene(loadingScene);

            // それぞれの処理を実行
            yield return sceneChangingRunner?.RunSceneChanging();

            // ローディングシーンがアクティブのままの場合
            if (_sceneManager.GetMainSceneName() == LoadingSceneName)
            {
                // ローディングシーンを除く最初のシーンを取得
                Scene scene = _sceneManager.GetFirstInactiveLoadedScene(LoadingSceneName);
                // 上記取得シーンアクティブ化
                _sceneManager.SwitchMainScene(scene);
            }

            // ローディングシーン破棄
            yield return new SceneUnloadingRunner(loadingScene).RunSceneChanging();
            // 後処理（フェード等）
            yield return postProcesser?.RunPostProcessing();

            // 実行終了
            _changingRunner = null;
            yield return null;
        }
    }
}
