using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	static public class StateMachineUtility {

		static List<Type> layerTypes;
		public static List<Type> LayerTypes {
			get {
				if (layerTypes == null) {
					BuildDicts();
				}
				
				return layerTypes;
			}
		}

		static List<Type> stateTypes;
		public static List<Type> StateTypes {
			get {
				if (stateTypes == null) {
					BuildDicts();
				}
				
				return stateTypes;
			}
		}
		
		static Dictionary<Type, List<Type>> layerStateDict;
		public static Dictionary<Type, List<Type>> LayerStateDict {
			get {
				if (layerStateDict == null) {
					BuildDicts();
				}
				
				return layerStateDict;
			}
		}
				
		static Dictionary<string, List<string>> layerStateNameDict;
		public static Dictionary<string, List<string>> LayerStateNameDict {
			get {
				if (layerStateDict == null) {
					BuildDicts();
				}
				
				return layerStateNameDict;
			}
		}
		
		static Dictionary<Type, string> layerTypeNameDict;
		public static Dictionary<Type, string> LayerTypeNameDict {
			get {
				if (layerTypeNameDict == null) {
					BuildDicts();
				}
				
				return layerTypeNameDict;
			}
		}
		
		public static string FormatLayer(string layerName) {
			string formattedName = layerName.Split('.').Last();
			
			for (int i = formattedName.Replace("Layer", "").Length - 1; i >= 0; i--) {
				char letter = layerName[i];
				
				if (i > 0 && char.IsUpper(letter)) {
					formattedName = formattedName.Insert(i, "/");
				}
			}
			
			return formattedName;
		}
		
		public static string FormatLayer(Type layerType) {
			return FormatLayer(layerType.Name);
		}
		
		public static string FormatState(string stateName, string layerTypePrefix) {
			string formattedName = stateName.Split('.').Last();
			
			formattedName = formattedName.Substring(layerTypePrefix.Length);
			
			return formattedName.Replace("State", "");
		}
		
		public static string FormatState(Type stateType, string layerTypePrefix) {
			return FormatState(stateType.Name, layerTypePrefix);
		}
		
		public static string FormatState(Type stateType, StateLayer layer) {
			return FormatState(stateType, GetLayerPrefix(layer));
		}
		
		public static string FormatState(Type stateType, Type layerType) {
			return FormatState(stateType, GetLayerPrefix(layerType));
		}
	
		public static string GetLayerPrefix(Type layerType) {
			return layerType.Name.Split('.').Last().Replace("Layer", "");
		}
		
		public static string GetLayerPrefix(StateLayer layer) {
			return GetLayerPrefix(layer.GetType());
		}
		
		public static void BuildDicts() {
			layerTypes = new List<Type>();
			stateTypes = new List<Type>();
			layerStateDict = new Dictionary<Type, List<Type>>();
			layerStateNameDict = new Dictionary<string, List<string>>();
			layerTypeNameDict = new Dictionary<Type, string>();
			
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies) {
				Type[] types = assembly.GetTypes();
				
				foreach (Type type in types) {
					if (type.IsSubclassOf(typeof(StateLayer))) {
						layerTypes.Add(type);
					}
					else if (type.IsSubclassOf(typeof(State))) {
						stateTypes.Add(type);
					}
				}
			}
			
			foreach (Type layerType in layerTypes) {
				string layerTypePrefix = GetLayerPrefix(layerType);
				string layerTypeName = FormatLayer(layerType);
				layerStateDict[layerType] = new List<Type>();
				layerStateNameDict[layerTypeName] = new List<string>();
				layerTypeNameDict[layerType] = layerTypeName;
				
				foreach (Type stateType in stateTypes) {
					if (stateType.Name.Split('.').Last().StartsWith(layerTypePrefix)) {
						layerStateDict[layerType].Add(stateType);
						layerStateNameDict[layerTypeName].Add(FormatState(stateType, layerTypePrefix));
					}
				}
			}
		}
		
		#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts() {
			layerStateDict = null;
			layerStateNameDict = null;
			layerTypeNameDict = null;
		}
		#endif
	}
}
