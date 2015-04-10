using System;
using System.IO;
using Magicolo.GeneralTools;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Magicolo;

namespace Magicolo.EditorTools {
	public class StateMachineGeneratorWindow : CustomWindowBase {

		public string path = "Assets";
		public string layer = "";
		public string inherit = "";
		public string subLayer = "";
		public int callbackMask = 3;
		List<string> states = new List<string> { "Idle", "" };
		List<string> lockedStates = new List<string>();
		
		[MenuItem("Magicolo's Tools/State Machine Generator")]
		public static void Create() {
			CreateWindow<StateMachineGeneratorWindow>("State Machine Generator", new Vector2(275, 250));
		}
		
		void OnGUI() {
			ShowPath();
			ShowLayer();
			ShowStates();
			ShowGeneratesButton();
			minSize = new Vector2(minSize.x, states.Count * 16 + 180);
			maxSize = new Vector2(maxSize.x, minSize.y);
		}
	
		void OnDestroy() {
			Save();
		}
		
		void ShowPath() {
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			
			GUIStyle style = new GUIStyle("boldLabel");
			EditorGUILayout.LabelField("Path: ".ToGUIContent(), style, GUILayout.Width("Path: ".GetWidth(style.font) + 13));
			path = CustomEditorBase.FolderPathButton(path, Application.dataPath.Substring(0, Application.dataPath.Length - 6));
			
			GUILayout.Space(5);
			
			EditorGUILayout.EndHorizontal();
			
			CustomEditorBase.Separator();
		}

		void ShowLayer() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Layer", new GUIStyle("boldLabel"), GUILayout.Width(100));
			layer = EditorGUILayout.TextField(layer);

			EditorGUILayout.EndHorizontal();
			
			ShowInherit();
			ShowSubLayer();
			
			CustomEditorBase.Separator();
		}

		void ShowInherit() {
			List<string> options = new List<string>{ " " };
			options.AddRange(StateMachineUtility.LayerTypes.ToStringArray());
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Inherits From", GUILayout.Width(100));
			inherit = CustomEditorBase.Popup(inherit, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));
			inherit = inherit == " " ? "StateLayer" : inherit;
			
			EditorGUILayout.EndHorizontal();
			
