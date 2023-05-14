using System.Collections.Generic;
using BetterChat.Extensions;
using HarmonyLib;
using UnityEngine;

namespace BetterChat.Patches {
	[HarmonyPatch(typeof(ChatPanel))]
	public static class ChatPanelPatches {
		// We can't use ChatPanel's m_messageArchive because it doesn't have commands 
		private static List<string> chatHistory = new List<string>();
		private static string lastMessage;
		private static int currentChatHistoryIndex;
		
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
		private static void ChatPanel_HideInput_Postfix(ChatPanel __instance) {
			__instance.VerticalScrollSpeedModifier = 0f;
			__instance.HorizontalScrollSpeedModifier = 0f;
		}
		
		[HarmonyPatch(nameof(ChatPanel.Update)), HarmonyPostfix]
		private static void ChatPanel_Update_Postfix(ChatPanel __instance) {
			if (__instance.JustFocused) {
				return;
			}

			int chatHistoryOffset = 0;
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				chatHistoryOffset--;
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				chatHistoryOffset++;
			}

			if (chatHistoryOffset == 0) {
				return;
			}

			if (currentChatHistoryIndex == chatHistory.Count) {
				lastMessage = __instance.m_chatEntry.text;
			}
			
			currentChatHistoryIndex = Mathf.Clamp(currentChatHistoryIndex + chatHistoryOffset, 0, chatHistory.Count);

			__instance.m_chatEntry.text = currentChatHistoryIndex == chatHistory.Count ? lastMessage : chatHistory[currentChatHistoryIndex];
			__instance.m_chatEntry.caretPosition = __instance.m_chatEntry.text.Length;
		}

		[HarmonyPatch(nameof(ChatPanel.SendChatMessage)), HarmonyPrefix]
		private static void ChatPanel_SendChatMessage_Prefix(ChatPanel __instance) {
			chatHistory.Add(__instance.m_chatEntry.text);
			currentChatHistoryIndex = chatHistory.Count;
		}
	}

}