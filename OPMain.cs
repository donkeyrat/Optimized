using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using DM;
using Landfall.TABS;
using TGCore;

namespace Optimized 
{
    public class OPMain 
    {
        public OPMain()
        {
            var db = ContentDatabase.Instance().LandfallContentDatabase;
            foreach (var unit in db.GetUnitBases().ToList().Where(unit => unit && unit.GetComponent<Unit>().unitType != Unit.UnitType.Warmachine && !unit.GetComponent<SinkOnDeath>()))
            {
                Debug.Log(unit.name);
                unitsWithoutSink.Add(unit);
            }
        }

        public static void SinkBodies_OnValueChanged(int value)
        {
            
        }

        public static readonly List<GameObject> unitsWithoutSink = new List<GameObject>();

        public static bool DisablePP => OPLauncher.ConfigDisablePPEnabled.Value;
    }
}
