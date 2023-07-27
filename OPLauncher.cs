using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using TGCore;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace Optimized
{
	[BepInPlugin("teamgrad.optimized", "Optimized!", "1.0.1")]
	public class OPLauncher : TGMod 
	{
		public override void LateLaunch()
		{
			new OPMain();
		}

		public override void AddSettings()
		{
			ConfigSinkBodiesEnabled = Config.Bind("Gameplay", "SinkBodiesEnabled", false, "Enables/disables all units sinking after death.");
			ConfigDisablePPEnabled = Config.Bind("Video", "DisablePPEnabled", false, "Enables/disables post processing.");
			
			Debug.Log(ConfigDisablePPEnabled.Value);
			
			var sinkBodies = TGAddons.CreateSetting(SettingsInstance.SettingsType.Options, "Sink bodies on death", "Enables/disables all units sinking after death.", "GAMEPLAY", 0f, ConfigSinkBodiesEnabled.Value ? 0 : 1, new[] { "Disable sinking", "Enable sinking" });
			sinkBodies.OnValueChanged += delegate(int value)
			{
				ConfigSinkBodiesEnabled.Value = value == 0;
				foreach (var ub in OPMain.unitsWithoutSink)
				{
					var sink = ub.GetComponent<SinkOnDeath>();
					if (sink) sink.enabled = !ConfigSinkBodiesEnabled.Value;
					else
					{
						sink = ub.AddComponent<SinkOnDeath>();
						sink.time = 3f;
						sink.moveMultiplier = 3f;
						sink.scale = true;
						sink.secUntilScale = 10f;
					}
				}
			};

			var disablePP = TGAddons.CreateSetting(SettingsInstance.SettingsType.Options, "Toggle post processing", "Enables/disables post processing.", "VIDEO", 0f, ConfigDisablePPEnabled.Value ? 0 : 1, new[] { "Disable post processing", "Enable post processing"});
			disablePP.OnValueChanged += delegate(int value)
			{
				ConfigDisablePPEnabled.Value = value == 0;
				foreach (var obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(x => x.GetComponentInChildren<PostProcessVolume>()))
				{
					if (obj) obj.GetComponentInChildren<PostProcessVolume>().enabled = !ConfigDisablePPEnabled.Value;
				}
				Debug.Log(ConfigDisablePPEnabled.Value);
			};
		}

		public override void SceneManager(Scene scene, LoadSceneMode loadSceneMode)
		{
			foreach (var obj in scene.GetRootGameObjects().Where(x => x.GetComponentInChildren<PostProcessVolume>()))
			{
				if (obj) obj.GetComponentInChildren<PostProcessVolume>().enabled = !OPMain.DisablePP;
			}
		}
		
		public static ConfigEntry<bool> ConfigSinkBodiesEnabled;
		public static ConfigEntry<bool> ConfigDisablePPEnabled;
	}
}
