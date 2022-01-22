using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace BetterChat.Patches {
	public static class ChatPanelUtils {
		public static void UpdateTimeBeforeFadeOut(ChatPanel chatPanel) {
			chatPanel.TimeBeforeFadeOut = Math.Max(BetterChat.FadeOutTime.Value, 0f);
		}
		
		public static void UpdateLocalPosition(ChatPanel chatPanel) {
			chatPanel.transform.localPosition = new Vector3(BetterChat.ChatPanelPosX.Value, -BetterChat.ChatPanelPosY.Value, 0f);
		}

		public static void UpdateAll(ChatPanel chatPanel) {
			UpdateTimeBeforeFadeOut(chatPanel);
			UpdateLocalPosition(chatPanel);
		}
	}
	
	[HarmonyPatch(typeof(ChatPanel), "StartInit")]
	public class ChatPanel_StartInit {
		[HarmonyPostfix]
		static void Postfix(ChatPanel __instance) {
			ChatPanelUtils.UpdateAll(__instance);
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