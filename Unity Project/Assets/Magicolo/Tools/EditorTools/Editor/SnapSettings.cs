using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.EditorTools {
	public class SnapSettings : CustomWindowBase {

		public float moveX;
		public float moveY;
		public float moveZ;
		public float rotation;
		public float scale;
		public int gridSize;
		public bool showCubes;
		public bool showLines;

		[MenuItem("Magicolo's Tools/Snap Settings")]
		public static void Create() {
			CreateWindow<SnapSettings>("Snap Settings", new Vector2(275, 176));
		}
			
		void OnGUI() {
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.Space();

			moveX = Mathf.Max(EditorGUILayout.FloatField("Move X", moveX), 0.001F);
			moveY = Mathf.Max(EditorGUILayout.FloatField("Move Y", moveY), 0.001F);
			moveZ = Mathf.Max(EditorGUILayout.FloatField("Move Z", moveZ), 0.001F);
			rotation = Mathf.Max(EditorGUILayout.FloatField("Rotation", rotation), 0.001F);
			scale = Mathf.Max(EditorGUILayout.FloatField("Scale", scale), 0.001F);
			gridSize = EditorGUILayout.IntSlider("Grid Size", gridSize, 0, 100);
			showCubes = EditorGUILayout.Toggle("Show Grid Cubes", showCubes);
			showLines = EditorGUILayout.Toggle("Show Grid Lines", showLines);
		
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Reset", GUILayout.Width(50))) {
				SetDefaultValues();
			}
			EditorGUILayout.EndHorizontal();
			
			if (EditorGUI.EndChangeCheck()) {
				Save();
			}
		}
		
		void OnDestroy() {
			Save();
		}
		
		public override void SetDefaultValues() {
			moveX = 1;
			moveY = 1;
			moveZ = 1;
			rotation = 15;
			scale = 0.1F;
			gridSize = 10;
			showCubes = true;
			showLines = true;
		}
		
		public static void CleanUp() {
			foreach (string key in GetKeys()) {
				if (!key.StartsWith("Toggle")) {
					continue;
				}
			
				bool stillExists = false;
				foreach (Transform transform in Object.FindObjectsOfType<Transform>()) {
					if (key.Contains(transform.GetInstanceID().ToString())) {
						stillExists = true;
						break;
					}
				}
				
				if (!stillExists) {
					DeleteKey(key);
				}
			}
		}
	
		public static object GetValue(string key, System.Type type) {
			return GetValue(key, type, typeof(SnapSettings));
		}
		
		public static T GetValue<T>(string key) {
			return GetValue<T>(key, typeof(SnapSettings));
		}
		
		public static void SetValue(string key, object value) {
			SetValue(key, value, typeof(SnapSettings));
		}

		public static string[] GetKeys() {
			return GetKeys(typeof(SnapSettings));
		}
		
		public static void DeleteKey(string key) {
			DeleteKey(key, typeof(SnapSettings));
		}
	}
}
