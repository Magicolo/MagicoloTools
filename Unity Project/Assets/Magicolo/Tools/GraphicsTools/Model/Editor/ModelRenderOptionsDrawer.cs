using System.IO;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GraphicsTools {
	[CustomPropertyDrawer(typeof(ModelRenderOptions))]
	public class ModelRenderOptionsDrawer : CustomPropertyDrawerBase {
		
		Model model;
		ModelRenderOptions renderOptions;
		SerializedProperty renderOptionsProperty;
		SerializedProperty materialsProperty;
		Material currentMaterial;
		
		bool updateMaterials;
		readonly string[] shaders = { "Diffuse", "Transparent/Diffuse", "Self-Illumin/Diffuse", "Sprites/Default", "Sprites/Diffuse" };

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			Begin(position, property, label);
			
			model = target as Model;
			renderOptions = model.renderOptions;
			renderOptionsProperty = property;
			renderOptionsProperty.isExpanded = true;
			
			currentPosition.height = lineHeight;
			EditorGUI.LabelField(currentPosition, renderOptionsProperty.displayName, new GUIStyle("boldLabel"));
			currentPosition.y += currentPosition.height + 2;
			
			ShowRenderOptions();
			
			End();
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return property.isExpanded ? EditorGUI.GetPropertyHeight(property, label, true) - 54 : EditorGUI.GetPropertyHeight(property, label, true);
		}
		
		void Initialize() {
			SetBounds();
			renderOptionsProperty.FindPropertyRelative("initialized").SetValue(true);
		}
		
		void ShowRenderOptions() {
			if (!renderOptions.initialized) {
				Initialize();
			}
			
			if (renderOptionsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				ShowRenderType();
				ShowMaterials();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowRenderType() {
			materialsProperty = renderOptionsProperty.FindPropertyRelative("materials");
			materialsProperty.isExpanded = true;
			FindMaterials();
			
			EditorGUI.BeginChangeCheck();
			renderOptions.shader = shaders[EditorGUI.Popup(currentPosition, "Shader", Mathf.Clamp(System.Array.IndexOf(shaders, renderOptions.shader), 0, shaders.Length - 1), shaders)];
			if (EditorGUI.EndChangeCheck() || updateMaterials) {
				serializedObject.Update();
				UpdateShaders();
			}
			currentPosition.y += currentPosition.height;
			
			EditorGUI.BeginChangeCheck();
			PropertyField(renderOptionsProperty.FindPropertyRelative("renderType"));
			PropertyField(renderOptionsProperty.FindPropertyRelative("offset"));
			
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				UpdateMaterialQueues();
			}
		}
		
		void ShowMaterials() {
			GUI.Box(EditorGUI.IndentedRect(new Rect(currentPosition.x, currentPosition.y, currentPosition.width, (lineHeight + 2) * materialsProperty.arraySize)), "");
			currentPosition.y += 2;
			
			for (int i = 0; i < materialsProperty.arraySize; i++) {
				currentMaterial = renderOptions.materials[i];
				
				CustomEditorBase.Reorderable(materialsProperty, i, true, EditorGUI.IndentedRect(currentPosition), OnMaterialReorder);
				ShowMaterial();
			}
		}
		
		void OnMaterialReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			CustomEditorBase.ReorderArray(arrayProperty, sourceIndex, targetIndex);
			UpdateMaterialQueues();
		}
		
		void ShowMaterial() {
			EditorGUI.LabelField(new Rect(currentPosition.x + 2, currentPosition.y + 6, 40, 10), GUIContent.none, new GUIStyle("RL DragHandle"));
			Rect position = currentPosition;
			position.x += 40;
			position.width -= 40;
			position = EditorGUI.PrefixLabel(position, currentMaterial.name.ToGUIContent());
			EditorGUI.LabelField(new Rect(Screen.width - 68, position.y, 68, lineHeight + 2), currentMaterial.renderQueue.ToString());
			currentPosition.y += lineHeight + 2;
		}
		
		void FindMaterials() {
			List<Material> materials = new List<Material>();
			
			foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>()) {
				Material material = renderer.sharedMaterial;
				
				if (material != null && material.name.Contains("Texture")) {
					if (!materials.Contains(material)) {
						materials.Add(material);
					}
				}
			}
			
			materials.Sort((material1, material2) => material2.renderQueue.CompareTo(material1.renderQueue));
			materialsProperty.SetValues(materials);
			updateMaterials = true;
		}
		
		void UpdateShaders() {
			foreach (Material material in renderOptions.materials) {
				material.shader = Shader.Find(renderOptions.shader);
			}
			
			SceneView.RepaintAll();
		}

		void UpdateMaterialQueues() {
			for (int i = 0; i < materialsProperty.arraySize; i++) {
				materialsProperty.GetValue<Material>(i).renderQueue = (int)renderOptions.renderType + renderOptions.offset + materialsProperty.arraySize - i - 1;
			}
			
			serializedObject.Update();
		}
		
		void SetBounds() {
			float l = float.MaxValue;
			float r = float.MinValue;
			float d = float.MaxValue;
			float u = float.MinValue;
			
			SkinnedMeshRenderer[] renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer renderer in renderers) {
				Bounds bounds = renderer.localBounds;
				
				if (bounds.center.x - bounds.extents.x < l) {
					l = bounds.center.x - bounds.extents.x;
				}
				
				if (bounds.center.x + bounds.extents.x > r) {
					r = bounds.center.x + bounds.extents.x;
				}
				
				if (bounds.center.y - bounds.extents.y < d) {
					d = bounds.center.y - bounds.extents.y;
				}
				
				if (bounds.center.y + bounds.extents.y > u) {
					u = bounds.center.y + bounds.extents.y;
				}
			}
			
			Vector3 center = new Vector2(l + r, d + u) / 2;
			Vector3 size = new Vector3(r - l, u - d, 2) * 1.5F;
			foreach (SkinnedMeshRenderer renderer in renderers) {
				renderer.localBounds = new Bounds(center, size);
			}
		}
	}
}