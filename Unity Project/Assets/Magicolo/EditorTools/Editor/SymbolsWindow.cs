using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Globalization;

namespace Magicolo.EditorTools {
	public class SymbolsWindow : EditorWindow {

		Dictionary<string, List<char>> symbols;
		
		const int symbolsPerPage = 64;
		string[] categories;
		char[] currentSymbols;
		int currentCategory;
		int currentPage;
		int[] pageIndices;
		List<string> mostRecentlyUsed = new List<string>(10);
	
		static SymbolsWindow Instance;
	
		[MenuItem("Magicolo's Tools/Symbols")]
		static void CreateSymbolsWindow() {
			if (Instance == null) {
				Instance = (SymbolsWindow)EditorWindow.GetWindow<SymbolsWindow>("Symbols", true);
				Instance.position = new Rect(Screen.currentResolution.width / 2 - 121, Screen.currentResolution.height / 2 - 167, 275, 335);
				Instance.minSize = new Vector2(275, 335);
				Instance.BuildSymbolsDict();
			}
		}
	
		void OnGUI() {
			if (symbols == null) BuildSymbolsDict();
		
			GUIStyle recentStyle = new GUIStyle("miniButton");
			recentStyle.alignment = TextAnchor.MiddleCenter;
			recentStyle.fontSize = (Mathf.Min(Screen.width, Screen.height) + 64) / 20;
			recentStyle.clipping = TextClipping.Overflow;
			float recentWidth = (float)(Screen.width - 40) / 10;
			float recentHeight = (float)(Screen.height - 140) / 10;
		
			GUIStyle style = new GUIStyle("button");
			style.fontSize = (Mathf.Min(Screen.width, Screen.height) + 64) / 16;
			style.alignment = TextAnchor.MiddleCenter;
			style.clipping = TextClipping.Overflow;
			float width = ((float)(Screen.width - 32)) / 8;
			float height = (float)(Screen.height - (float)(Screen.height - 140) / 10 - 100) / 8;
		
			EditorGUILayout.Space();
		
			EditorGUILayout.BeginHorizontal();
		
			for (int i = 0; i < mostRecentlyUsed.Count; i++) {
				string recentLabel = "|                                                                                     " + mostRecentlyUsed[i] + "                                                                                     |";
				if (GUILayout.Button(recentLabel, recentStyle, GUILayout.Width(recentWidth), GUILayout.Height(recentHeight))) {
					ToCopyBuffer(mostRecentlyUsed[i]);
				}
			}
		
			EditorGUILayout.EndHorizontal();
		
			EditorGUILayout.Space();
		
			EditorGUIUtility.labelWidth = 100;
			EditorGUI.BeginChangeCheck();
			currentCategory = EditorGUILayout.Popup("Category", currentCategory, categories);
			if (EditorGUI.EndChangeCheck()) {
				currentSymbols = symbols[categories[currentCategory]].ToArray();
				pageIndices = new int[(int)(currentSymbols.Length / symbolsPerPage) + 1];
				for (int i = 0; i < pageIndices.Length; i++) {
					pageIndices[i] = i;
				}
				currentPage = 0;
			}
		
			EditorGUI.BeginChangeCheck();
			currentPage = EditorGUILayout.IntPopup("Page", currentPage, pageIndices.ToStringArray(), pageIndices);
			if (EditorGUI.EndChangeCheck()) {
				pageIndices = new int[(int)(currentSymbols.Length / symbolsPerPage) + 1];
				for (int i = 0; i < pageIndices.Length; i++) {
					pageIndices[i] = i;
				}
			}
		
			for (int i = 0; i < symbolsPerPage / 8; i++) {
				if (i * 8 + (currentPage * symbolsPerPage) >= currentSymbols.Length) break;
			
				EditorGUILayout.BeginHorizontal();
				for (int j = 0; j < 8; j++) {
					int symbolIndex = i * 8 + j + (currentPage * symbolsPerPage);
					if (symbolIndex >= currentSymbols.Length) break;
				
					string currentSymbol = currentSymbols[symbolIndex].ToString();
					string label = "|                                                                                     " + currentSymbol + "                                                                                     |";
					if (GUILayout.Button(label, style, GUILayout.Width(width), GUILayout.Height(height))) {
						ToCopyBuffer(currentSymbol);
					}
				}
			
				EditorGUILayout.EndHorizontal();
			}
		}
	
		void ToCopyBuffer(string str) {
			EditorGUIUtility.systemCopyBuffer = str;
			Debug.Log(str + "  was copied to clipboard.");
		
			if (mostRecentlyUsed.Contains(str)) {
				mostRecentlyUsed.Remove(str);
			}
			mostRecentlyUsed.Insert(0, str);
			if (mostRecentlyUsed.Count > 10) {
				mostRecentlyUsed.RemoveAt(10);
			}
			currentSymbols = symbols[categories[currentCategory]].ToArray();
		}
	
		void BuildSymbolsDict() {
			symbols = new Dictionary<string, List<char>>();
		
			for (int i = 0; i < 55295; i++) {
				char currentChar = char.ConvertFromUtf32(i)[0];
				if (char.IsControl(currentChar) || char.IsWhiteSpace(currentChar) || !EditorStyles.standardFont.HasCharacter(currentChar) || char.GetUnicodeCategory(currentChar) == UnicodeCategory.SpacingCombiningMark) {
					continue;
				}
			
				string category = char.GetUnicodeCategory(currentChar).ToString();
				if (!symbols.ContainsKey(category)) symbols[category] = new List<char>();
				symbols[category].Add(currentChar);
			}
		
			var newCategories = new List<string>(symbols.Keys);
			newCategories.Sort();
			categories = newCategories.ToArray();
			currentSymbols = symbols[categories[currentCategory]].ToArray();
			pageIndices = new int[(int)(currentSymbols.Length / symbolsPerPage) + 1];
			for (int i = 0; i < pageIndices.Length; i++) {
				pageIndices[i] = i;
			}
		}
	}
}
