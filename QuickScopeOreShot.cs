using HarmonyLib;

namespace Carbon.Plugins;

[Info("QuickScopeOreShot", "bogdannbv", "0.1.0")]
[Description("Allows players to 360NoScope HotSpots/Markers on Ores and Trees all the time, every time (cue the MLG montage)")]
public class QuickScopeOreShot : CarbonPlugin
{
    public override bool AutoPatch => true;
    
    #region Patches
    [HarmonyPatch(typeof(TreeEntity), "DidHitMarker", typeof(HitInfo))]
    public class TreeEntityDidHitMarker
    {
        public static bool Prefix(HitInfo info, ref bool __result)
        {
            if (info == null)
            {
                return true;
            }
            
            __result = true;
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof(OreResourceEntity), "OnAttacked", typeof(HitInfo))]
    public class OreResourceEntityOnAttacked
    {
        public static void Prefix(OreResourceEntity __instance, HitInfo info)
        {
            if (__instance == null || info is not { CanGather: true })
            {
                return;
            }
            
            if (__instance._hotSpot == null)
            {
                __instance._hotSpot = __instance.SpawnBonusSpot(info.HitPositionWorld);
            }
            
            __instance._hotSpot.transform.position = info.HitPositionWorld;
            __instance._hotSpot.SendNetworkUpdateImmediate();
        }
    }
    #endregion
}
