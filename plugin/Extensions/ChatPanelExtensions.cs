using System;
using UnityEngine;

namespace BetterChat.Extensions {
	public static class ChatPanelExtensions {
		public static void UpdateTimeBeforeFadeOut(this ChatPanel chatPanel) {
			chatPanel.TimeBeforeFadeOut = Math.Max(BetterChat.FadeOutTime.Value, 0f);
		}
		
		public static void UpdateLocalPosition(this ChatPanel chatPanel) {
			chatPanel.transform.localPosition = new Vector3(BetterChat.ChatPanelPosX.Value, -BetterChat.ChatPanelPosY.Value, 0f);
		}

		private static void UpdateChatEntryFontSize(this ChatEntry entry) {
			entry.m_lblMessage.fontSize = BetterChat.ChatFontSize.Value;
			entry.m_lblPlayerName.fontSize = BetterChat.ChatFontSize.Value;
		}

		public static void UpdatePrefabFontSize() {
			UpdateChatEntryFontSize(UIUtilities.ChatEntryPrefab);
		}

		public static void UpdateFontSize(this ChatPanel chatPanel) {
			foreach (ChatEntry entry in chatPanel.m_messageArchive) {
				UpdateChatEntryFontSize(entry);
			}
		}
	}
}