			if (inherit == "StateLayer") {
				lockedStates.Clear();
			}
			else {
				lockedStates = StateMachineUtility.LayerStateNameDict[StateMachineUtility.FormatLayer(inherit)];
					
				for (int i = lockedStates.Count - 1; i >= 0; i--) {
					if (!states.Contains(lockedStates[i])) {
						AddState(lockedStates[i]);
					}
					
					states.Move(states.IndexOf(lockedStates[i]), 0);
				}
			}
			
		}

		void ShowSubLayer() {
			List<string> options = new List<string>{ " " };
			options.AddRange(StateMachineUtility.LayerTypes.ToStringArray());
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Sublayer Of", GUILayout.Width(100));
			subLayer = CustomEditorBase.Popup(subLayer, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));
			subLayer = subLayer == " " ? "" : subLayer;
			
			EditorGUILayout.EndHorizontal();
		}
		
		void ShowStates() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("States", new GUIStyle("boldLabel"), GUILayout.Width(100));
			
			callbackMask = EditorGUILayout.MaskField(callbackMask, StateMachineUtility.FullCallbackNames, GUILayout.Width(position.width / 2.55F));
			
			if (CustomEditorBase.AddButton()) {
				AddState("");
			}
			
			GUILayout.Space(6);
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel += 1;
			
			for (int i = 0; i < states.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup(lockedStates.Contains(states[i]));
				
				states[i] = EditorGUILayout.TextField(states[i]);
				
				if (CustomEditorBase.DeleteButton()) {
					RemoveState(i);
					break;
				}
				
				GUILayout.Space(6);
				
				EditorGUI.EndDisabledGroup();
				EditorGUILayout.EndHorizontal();
			}
			
			if (EditorGUIUtility.editingTextField && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab) {
				AddState("");
			}
			
			EditorGUI.indentLevel -= 1;
			CustomEditorBase.Separator();
		}

		void ShowGeneratesButton() {
			if (CustomEditorBase.LargeButton("Generate".ToGUIContent())) {
				Generate();
			}
			
			CustomEditorBase.Separator();
		}
		
		void AddState(string stateName) {
			states.Add(stateName);
		}
		
		void RemoveState(int index) {
			states.RemoveAt(index);
			
			if (states.Count == 0) {
				states.Add("");
			}
		}
		
		void Generate() {
			GenerateLayer();
			GenerateStates();
			AssetDatabase.Refresh();
			Save();
		}
		
		void GenerateLayer() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(path)) {
				Logger.LogError("Path can not be empty.");
				return;
			}
			
			if (string.IsNullOrEmpty(layer)) {
				Logger.LogError("Layer name can not be empty.");
				return;
			}
			
			string layerFileName = layer.Capitalized() + ".cs";
			List<string> script = new List<string>();
			
			if (!string.IsNullOrEmpty(HelperFunctions.GetAssetPath(layerFileName))) {
				return;
			}

			script.Add("using UnityEngine;");
			script.Add("using System.Collections;");
			script.Add("using System.Collections.Generic;");
			script.Add("using Magicolo;");
			script.Add("");
			script.Add("public class " + layer + " : " + inherit + " {");
			script.Add("	");
			AddLayer(script, subLayer);
			AddMachine(script);
			AddCallbacks(script, callbackMask);
			script.Add("}");
			
			File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + Path.AltDirectorySeparatorChar + path + Path.AltDirectorySeparatorChar + layerFileName, script.ToArray());
			#endif
		}
		
		void GenerateStates() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(path)) {
				Logger.LogError("Path can not be empty.");
				return;
			}
			
			if (string.IsNullOrEmpty(layer)) {
				Logger.LogError("Layer name can not be empty.");
				return;
			}

			foreach (string state in states) {
				string stateFileName = layer.Capitalized() + state.Capitalized() + ".cs";
				string stateInherit = "State";
				List<string> script = new List<string>();
				
				if (string.IsNullOrEmpty(state)) {
					continue;
				}
				
				if (!string.IsNullOrEmpty(HelperFunctions.GetAssetPath(stateFileName))) {
					Logger.LogError(string.Format("A script named {0} already exists.", stateFileName));
					continue;
				}

				string formattedInherit = StateMachineUtility.FormatLayer(inherit);
				if (StateMachineUtility.LayerStateNameDict.ContainsKey(formattedInherit) && StateMachineUtility.LayerStateNameDict[formattedInherit].Contains(state)) {
					stateInherit = inherit + state;
				}
				
				script.Add("using UnityEngine;");
				script.Add("using System.Collections;");
				script.Add("using System.Collections.Generic;");
				script.Add("using Magicolo;");
				script.Add("");
				script.Add("public class " + layer + state + " : " + stateInherit + " {");
				script.Add("	");
				AddLayer(script, layer);
				AddMachine(script);
				AddCallbacks(script, callbackMask);
				script.Add("}");
				
				File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + path + Path.AltDirectorySeparatorChar + stateFileName, script.ToArray());
			}
			#endif
		}
		
		void AddLayer(List<string> script, string layerName) {
			if (string.IsNullOrEmpty(layerName)) {
				return;
			}
			
			script.Add("    " + layerName + " Layer {");
			script.Add("    	get { return ((" + layerName + ")layer); }");
			script.Add("    }");
			script.Add("    ");
		}
		
		void AddMachine(List<string> script) {
			script.Add("    StateMachine Machine {");
			script.Add("    	get { return ((StateMachine)machine); }");
			script.Add("    }");
			script.Add("	");
		}
		
		void AddCallbacks(List<string> script, int callbacks) {
			for (int i = 0; i < StateMachineUtility.FullCallbackNames.Length; i++) {
				if ((callbacks & 1 << i) != 0) {
					script.Add("	public override void " + StateMachineUtility.CallbackOverrideMethods[i] + " {");
					script.Add("		base." + StateMachineUtility.CallbackBaseMethods[i] + ";");
					script.Add("		");
					script.Add("	}");
					script.Add("	");
				}
			}
			
			if (script.Count > 0) {
				script.PopLast();
			}
		}
	}
}

