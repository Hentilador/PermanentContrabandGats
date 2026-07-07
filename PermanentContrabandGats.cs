using BepInEx;
using HarmonyLib;
using System.Reflection;
using Pigeon; // Based on game namespaces

namespace MycopunkMods
{
    // ==========================================
    // 1. THE BEPINEX PLUGIN ENTRY POINT
    // ==========================================
    [BepInPlugin("hed14.permanentcontrabandgats", "Permanent Contraband Gats", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Create the Harmony instance
            var harmony = new Harmony("hed14.permanentcontrabandgats");

            // This searches this EXACT file (and assembly) for the [HarmonyPatch] class below
            harmony.PatchAll();

            Logger.LogInfo("Mod Initialized: Single-file patch applied successfully!");
        }
    }

    // ==========================================
    // 2. THE HARMONY PATCH LAYER
    // ==========================================
    [HarmonyPatch(typeof(OuroObjective))]
    public static class OuroResetPatch
    {
        [HarmonyPatch("SetupOnMissionStart")]
        [HarmonyPrefix]
        public static bool Prefix(OuroObjective __instance)
        {
            ref bool setupOnStart = ref AccessTools.FieldRefAccess<OuroObjective, bool>(__instance, "setupOnStart");

            if (setupOnStart)
            {
                return false;
            }
            setupOnStart = true;

            OuroObjective.ContrabandsCollected = 0;
            PlayerData.Instance.ouroData.lastOuroTime = 0f;
            PlayerData.Instance.ouroData.lastOuroRooms = 0;
            PlayerData.Instance.ouroData.lastOuroKills = 0;
            PlayerData.Instance.ouroData.lastOuroDamage = 0f;
            PlayerData.Instance.ouroData.lastOuroDeaths = 0;
            PlayerData.Instance.ouroData.lastOuroBosses = 0;
            PlayerData.Instance.ouroData.lastOuroLevels = 0;
            PlayerData.Instance.ouroData.lastOuroUpgrades = 0;
            PlayerData.DeleteMissionUpgrades(Rarity.None);

            return false;
        }
    }
}