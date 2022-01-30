using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using BetterChat.Extensions;
using BetterChat.Patches;
using UnityEngine.UI;

namespace BetterChat {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class BetterChat : BaseUnityPlugin {
		public const string GUID = "faeryn.betterchat";
		public const string NAME = "BetterChat";
		public const string VERSION = "1.1.0";
		private const string DISPLAY_NAME = "Better Chat";
		internal static ManualLogSource Log;
		
		public static ConfigEntry<float> FadeOutTime;
		public static ConfigEntry<int> ChatFontSize;
		public static ConfigEntry<float> ChatPanelPosX;
		public static ConfigEntry<float> ChatPanelPosY;

		internal void Awake() {
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			InitializeConfig();
			new Harmony(GUID).PatchAll();
		}

		private void Start() {
			ChatPanelExtensions.UpdatePrefabFontSize();
		}

		public void ApplyToChatPanels(Action<ChatPanel> panelFunc) {
			foreach (Character character in CharacterManager.Instance.Characters.Values) {
				panelFunc(character.CharacterUI.ChatPanel);
			}
		}

		private void InitializeConfig() {
			FadeOutTime = Config.Bind(DISPLAY_NAME, "Fade Out Time", 15f, "Chat fade out time in seconds");
			FadeOutTime.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelExtensions.UpdateTimeBeforeFadeOut);
			};
			ChatFontSize = Config.Bind(DISPLAY_NAME, "Chat Font Size", 19, "Chat font size");
			ChatFontSize.SettingChanged += (sender, args) => {
				ChatPanelExtensions.UpdatePrefabFontSize();
				ApplyToChatPanels(ChatPanelExtensions.UpdateFontSize);
			};
			ChatPanelPosX = Config.Bind(DISPLAY_NAME, "Chat panel position X", 0f, "Chat panel horizontal position");
			ChatPanelPosX.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelExtensions.UpdateLocalPosition);
			};
			ChatPanelPosY = Config.Bind(DISPLAY_NAME, "Chat panel position Y", 0f, "Chat panel vertical position");
			ChatPanelPosY.SettingChanged += (sender, args) => {
				ApplyToChatPanels(ChatPanelExtensions.UpdateLocalPosition);
			};
		}
	}
}