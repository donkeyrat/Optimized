using System.Collections;
using UnityEngine;

namespace Optimized_ {

    public class OPBinder : MonoBehaviour {

        public static void UnitGlad() {

            if (!instance) {

                instance = new GameObject {
                    hideFlags = HideFlags.HideAndDontSave
                }.AddComponent<OPBinder>();
            }
            instance.StartCoroutine(StartUnitgradLate());
        }

        private static IEnumerator StartUnitgradLate() {

            yield return new WaitUntil(() => FindObjectOfType<ServiceLocator>() != null);
            yield return new WaitUntil(() => ServiceLocator.GetService<ISaveLoaderService>() != null);
            new OPMain();
            yield break;
        }

        private static OPBinder instance;
    }
}