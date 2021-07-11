using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horiguchi.SceneChanging.Sample
{
    public class SampleSceneChanging : MonoBehaviour
    {
        private static SampleSceneChanging s_instance;

        [SerializeField]
        private ChangingSceneData _dataA;
        [SerializeField]
        private ChangingSceneData _dataB;

        private void Awake()
        {
            if (s_instance) return;
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) SceneChangingBehaviour.Instance.SwitchScene(_dataA);
            if (Input.GetKeyDown(KeyCode.B)) SceneChangingBehaviour.Instance.SwapScene(_dataB);
            if (SceneChangingBehaviour.Instance.IsChanging) Debug.Log(SceneChangingBehaviour.Instance.Progress);
        }
    }
}
