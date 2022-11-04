using System.Collections;
using HarmonyLib;

namespace Optimized_ {
    
    [HarmonyPatch(typeof(SinkOnDeath), "Sink")]
    class ReviveFixer {
        
        [HarmonyPrefix]
        public static bool Prefix(SinkOnDeath __instance, ref bool ___done, ref DataHandler ___data) {
            if (___done || __instance.transform.root.GetComponentInChildren<HealthHandler>().willBeRewived)
            {
                return false;
            }
            __instance.SetField("data", __instance.GetComponentInChildren<DataHandler>());
            __instance.SetField("done", true);
            __instance.StartCoroutine((IEnumerator)__instance.InvokeMethod("DoSink"));
            return false;
        }
    }
}