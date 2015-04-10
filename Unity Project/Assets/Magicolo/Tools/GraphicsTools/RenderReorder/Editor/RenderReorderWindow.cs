using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GraphicsTools {
	public class RenderReorderWindow : CustomWindowBase {

		public string pathFilter;
		public string searchFilter = "";
		
		[SerializeField] Material[] materials = new Material[0];
		List<Material> backgroundMaterials = new List<Material>();
		List<Material> geometryMaterials = new List<Material>();
		List<Material> transparentMaterials = new List<Material>();
		List<Material> overlayMaterials = new List<Material>();
		Dictionary<Material, string> materialGroups = new Dictionary<Material, string>();
		Dictionary<Material, int> materialIndices = new Dictionary<Material, int>();
		
		Vector2 scrollView;
		bool changed = true;
		
		SerializedObject serializedObject;
		SerializedProperty materialsProperty;
		
		[MenuItem("Magicolo's Tools/Render Reorder")]
		public static void Create() {
			CreateWindow<RenderReorderWindow>("Render Reorder", new Vector2(358, 400));
		}
		
		void OnEnable() {
			wantsMouseMove = true;
			Load();
			SetOrdererMaterials();
		}
		
		void OnFocus() {
			SetOrdererMaterials();
		}
		
		void OnProjectChange() {
			SetOrdererMaterials();
		}
		
		void OnDestroy() {
			Save();
		}
		
		void OnGUI() {
			serializedObject = new SerializedObject(this);
			materialsProperty = serializedObject.FindProperty("materials");
			
			EditorGUILayout.Space();
			
			ShowFilters();
			
			EditorGUILayout.Space();
			
			EditorGUI.BeginChangeCheck();
			scrollView = EditorGUILayout.BeginScrollView(scrollView);

			ShowMaterials("Background", backgroundMaterials);
			EditorGUILayout.Space();
			ShowMaterials("Geometry", geometryMaterials);
			EditorGUILayout.Space();
			ShowMaterials("Transparent", transparentMaterials);
			EditorGUILayout.Space();
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
			
			serializedObject.ApplyModifiedProperties();
		}

		void ShowFilters() {
			EditorGUI.BeginChangeCheck();
			CustomEditorBase.BeginBox();
			GUILayout.Space(2);
			EditorGUILayout.BeginHorizontal();
			
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 42;
			
			pathFilter = CustomEditorBase.FolderPathButton(pathFilter, Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), "Filter".ToGUIContent(), GUILayout.MinWidth(200));
			
			EditorGUIUtility.labelWidth = labelWidth;
			
			GUILayout.Space(32);
			
			searchFilter = EditorGUILayout.TextField(searchFilter, new GUIStyle("ToolbarSeachTextField"));
			
			if (GUILayout.Button("", new GUIStyle("ToolbarSeachCancelButton"))) {
				EditorGUIUtility.editingTextField = false;
				searchFilter = "";
			}
			
			EditorGUILayout.EndHorizontal();
			CustomEditorBase.EndBox();
			if (EditorGUI.EndChangeCheck()) {
				SetOrdererMaterials();
				Save();
			}
		}
		
		void ShowMaterials(string category, List<Material> materialGroup) {
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
			
			for (int i = 0; i < materialGroup.Count; i++) {
				Material material = materialGroup[i];
				
				EditorGUILayout.BeginHorizontal();
				
				GUIStyle style = new GUIStyle("label");
				style.fontStyle = FontStyle.Italic;
				style.clipping = TextClipping.Overflow;
				EditorGUILayout.LabelField(materialGroups[material], style, GUILayout.Width(100));
				EditorGUILayout.LabelField(":", new GUIStyle("boldLabel"), GUILayout.Width(24));
				material.renderQueue = EditorGUILayout.IntField(material.name, material.renderQueue);
				
				EditorGUILayout.EndHorizontal();
				
				CustomEditorBase.Reorderable(materialsProperty, materialIndices[material], false, OnMaterialReorder);
			}
			
			if (materialGroup.Count > 0) {
				EditorGUILayout.LabelField(GUIContent.none, new GUIStyle("RL DragHandle"), GUILayout.Height(4));
			}
			
			EditorGUI.indentLevel -= 1;
			EditorGUIUtility.labelWidth = labelWidth;
		}
		
		void OnMaterialReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			Material sourceMaterial = materials[sourceIndex];
			Material targetMaterial = materials[targetIndex];
			int direction = targetIndex > sourceIndex ? 1 : -1;
			
			sourceMaterial.renderQueue = targetMaterial.renderQueue + direction;
			
			if (direction == -1 && targetIndex > 0) {
				Material nextMaterial = materials[targetIndex - 1];
				
				if (nextMaterial.renderQueue >= sourceMaterial.renderQueue) {
					int difference = sourceMaterial.renderQueue - nextMaterial.renderQueue - 1;
					
					for (int i = 0; i < targetIndex; i++) {
						materials[i].renderQueue += difference;
					}
				}
			}
			else if (direction == 1 && targetIndex < materials.Length - 1) {
				Material nextMaterial = materials[targetIndex + 1];
				
				if (nextMaterial.renderQueue <= sourceMaterial.renderQueue) {
					int difference = sourceMaterial.renderQueue - nextMaterial.renderQueue + 1;
					
					for (int i = targetIndex + 1; i < materials.Length; i++) {
						materials[i].renderQueue += difference;
					}
				}
			}
			
			arrayProperty.serializedObject.Update();
			
			SetOrdererMaterials();
		}
		
		void SetOrdererMaterials() {
			materials = HelperFunctions.LoadAllAssetsOfTypeAtPath<Material>(pathFilter);
			backgroundMaterials = new List<Material>();
			geometryMaterials = new List<Material>();
			transparentMaterials = new List<Material>();
			overlayMaterials = new List<Material>();
			materialGroups = new Dictionary<Material, string>();
			materialIndices = new Dictionary<Material, int>();
			
			int[] renderQueues = new int[materials.Length];
			
			for (int i = 0; i < materials.Length; i++) {
				renderQueues[i] = materials[i].renderQueue;
			}
			
			System.Array.Sort(renderQueues, materials);
			
			for (int i = 0; i < materials.Length; i++) {
				Material material = materials[i];
				string[] path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(material)).Split('/');
				string groupName = path.Length == 1 ? path[0] : path[path.Length - 2];
				
				if (FilterMaterial(material, groupName)) {
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
					
					materialGroups[material] = groupName;
					materialIndices[material] = i;
				}
			}
		}
		
		bool FilterMaterial(Material material, string groupName) {
			bool valid = true;
			string[] filters = searchFilter.ToLower().Split(' ');
			
			foreach (string filter in filters) {
				if (string.IsNullOrEmpty(filter)) {
					continue;
				}
				
				if (!(material.name.ToLower().Contains(filter) || groupName.ToLower().Contains(filter) || material.renderQueue.ToString().Contains(filter))) {
					valid = false;
					break;
				}
			}
			
			return valid;
		}
	}
}
