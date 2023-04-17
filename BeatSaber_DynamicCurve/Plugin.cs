using HarmonyLib;
using IPA;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace BeatSaber_DynamicCurve
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        /// <summary>
        /// Use to send log messages through BSIPA.
        /// </summary>
        internal static IPALogger Log { get; private set; }
        internal static Harmony harmony;

        [Init]
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            harmony = new Harmony("Loloppe.BeatSaber.BeatmapScanner");
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


        [OnDisable]
        public void OnDisable()
        {
            harmony.UnpatchSelf();
        }
    }
}
