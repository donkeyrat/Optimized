using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

namespace Optimized_ {

    public class OPLightingDisabler : MonoBehaviour {

        void Awake() {

            SceneManager.sceneLoaded += DisablePostProcessing;
        }

        void DisablePostProcessing(Scene scene, LoadSceneMode loadSceneMode) {

            if (OPMain.DisablePP == 1) {
                foreach (var obj in scene.GetRootGameObjects()) { if (obj.GetComponent<PostProcessVolume>()) { obj.GetComponent<PostProcessVolume>().enabled = false; } }
            }
        }
    }
}