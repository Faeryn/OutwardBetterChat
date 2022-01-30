using BetterChat.Extensions;
using HarmonyLib;

namespace BetterChat.Patches {
	public class KeybindInfo {
		[HarmonyPatch(typeof(SideLoader.KeybindInfo), "DoGetKey")]
		public class KeybindInfo_DoGetKey {
			[HarmonyPostfix]
			static bool Postfix(bool result) {
				if (!result) {
					return false;
				}
				foreach (Character character in CharacterManager.Instance.Characters.Values) {
					if (character == null || character.CharacterUI == null || character.CharacterUI.ChatPanel == null) {
						continue;
					}
					if (character.CharacterUI.ChatPanel.IsInputFocused()) {
						return false;
					}
				}
				return true;
			}
		}
	}
}