using BetterChat.Extensions;
using HarmonyLib;

namespace BetterChat.Patches {
	[HarmonyPatch(typeof(ChatPanel))]
	public static class ChatPanelPatches {
		[HarmonyPatch(nameof(ChatPanel.StartInit)), HarmonyPostfix]
		private static void ChatPanel_StartInit_Postfix(ChatPanel __instance) {
			__instance.UpdateTimeBeforeFadeOut();
			__instance.UpdateLocalPosition();
		}
		
		[HarmonyPatch(nameof(ChatPanel.ShowInput)), HarmonyPrefix]
		private static void ChatPanel_ShowInput_Prefix(ChatPanel __instance) {
			__instance.VerticalScrollSpeedModifier = 0.1f;
			__instance.HorizontalScrollSpeedModifier = 5f;
		}
		
		[HarmonyPatch(nameof(ChatPanel.HideInput)), HarmonyPostfix]
		static void Postfix(ChatPanel __instance) {
			__instance.VerticalScrollSpeedModifier = 0f;
			__instance.HorizontalScrollSpeedModifier = 0f;
		}
	}

}