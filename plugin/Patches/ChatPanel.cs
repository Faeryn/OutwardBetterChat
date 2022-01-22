using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BetterChat.Patches {
	public static class ChatPanelUtils {
		private static FieldInfo chatEntry_lblMessage = AccessTools.Field(typeof(ChatEntry), "m_lblMessage");
		private static FieldInfo chatEntry_lblPlayerName = AccessTools.Field(typeof(ChatEntry), "m_lblPlayerName");
		private static FieldInfo chatPanel_lblMessage = AccessTools.Field(typeof(ChatPanel), "m_messageArchive");
		
		public static void UpdateTimeBeforeFadeOut(ChatPanel chatPanel) {
			chatPanel.TimeBeforeFadeOut = Math.Max(BetterChat.FadeOutTime.Value, 0f);
		}
		
		public static void UpdateLocalPosition(ChatPanel chatPanel) {
			chatPanel.transform.localPosition = new Vector3(BetterChat.ChatPanelPosX.Value, -BetterChat.ChatPanelPosY.Value, 0f);
		}

		private static void UpdateChatEntryFontSize(ChatEntry entry) {
			Text chatText = (Text)chatEntry_lblMessage.GetValue(entry);
			chatText.fontSize = BetterChat.ChatFontSize.Value;
			Text playerNameText = (Text)chatEntry_lblPlayerName.GetValue(entry);
			playerNameText.fontSize = BetterChat.ChatFontSize.Value;
		}

		public static void UpdatePrefabFontSize() {
			UpdateChatEntryFontSize(UIUtilities.ChatEntryPrefab);
		}

		public static void UpdateFontSize(ChatPanel chatPanel) {
			List<ChatEntry> chatEntries = (List<ChatEntry>)chatPanel_lblMessage.GetValue(chatPanel);
			foreach (ChatEntry entry in chatEntries) {
				UpdateChatEntryFontSize(entry);
			}
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