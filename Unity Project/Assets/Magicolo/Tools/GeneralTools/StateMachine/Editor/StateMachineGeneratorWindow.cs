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
		List<string> states = new List<string> { "" };
		
		[MenuItem("Magicolo's Tools/State Machine Generator")]
		public static void Create() {
			CreateWindow<StateMachineGeneratorWindow>("State Machine Generator", new Vector2(275, 250));
		}
		
		void OnGUI() {
			ShowPath();
			ShowLayer();
			ShowStates();
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
			ShowGenerateLayerButton();
			
			CustomEditorBase.Separator();
		}

		void ShowInherit() {
			List<string> options = new List<string>{ "StateLayer" };
			options.AddRange(StateMachineUtility.LayerTypes.ToStringArray());
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("Inherits from", GUILayout.Width(100));
			inherit = CustomEditorBase.Popup(inherit, options.ToArray(), GUIContent.none, GUILayout.MinWidth(150));

			EditorGUILayout.EndHorizontal();
			
		}
		
		void ShowStates() {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField("States", new GUIStyle("boldLabel"));
			if (CustomEditorBase.AddButton()) {
				states.Add("");
			}
			
			GUILayout.Space(6);
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel += 1;
			
			for (int i = 0; i < states.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				
				states[i] = EditorGUILayout.TextField(states[i]);
				if (CustomEditorBase.DeleteButton()) {
					states.RemoveAt(i);
					break;
				}
				
				GUILayout.Space(6);
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUI.indentLevel -= 1;
			
			ShowGenerateStatesButton();
			
			CustomEditorBase.Separator();
		}

		void ShowGenerateLayerButton() {
			EditorGUILayout.Space();
			
			if (CustomEditorBase.LargeButton("Generate Layer".ToGUIContent())) {
				GenerateLayer();
			}
		}
		
		void ShowGenerateStatesButton() {
			EditorGUILayout.Space();
			
			if (CustomEditorBase.LargeButton("Generate States".ToGUIContent())) {
				GenerateStates();
			}
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
			List<string> layerScript = new List<string>();
			
			if (!string.IsNullOrEmpty(HelperFunctions.GetAssetPath(layerFileName))) {
				Logger.LogError(string.Format("A script named {0} already exists.", layerFileName));
				return;
			}

			layerScript.Add("using UnityEngine;");
			layerScript.Add("using System.Collections;");
			layerScript.Add("using System.Collections.Generic;");
			layerScript.Add("using Magicolo;");
			layerScript.Add("");
			layerScript.Add("public class " + layer + " : " + inherit + " {");
			layerScript.Add("	");
			layerScript.Add("	");
			layerScript.Add("}");
			
			File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + Path.AltDirectorySeparatorChar + path + Path.AltDirectorySeparatorChar + layerFileName, layerScript.ToArray());
			AssetDatabase.Refresh();
			Save();
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
				List<string> stateScript = new List<string>();
				
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
				
				stateScript.Add("using UnityEngine;");
				stateScript.Add("using System.Collections;");
				stateScript.Add("using System.Collections.Generic;");
				stateScript.Add("using Magicolo;");
				stateScript.Add("");
				stateScript.Add("public class " + layer + state + " : " + stateInherit + " {");
				stateScript.Add("	");
				stateScript.Add("    " + layer + " Layer {");
				stateScript.Add("    	get { return ((" + layer + ")layer); }");
				stateScript.Add("    }");
				stateScript.Add("	");
				stateScript.Add("	public override void OnEnter() {");
				stateScript.Add("		base.OnEnter();");
				stateScript.Add("		");
				stateScript.Add("	}");
				stateScript.Add("	");
				stateScript.Add("	public override void OnExit() {");
				stateScript.Add("		base.OnExit();");
				stateScript.Add("		");
				stateScript.Add("	}");
				stateScript.Add("}");
				
				File.WriteAllLines(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + path + Path.AltDirectorySeparatorChar + stateFileName, stateScript.ToArray());
			}
			
			AssetDatabase.Refresh();
			Save();
			#endif
		}
	}
}

