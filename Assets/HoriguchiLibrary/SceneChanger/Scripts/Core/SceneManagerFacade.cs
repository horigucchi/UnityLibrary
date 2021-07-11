using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Horiguchi.SceneChanging
{
    /// <summary>
    /// シーンマネージャ受付
    /// </summary>
    public class SceneManagerFacade : SceneManager
    {
        #region// flag

        /// <summary>
        /// 読込済みのシーン名の存在
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public bool IsExistInLoadedScene(string sceneName)
            => GetLoadedScene(sceneName).IsValid();

        /// <summary>
        /// ビルドインデックス内のシーン名の存在
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public bool IsExistInBuildIndex(string sceneName)
        {
            int length = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < length; i++)
            {
                if (SceneManager.GetSceneByBuildIndex(i).name == sceneName) return true;
            }
            return false;
        }

        /// <summary>
        /// ビルドインデックス内のシーン名の存在
        /// </summary>
        /// <param name="sceneNames"></param>
        /// <returns></returns>
        public bool IsExistInBuildIndex(params string[] sceneNames)
        {
            string[] sceneNamesInBuildIndex = GetSceneNamesInBuildIndex();
            foreach (var sceneName in sceneNames)
            {
                if (!sceneNamesInBuildIndex.Any(name => name == sceneName)) return false;
            }
            return true;
        }

        #endregion

        #region// getter

        /// <summary>
        /// ビルドインデックス内のシーン名を取得
        /// </summary>
        /// <returns></returns>
        public Scene[] GetScenesInBuildIndex()
        {
            var scenes = new Scene[SceneManager.sceneCountInBuildSettings];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = SceneManager.GetSceneByBuildIndex(i);
            }
            return scenes;
        }

        /// <summary>
        /// ビルドインデックス内のシーン名を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetSceneNamesInBuildIndex()
            => ConvertSceneNamesFromScene(GetScenesInBuildIndex());

        /// <summary>
        /// 読込済みのシーンを名前で取得
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public Scene GetLoadedScene(string sceneName)
            => SceneManager.GetSceneByName(sceneName);

        /// <summary>
        /// 読込済みのシーンを全部取得
        /// </summary>
        /// <returns></returns>
        public Scene[] GetAllLoadedScenes()
        {
            int count = SceneManager.sceneCount;
            var result = new Scene[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = SceneManager.GetSceneAt(i);
            }
            return result;
        }

        /// <summary>
        /// 読込済みのシーン名を全部取得
        /// </summary>
        /// <returns></returns>
        public string[] GetAllLoadedSceneNames()
            => ConvertSceneNamesFromScene(GetAllLoadedScenes());

        /// <summary>
        /// 読込済みアクティブなシーンを取得
        /// </summary>
        /// <returns></returns>
        public Scene GetMainScene()
            => SceneManager.GetActiveScene();

        /// <summary>
        /// 読込済みアクティブなシーン名を取得
        /// </summary>
        /// <returns></returns>
        public string GetMainSceneName()
            => GetMainScene().name;

        /// <summary>
        /// 読込済みの補助シーンを取得
        /// </summary>
        /// <returns></returns>
        public Scene[] GetSubsidiaryScenes()
        {
            Scene[] scenes = GetAllLoadedScenes();
            string mainSceneName = GetMainSceneName();
            var subsidaryScenes = from scene in scenes where scene.name != mainSceneName select scene;
            return subsidaryScenes.ToArray();
        }

        /// <summary>
        /// 読込済みの補助シーン名を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetSubsidiarySceneNames()
            => ConvertSceneNamesFromScene(GetSubsidiaryScenes());

        /// <summary>
        /// 読込済みから差分のシーン名を取得
        /// </summary>
        /// <param name="sceneNames"></param>
        /// <returns></returns>
        public Scene[] GetLoadedDifferenceScenes(params string[] sceneNames)
        {
            string[] names = GetLoadedDifferenceSceneNames(sceneNames);
            var scenes = from name in names select GetLoadedScene(name);
            return scenes.ToArray();
        }

        /// <summary>
        /// 読込済みから差分のシーン名を取得
        /// </summary>
        /// <param name="sceneNames"></param>
        /// <returns></returns>
        public string[] GetLoadedDifferenceSceneNames(params string[] sceneNames)
            => GetDifferenceSceneNames(GetSceneNamesInBuildIndex(), sceneNames);

        /// <summary>
        /// 差分のシーン名を取得
        /// </summary>
        /// <param name="universeSceneNames"></param>
        /// <param name="sceneNames"></param>
        /// <returns></returns>
        public string[] GetDifferenceSceneNames(string[] universeSceneNames, params string[] sceneNames)
            => universeSceneNames.Except(sceneNames).ToArray();

        /// <summary>
        /// 差分のシーン名を取得
        /// </summary>
        /// <param name="universeScenes"></param>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public Scene[] GetDifferenceScenes(Scene[] universeScenes, params Scene[] scenes)
            => universeScenes.Except(scenes).ToArray();

        /// <summary>
        /// 差分のシーン名を取得
        /// </summary>
        /// <param name="universeScenes"></param>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public Scene[] GetDifferenceScenes(Scene[] universeScenes, params string[] scenes)
        {
            return (from scene in universeScenes
                    where !scenes.Contains(scene.name)
                    select scene).ToArray();
        }


        /// <summary>
        /// 読込済みの非アクティブなシーンの一つ目を取得
        /// </summary>
        /// <param name="exclusionSceneNames"></param>
        /// <returns></returns>
        public Scene GetFirstInactiveLoadedScene(params string[] exclusionSceneNames)
            => GetAllLoadedScenes().First(scene => !exclusionSceneNames.Contains(scene.name) && scene.isLoaded);

        /// <summary>
        /// 読込済みの非アクティブなシーンの一つ目を取得
        /// </summary>
        /// <param name="exclusionScenes"></param>
        /// <returns></returns>
        public Scene GetFirstInactiveLoadedScene(params Scene[] exclusionScenes)
            => GetFirstInactiveLoadedScene(ConvertSceneNamesFromScene(exclusionScenes));


        #endregion

        #region// converter

        /// <summary>
        /// シーン配列をシーン名の配列に変換
        /// </summary>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public string[] ConvertSceneNamesFromScene(Scene[] scenes)
            => (from scene in scenes select scene.name).ToArray();

        /// <summary>
        /// シーン名の配列をシーン配列に変換
        /// </summary>
        /// <param name="sceneNames"></param>
        /// <returns></returns>
        public Scene[] ConvertSceneFromNames(string[] sceneNames)
            => (from name in sceneNames select SceneManager.GetSceneByName(name)).ToArray();

        #endregion

        #region// active switcher

        /// <summary>
        /// アクティブシーンの切り替え
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public bool SwitchMainScene(Scene scene)
            => SceneManager.SetActiveScene(scene);

        /// <summary>
        /// アクティブシーンの切り替え
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public bool SwitchMainScene(string sceneName)
            => SwitchMainScene(GetLoadedScene(sceneName));

        #endregion

    }
}
