using BepInEx;

namespace Optimized_ {

	[BepInPlugin("teamgrad.optimized", "Optimized!", "1.0.1")]
	public class OPLauncher : BaseUnityPlugin {

		public OPLauncher() { OPBinder.UnitGlad(); }
	}
}
