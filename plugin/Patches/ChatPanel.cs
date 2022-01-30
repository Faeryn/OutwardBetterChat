using BetterChat.Extensions;
using HarmonyLib;

namespace BetterChat.Patches {
	
	[HarmonyPatch(typeof(ChatPanel), "StartInit")]
	public class ChatPanel_StartInit {
		[HarmonyPostfix]
		static void Postfix(ChatPanel __instance) {
			__instance.UpdateTimeBeforeFadeOut();
			__instance.UpdateLocalPosition();
		}
	}

	[HarmonyPatch(typeof(ChatPanel), nameof(ChatPanel.ShowInput))]
	public class ChatPanel_ShowInput {
		[HarmonyPrefix]
		static void Prefix(ChatPanel __instance) {
			__instance.VerticalScrollSpeedModifier = 0.1f;
			__instance.HorizontalScrollSpeedModifier = 5f;
		}
	}
	
	[HarmonyPatch(typeof(ChatPanel), nameof(ChatPanel.HideInput))]
	public class ChatPanel_HideInput {
		[HarmonyPostfix]
		static void Postfix(ChatPanel __instance) {
			__instance.VerticalScrollSpeedModifier = 0f;
			__instance.HorizontalScrollSpeedModifier = 0f;
		}
	}
}