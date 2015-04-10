using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public static class HelperFunctions {

		static System.Random randomGenerator;
		static System.Random RandomGenerator {
			get {
				if (randomGenerator == null) {
					randomGenerator = new System.Random(System.DateTime.Now.Millisecond * System.DateTime.Now.Second * System.DateTime.Now.Minute);
				}
				
				return randomGenerator;
			}
		}
		
		public static float MidiToFrequency(float note) {
			return Mathf.Pow(2, (note - 69) / 12) * 440;
		}

		public static KeyCode[] GetKeys() {
			List<KeyCode> keys = new List<KeyCode>();
			
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKey(key)) {
					keys.Add(key);
				}
			}
			
			return keys.ToArray();
		}
		
		public static KeyCode[] GetKeysDown() {
			List<KeyCode> keys = new List<KeyCode>();
			
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown(key)) {
					keys.Add(key);
				}
			}
			
			return keys.ToArray();
		}
		
		public static KeyCode[] GetKeysUp() {
			List<KeyCode> keys = new List<KeyCode>();
			
			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyUp(key)) {
					keys.Add(key);
				}
			}
			
			return keys.ToArray();
		}
		
		public static float Hypotenuse(float a) {
			return Hypotenuse(a, a);
		}
	
		public static float Hypotenuse(float a, float b) {
			return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
		}
	
		public static string GetExtensionForType<T>() where T : Object {
			string extension = "";
			
			if (typeof(T) == typeof(Material)) {
				extension = ".mat";
			}
			else if (typeof(T) == typeof(Cubemap)) {
				extension = ".cubemap";
			}
			else if (typeof(T) == typeof(GUISkin)) {
				extension = ".GUISkin";
			}
			else if (typeof(T) == typeof(Animation)) {
				extension = ".anim";
			}
			else {
				extension = ".asset";
			}
			
			return extension;
		}
		
		public static T[] LoadAllAssetsOfTypeAtPath<T>(string path) where T : Object {
			List<T> assets = new List<T>();
			string extension = GetExtensionForType<T>();
			
			#if UNITY_EDITOR
			foreach (string assetPath in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
				if (assetPath.StartsWith(path) && assetPath.EndsWith(extension)) {
					assets.Add(UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T);
				}
			}
			#endif
		
			return assets.ToArray();
		}
		
		public static T[] LoadAllAssetsOfType<T>() where T : Object {
			return LoadAllAssetsOfTypeAtPath<T>("");
		}
		
		public static T GetDefaultAssetOfType<T>(string assetName) where T : Object {
			T defaultAsset = default(T);
			
			foreach (Object asset in GetDefaultAssetsOfType<T>()) {
				if (asset.name == assetName) {
					defaultAsset = asset as T;
				}
			}
			
			return defaultAsset;
		}
		
		public static Object GetDefaultAsset(string assetName) {
			return GetDefaultAssetOfType<Object>(assetName);
		}
		
		public static T[] GetDefaultAssetsOfType<T>() where T : Object {
			List<T> defaultAssets = new List<T>();
			
			#if UNITY_EDITOR
			foreach (Object asset in UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Library/unity default resources")) {
				if (asset is T) {
					defaultAssets.Add(asset as T);
				}
			}
			#endif
			
			return defaultAssets.ToArray();
		}
		
		public static string GetAssetPath(Object obj) {
			string path = "";
		
			#if UNITY_EDITOR
			path = UnityEditor.AssetDatabase.GetAssetPath(obj).GetRange("/Assets".Length);
			#endif
		
			return path;
		}
	
		public static string GetResourcesPath(Object obj) {
			string resourcesPath = "";
		
			#if UNITY_EDITOR
			resourcesPath = GetResourcesPath(UnityEditor.AssetDatabase.GetAssetPath(obj));
			#endif
		
			return resourcesPath;
		}
		
		public static string GetResourcesPath(string path) {
			string resourcesPath = "";
			
			resourcesPath = GetPathWithoutExtension(path.Substring(path.IndexOf("Resources/") + "Resources/".Length));
				
			return resourcesPath;
		}
	
		public static string GetPathWithoutExtension(string path) {
			return path.Substring(0, path.Length - Path.GetExtension(path).Length);
		}
	
		public static string GetFolderPath(string folderName) {
			string folderPath = "";
		
			#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
				if (path.EndsWith(folderName)) {
					folderPath = path;
					break;
				}
			}
			#endif
		
			return folderPath;
		}

		public static string[] GetFolderPaths(string folderName) {
			List<string> folderPaths = new List<string>();
		
			#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
				if (path.EndsWith(folderName)) {
					folderPaths.Add(path);
					break;
				}
			}
			#endif
		
			return folderPaths.ToArray();
		}
		
		public static T LoadAssetInFolder<T>(string assetFileName, string folderName) where T : Object {
			T asset = default(T);
		
			#if UNITY_EDITOR
			asset = UnityEditor.AssetDatabase.LoadAssetAtPath(GetFolderPath(folderName) + Path.AltDirectorySeparatorChar + assetFileName, typeof(T)) as T;
			#endif
		
			return asset;
		}
	
		public static Object[] LoadAssetsInFolder<T>(string assetFileName, string folderName) where T : Object {
			Object[] assets = null;
		
			#if UNITY_EDITOR
			assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(GetFolderPath(folderName) + Path.AltDirectorySeparatorChar + assetFileName);
			#endif
		
			return assets;
		}

		public static T GetOrAddAssetOfType<T>(string name, string path) where T : ScriptableObject {
			T asset = GetAssetOfType<T>(path);
		
			#if UNITY_EDITOR		
			if (asset == null) {
				Object[] existingAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
				
				if (existingAssets == null || existingAssets.Length == 0) {
					asset = CreateAssetOfType<T>(name, path);
				}
				else {
					asset = AddAssetOfType<T>(name, path);
				}
			}
			#endif
		
			return asset;
		}
	
		public static T GetAssetOfType<T>(string path) where T : ScriptableObject {
			T asset = null;
		
			#if UNITY_EDITOR		
			Object[] existingAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
			asset = System.Array.Find(existingAssets, s => s is T) as T;
			#endif
		
			return asset;
		}
	
		public static T CreateAssetOfType<T>(string name, string path) where T : ScriptableObject {
			T asset = null;
		
			#if UNITY_EDITOR	
			asset = ScriptableObject.CreateInstance<T>();
			asset.name = name;
			UnityEditor.AssetDatabase.CreateAsset(asset, path);
			#endif
		
			return asset;
		}
		
		public static T AddAssetOfType<T>(string name, string path) where T : ScriptableObject {
			T asset = null;
		
			#if UNITY_EDITOR		
			asset = ScriptableObject.CreateInstance<T>();
			asset.name = name;
			UnityEditor.AssetDatabase.AddObjectToAsset(asset, path);
			#endif
		
			return asset;
		}

		public static void SaveAssets() {
			#if UNITY_EDITOR		
			UnityEditor.AssetDatabase.SaveAssets();
			#endif
		}
	
		public static string GetAssetPath(string assetName) {
			string assetPath = "";
		
			#if UNITY_EDITOR
			foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths()) {
				if (path.EndsWith(assetName)){
					assetPath = path;
					break;
				}
			}
			#endif
		
			return assetPath;
		}

		public static bool PathIsRelativeTo(string path, string relativeTo) {
			return path.StartsWith(relativeTo);
		}
		
		public static void Copy<T>(T copyTo, T copyFrom) where T : Object {
			#if UNITY_EDITOR
			UnityEditor.SerializedObject copyToSerialized = new UnityEditor.SerializedObject(copyTo);
			UnityEditor.SerializedObject copyFromSerialized = new UnityEditor.SerializedObject(copyFrom);
			UnityEditor.SerializedProperty iterator = copyFromSerialized.GetIterator();
		
			while (iterator.Next(true)) {
				copyToSerialized.CopyFromSerializedProperty(iterator);
			}
			
			copyToSerialized.ApplyModifiedProperties();
			#endif
		}

		public static float RandomFloat() {
			return (float)RandomDouble();
		}
		
		public static double RandomDouble() {
			return RandomGenerator.NextDouble();
		}
		
		public static int RandomRange(int min, int max) {
			max = max < min ? min : max;
			return (int)(RandomDouble() * (max - min) + min).Round();
		}
		
		public static float RandomRange(float min, float max) {
			max = max < min ? min : max;
			return RandomFloat() * (max - min) + min;
		}
		
		public static T WeightedRandom<T>(Dictionary<T, float> objectsAndWeights) {
			T[] objects = new T[objectsAndWeights.Keys.Count];
			float[] weights = new float[objectsAndWeights.Values.Count];
		
			objectsAndWeights.GetOrderedKeysValues(out objects, out weights);
			
			return WeightedRandom<T>(objects, weights);
		}

		public static T WeightedRandom<T>(IList<T> objects, IList<float> weights) {
			float[] weightSums = new float[weights.Count];
			float weightSum = 0;
			float randomValue = 0;
		
			for (int i = 0; i < weightSums.Length; i++) {
				weightSum += weights[i];
				weightSums[i] = weightSum;
			}
		
			randomValue = RandomRange(0, weightSum);
		
			for (int i = 0; i < weightSums.Length; i++) {
				if (randomValue < weightSums[i]) {
					return objects[i];
				}
			}
			
			return default(T);
		}

		public static float ProportionalRandomRange(float minValue, float maxValue) {
			return Mathf.Pow(2, RandomRange(Mathf.Log(minValue, 2), Mathf.Log(maxValue, 2)));
		}
	}
}
