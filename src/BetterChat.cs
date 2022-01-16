﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using BetterChat.Patches;

namespace BetterChat {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class BetterChat : BaseUnityPlugin {
		public const string GUID = "faeryn.betterchat";
		public const string NAME = "BetterChat";
		public const string VERSION = "1.0.1";
		private const string DISPLAY_NAME = "Better Chat";
		internal static ManualLogSource Log;
		
		public static ConfigEntry<float> FadeOutTime;
		public static ConfigEntry<float> ChatPanelPosX;
		public static ConfigEntry<float> ChatPanelPosY;

		internal void Awake() {
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			InitializeConfig();
			new Harmony(GUID).PatchAll();
		}

		public void ApplyToChatPanels(Action<ChatPanel> panelFunc) {
			foreach (Character character in CharacterManager.Instance.Characters.Values) {
				panelFunc(character.CharacterUI.ChatPanel);
			}
		}

		private void InitializeConfig() {
			FadeOutTime = Config.Bind(DISPLAY_NAME, "Fade Out Time", 15f, "Chat fade out time in seconds");
			FadeOutTime.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelUtils.UpdateTimeBeforeFadeOut);
			};
			ChatPanelPosX = Config.Bind(DISPLAY_NAME, "Chat panel position X", 0f, "Chat panel horizontal position");
			ChatPanelPosX.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelUtils.UpdateLocalPosition);
			};
			ChatPanelPosY = Config.Bind(DISPLAY_NAME, "Chat panel position Y", 0f, "Chat panel vertical position");
			ChatPanelPosY.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelUtils.UpdateLocalPosition);
			};
		}
	}
}