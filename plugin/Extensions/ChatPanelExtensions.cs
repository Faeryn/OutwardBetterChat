using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BetterChat.Extensions {
	public static class ChatPanelExtensions {
		private static FieldInfo chatEntry_lblMessage = AccessTools.Field(typeof(ChatEntry), "m_lblMessage");
		private static FieldInfo chatEntry_lblPlayerName = AccessTools.Field(typeof(ChatEntry), "m_lblPlayerName");
		private static FieldInfo chatPanel_lblMessage = AccessTools.Field(typeof(ChatPanel), "m_messageArchive");
		private static FieldInfo chatPanel_chatEntry = AccessTools.Field(typeof(ChatPanel), "m_chatEntry");

		public static bool IsInputFocused(this ChatPanel chatPanel) {
			InputField inputField = (InputField) chatPanel_chatEntry.GetValue(chatPanel);
			return inputField.isFocused;
		}
		
		public static void UpdateTimeBeforeFadeOut(this ChatPanel chatPanel) {
			chatPanel.TimeBeforeFadeOut = Math.Max(BetterChat.FadeOutTime.Value, 0f);
		}
		
		public static void UpdateLocalPosition(this ChatPanel chatPanel) {
			chatPanel.transform.localPosition = new Vector3(BetterChat.ChatPanelPosX.Value, -BetterChat.ChatPanelPosY.Value, 0f);
		}

		private static void UpdateChatEntryFontSize(this ChatEntry entry) {
			Text chatText = (Text)chatEntry_lblMessage.GetValue(entry);
			chatText.fontSize = BetterChat.ChatFontSize.Value;
			Text playerNameText = (Text)chatEntry_lblPlayerName.GetValue(entry);
			playerNameText.fontSize = BetterChat.ChatFontSize.Value;
		}

		public static void UpdatePrefabFontSize() {
			UpdateChatEntryFontSize(UIUtilities.ChatEntryPrefab);
		}

		public static void UpdateFontSize(this ChatPanel chatPanel) {
			List<ChatEntry> chatEntries = (List<ChatEntry>)chatPanel_lblMessage.GetValue(chatPanel);
			foreach (ChatEntry entry in chatEntries) {
				UpdateChatEntryFontSize(entry);
			}
		}
	}
}