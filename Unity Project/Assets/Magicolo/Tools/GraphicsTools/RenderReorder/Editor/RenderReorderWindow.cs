using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GraphicsTools {
	public class RenderReorderWindow : CustomWindowBase {

		List<Material> backgroundMaterials;
		List<Material> geometryMaterials;
		List<Material> transparentMaterials;
		List<Material> overlayMaterials;
		Dictionary<Material, string> materialGroups;
		
		Vector2 scrollView;
		bool changed;
		
		[MenuItem("Magicolo's Tools/Render Reorder")]
		public static void Create() {
			CreateWindow<RenderReorderWindow>("Render Reorder", new Vector2(358, 400));
		}
		
		void OnEnable() {
			wantsMouseMove = true;
			SetOrdererMaterials();
		}
		
		void OnGUI() {
			EditorGUILayout.Space();
			
			EditorGUI.BeginChangeCheck();
			scrollView = EditorGUILayout.BeginScrollView(scrollView);

			ShowMaterials("Background", backgroundMaterials);
			ShowMaterials("Geometry", geometryMaterials);
			ShowMaterials("Transparent", transparentMaterials);
			ShowMaterials("Overlay", overlayMaterials);
			
			CustomEditorBase.Separator();
			
			EditorGUILayout.EndScrollView();
			if (EditorGUI.EndChangeCheck()) {
				changed = true;
			}
			
			if (changed && Event.current.type == EventType.MouseMove && !EditorGUIUtility.editingTextField) {
				SetOrdererMaterials();
				changed = false;
				Event.current.Use();
			}
		}

		void ShowMaterials(string category, List<Material> materials) {
			GUIStyle categoryStyle = new GUIStyle("boldLabel");
			categoryStyle.fontSize = 16;
			EditorGUILayout.LabelField(category, categoryStyle, GUILayout.Height(24));
			
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 149;
			EditorGUI.indentLevel += 1;
			
			EditorGUILayout.LabelField(GUIContent.none, new GUIStyle("RL DragHandle"), GUILayout.Height(4));
			
			EditorGUILayout.BeginHorizontal();
				
			EditorGUILayout.LabelField("Group", new GUIStyle("boldLabel"), GUILayout.Width(100));
			EditorGUILayout.LabelField(" ", new GUIStyle("boldLabel"), GUILayout.Width(24));
			EditorGUILayout.LabelField("Name                        Queue", new GUIStyle("boldLabel"));
				
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField(GUIContent.none, new GUIStyle("RL DragHandle"), GUILayout.Height(4));
			
			for (int i = 0; i < materials.Count; i++) {
				Material material = materials[i];
				
				EditorGUILayout.BeginHorizontal();
				
				GUIStyle style = new GUIStyle("label");
				style.fontStyle = FontStyle.Italic;
				style.clipping = TextClipping.Overflow;
				EditorGUILayout.LabelField(materialGroups[material], style, GUILayout.Width(100));
				EditorGUILayout.LabelField(":", new GUIStyle("boldLabel"), GUILayout.Width(24));
				material.renderQueue = EditorGUILayout.IntField(material.name, material.renderQueue);
				
				EditorGUILayout.EndHorizontal();
			}
			
			if (materials.Count > 0) {
				EditorGUILayout.LabelField(GUIContent.none, new GUIStyle("RL DragHandle"), GUILayout.Height(4));
			}
			EditorGUI.indentLevel -= 1;
			EditorGUIUtility.labelWidth = labelWidth;
		}
		
		void OnFocus() {
			SetOrdererMaterials();
		}
		
		void OnProjectChange() {
			SetOrdererMaterials();
		}
		
		void SetOrdererMaterials() {
			Material[] materials = HelperFunctions.LoadAllAssetsOfType<Material>();
			backgroundMaterials = new List<Material>();
			geometryMaterials = new List<Material>();
			transparentMaterials = new List<Material>();
			overlayMaterials = new List<Material>();
			materialGroups = new Dictionary<Material, string>();
			
			int[] renderQueues = new int[materials.Length];
			
			for (int i = 0; i < materials.Length; i++) {
				renderQueues[i] = materials[i].renderQueue;
			}
			
			System.Array.Sort(renderQueues, materials);
			
			foreach (Material material in materials) {
				if (material.renderQueue < 2000) {
					backgroundMaterials.Add(material);
				}
				else if (material.renderQueue < 3000) {
					geometryMaterials.Add(material);
				}
				else if (material.renderQueue < 4000) {
					transparentMaterials.Add(material);
				}
				else {
					overlayMaterials.Add(material);
				}
				
				string[] path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(material)).Split('/');
				materialGroups[material] = path.Length == 1 ? path[0] : path[path.Length - 2];
			}
		}
	}
}
