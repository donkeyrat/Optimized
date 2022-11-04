using Landfall.TABS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using HarmonyLib;

namespace Optimized_ {

    public class OPMain {

        public OPMain() {

            db = LandfallUnitDatabase.GetDatabase();
            foreach (var unit in db.UnitBaseList) {
                if (!unit.GetComponent<SinkOnDeath>()) { unitsWithoutSink.Add(unit); }
            }

            new GameObject() {
                name = "Bullshit: The Spinoff",
                hideFlags = HideFlags.HideAndDontSave
            }.AddComponent<OPLightingDisabler>();

            var sinkBodies = CreateSetting(SettingsInstance.SettingsType.Options, "Sink bodies on death", "Makes all units sink after death.", "GAMEPLAY", new string[] { "Disabled", "Enabled" });
            sinkBodies.OnValueChanged += SinkBodies_OnValueChanged;

            var disablePP = CreateSetting(SettingsInstance.SettingsType.Options, "Toggle post processing", "Enables/disables post processing.", "VIDEO", new string[] { "Enable post processing", "Disable post processing" });
            disablePP.OnValueChanged += DisablePP_OnValueChanged;
            
            var harmony = new Harmony("To any reading this code, I can See You.");
            harmony.PatchAll();
        }

        private void DisablePP_OnValueChanged(int value)
        {
            DisablePP = value;
            if (value == 0) { foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects()) { if (obj.GetComponent<PostProcessVolume>()) { obj.GetComponent<PostProcessVolume>().enabled = true; } } }
            else { foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects()) { if (obj.GetComponent<PostProcessVolume>()) { obj.GetComponent<PostProcessVolume>().enabled = false; } } }
        }

        private void SinkBodies_OnValueChanged(int value)
        {
            if (value == 0) {

                foreach (var ub in unitsWithoutSink) {

                    if (ub.GetComponent<SinkOnDeath>()) { ub.GetComponent<SinkOnDeath>().enabled = false; }
                }
            }
            else {
                foreach (var ub in unitsWithoutSink) {

                    if (ub.GetComponent<SinkOnDeath>() && !ub.transform.root.GetComponentInChildren<HealthHandler>().willBeRewived) { ub.GetComponent<SinkOnDeath>().enabled = true; }
                    else if (!ub.GetComponent<SinkOnDeath>() && !ub.transform.root.GetComponentInChildren<HealthHandler>().willBeRewived) {
                        var sink = ub.AddComponent<SinkOnDeath>();
                        sink.time = 3f;
                        sink.moveMultiplier = 3f;
                        sink.scale = true;
                        sink.secUntilScale = 10f;
                    }
                }
            }
        }

        public SettingsInstance CreateSetting(SettingsInstance.SettingsType settingsType, string settingName, string toolTip, string settingListToAddTo, string[] options = null, float min = 0f, float max = 1f) {

            var setting = new SettingsInstance();

            setting.settingName = settingName;
            setting.toolTip = toolTip;
            setting.m_settingsKey = settingName;

            setting.settingsType = settingsType;
            setting.options = options;
            setting.min = min;
            setting.max = max;

            var global = ServiceLocator.GetService<GlobalSettingsHandler>();
            SettingsInstance[] listToAdd;
            if (settingListToAddTo == "BUG") { listToAdd = global.BugsSettings; }
            else if (settingListToAddTo == "VIDEO") { listToAdd = global.VideoSettings; }
            else if (settingListToAddTo == "AUDIO") { listToAdd = global.AudioSettings; }
            else if (settingListToAddTo == "CONTROLS") { listToAdd = global.ControlSettings; }
            else { listToAdd = global.GameplaySettings; }

            var list = listToAdd.ToList();
            list.Add(setting);

            if (settingListToAddTo == "BUG") { typeof(GlobalSettingsHandler).GetField("m_bugsSettings", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(global, list.ToArray()); }
            else if (settingListToAddTo == "VIDEO") { typeof(GlobalSettingsHandler).GetField("m_videoSettings", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(global, list.ToArray()); }
            else if (settingListToAddTo == "AUDIO") { typeof(GlobalSettingsHandler).GetField("m_audioSettings", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(global, list.ToArray()); }
            else if (settingListToAddTo == "CONTROLS") { typeof(GlobalSettingsHandler).GetField("m_controlSettings", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(global, list.ToArray()); }
            else { typeof(GlobalSettingsHandler).GetField("m_gameplaySettings", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(global, list.ToArray()); }

            return setting;
        }

        public LandfallUnitDatabase db;

        public List<GameObject> unitsWithoutSink = new List<GameObject>();

        public static int DisablePP = 0;
    }
}
