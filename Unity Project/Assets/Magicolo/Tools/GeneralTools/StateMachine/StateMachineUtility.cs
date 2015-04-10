using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public static class StateMachineUtility {

		public static Type[] CallbackTypes = { 
			typeof(StateMachineUpdateCaller), typeof(StateMachineFixedUpdateCaller), typeof(StateMachineLateUpdateCaller),
			typeof(StateMachineCollisionEnterCaller), typeof(StateMachineCollisionStayCaller), typeof(StateMachineCollisionExitCaller),
			typeof(StateMachineCollisionEnter2DCaller), typeof(StateMachineCollisionStay2DCaller), typeof(StateMachineCollisionExit2DCaller),
			typeof(StateMachineTriggerEnterCaller), typeof(StateMachineTriggerStayCaller), typeof(StateMachineTriggerExitCaller),
			typeof(StateMachineTriggerEnter2DCaller), typeof(StateMachineTriggerStay2DCaller), typeof(StateMachineTriggerExit2DCaller)
		};
		
		public static string[] CallbackNames = {
			"OnUpdate", "OnFixedUpdate", "OnLateUpdate",
			"CollisionEnter", "CollisionStay", "CollisionExit", 
			"CollisionEnter2D", "CollisionStay2D", "CollisionExit2D", 
			"TriggerEnter", "TriggerStay", "TriggerExit", 
			"TriggerEnter2D", "TriggerStay2D", "TriggerExit2D"
		};
		
		public static string[] FullCallbackNames = {
			"OnEnter", "OnExit",
			"OnUpdate", "OnFixedUpdate", "OnLateUpdate",
			"CollisionEnter", "CollisionStay", "CollisionExit", 
			"CollisionEnter2D", "CollisionStay2D", "CollisionExit2D", 
			"TriggerEnter", "TriggerStay", "TriggerExit", 
			"TriggerEnter2D", "TriggerStay2D", "TriggerExit2D"
		};
		
		public static string[] CallbackOverrideMethods = {
			"OnEnter()", "OnExit()",
			"OnUpdate()", "OnFixedUpdate()", "OnLateUpdate()",
			"CollisionEnter(Collision collision)", "CollisionStay(Collision collision)", "CollisionExit(Collision collision)", 
			"CollisionEnter2D(Collision2D collision)", "CollisionStay2D(Collision2D collision)", "CollisionExit2D(Collision2D collision)", 
			"TriggerEnter(Collider collision)", "TriggerStay(Collider collision)", "TriggerExit(Collider collision)", 
			"TriggerEnter2D(Collider2D collision)", "TriggerStay2D(Collider2D collision)", "TriggerExit2D(Collider2D collision)"
		};
		
		public static string[] CallbackBaseMethods = {
			"OnEnter()", "OnExit()",
			"OnUpdate()", "OnFixedUpdate()", "OnLateUpdate()",
			"CollisionEnter(collision)", "CollisionStay(collision)", "CollisionExit(collision)", 
			"CollisionEnter2D(collision)", "CollisionStay2D(collision)", "CollisionExit2D(collision)", 
			"TriggerEnter(collision)", "TriggerStay(collision)", "TriggerExit(collision)", 
			"TriggerEnter2D(collision)", "TriggerStay2D(collision)", "TriggerExit2D(collision)"
		};
		
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
		
		public static StateLayer AddLayer(StateMachine machine, State state) {
			StateLayer layer = null;
			
			#if UNITY_EDITOR
			layer = AddLayer(machine, GetLayerTypeFromState(state), machine);
			AddState(machine, layer, state);
			#endif
			
			return layer;
		}
		
		public static StateLayer AddLayer(StateMachine machine, Type layerType, UnityEngine.Object parent) {
			StateLayer layer = null;
			
			#if UNITY_EDITOR
			layer = AddLayer(machine, machine.GetOrAddComponent(layerType) as StateLayer, parent);
			#endif
			
			return layer;
		}
		
		public static StateLayer AddLayer(StateMachine machine, StateLayer layer, UnityEngine.Object parent) {
			#if UNITY_EDITOR
			layer.hideFlags = HideFlags.HideInInspector;
			
			UnityEditor.SerializedObject parentSerialized = new UnityEditor.SerializedObject(parent);
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty layersProperty = parentSerialized.FindProperty(parent is StateMachine ? "layers" : "stateReferences");
			UnityEditor.SerializedProperty parentProperty = layerSerialized.FindProperty("parentReference");
			
			if (parentProperty.GetValue<UnityEngine.Object>() == null) {
				parentProperty.SetValue(parent);
			}
			
			layerSerialized.FindProperty("machineReference").SetValue(machine);
			layerSerialized.ApplyModifiedProperties();
			
			if (!layersProperty.Contains(layer)) {
				layersProperty.Add(layer);
			}
			
			UpdateLayerStates(machine, layer);
			#endif
			
			return layer;
		}

		public static void RemoveLayer(StateLayer layer) {
			#if UNITY_EDITOR
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			
			foreach (UnityEngine.Object state in statesProperty.GetValues<UnityEngine.Object>()) {
				if (state is StateLayer) {
					RemoveLayer(state as StateLayer);
				}
				else {
					state.Remove();
				}
			}
			
			layer.Remove();
			#endif
		}

		public static State AddState(StateMachine machine, StateLayer layer, Type stateType) {
			State state = null;
			
			#if UNITY_EDITOR
			state = AddState(machine, layer, machine.GetOrAddComponent(stateType) as State);
			#endif
			
			return state;
		}
		
		public static State AddState(StateMachine machine, StateLayer layer, State state) {
			state.hideFlags = HideFlags.HideInInspector;
			
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedObject stateSerialized = new UnityEditor.SerializedObject(state);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			
			stateSerialized.FindProperty("layerReference").SetValue(layer);
			stateSerialized.FindProperty("machineReference").SetValue(machine);
			stateSerialized.ApplyModifiedProperties();
			
			if (!statesProperty.Contains(state)) {
				statesProperty.Add(state);
			}
					
			return state;
		}

		public static void AddMissingStates(StateMachine machine, StateLayer layer) {
			UnityEditor.SerializedObject layerSerialized = new UnityEditor.SerializedObject(layer);
			UnityEditor.SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			
			foreach (Type stateType in LayerStateDict[layer.GetType()]) {
				if (!Array.Exists(statesProperty.GetValues<UnityEngine.Object>(), state => state.GetType() == stateType)) {
					AddState(machine, layer, stateType);
				}
			}
		}
		
		public static Type GetLayerTypeFromState(State state) {
			return GetLayerTypeFromState(state.GetType());
		}
		
		public static Type GetLayerTypeFromState(Type stateType) {
			Type layerType = null;
			
			foreach (KeyValuePair<Type, List<Type>> pair in LayerStateDict) {
				if (pair.Value.Contains(stateType)) {
					layerType = pair.Key;
					break;
				}
			}
			
			return layerType;
		}
		
		public static string FormatLayer(string layerName) {
			string formattedName = layerName.Split('.').Last().Replace("Layer", "");
			int counter = 0;
			
			for (int i = 0; i < formattedName.Length; i++) {
				char letter = formattedName[i];
				
				if (counter > 1 && (char.IsUpper(letter) || char.IsNumber(letter))) {
					formattedName = formattedName.Insert(i, "/");
					i += 1;
					counter = 0;
				}
				
				counter += 1;
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
			return FormatState(stateType.Name, stateType == typeof(EmptyState) ? "" : layerTypePrefix);
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
		
		public static void UpdateLayerStates(StateMachine machine) {
			foreach (StateLayer layer in machine.GetComponents<StateLayer>()) {
				UpdateLayerStates(machine, layer);
			}
		}

		public static void UpdateLayerStates(StateMachine machine, StateLayer layer) {
			foreach (Type stateType in LayerStateDict[layer.GetType()]) {
				StateMachineUtility.AddState(machine, layer, stateType);
			}
		}

		public static void UpdateCallbacks(StateMachine machine) {
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			int callerMask = 0;
			
			foreach (StateLayer layer in machine.GetComponents<StateLayer>()) {
				foreach (MethodInfo method in layer.GetType().GetMethods(flags)) {
					if (CallbackNames.Contains(method.Name)) {
						callerMask |= 1 << (Array.IndexOf(CallbackNames, method.Name) + 2);
					}
				}
			}
			
			foreach (State state in machine.GetComponents<State>()) {
				foreach (MethodInfo method in state.GetType().GetMethods(flags)) {
					if (CallbackNames.Contains(method.Name)) {
						callerMask |= 1 << (Array.IndexOf(CallbackNames, method.Name) + 2);
					}
				}
			}
			
			for (int i = 0; i < CallbackTypes.Length; i++) {
				if ((callerMask & 1 << i + 2) != 0) {
					StateMachineCaller caller = machine.GetOrAddComponent(CallbackTypes[i]) as StateMachineCaller;
					
					caller.machine = machine;
				}
				else {
					StateMachineCaller caller = machine.GetComponent(CallbackTypes[i]) as StateMachineCaller;
					
					if (caller != null) {
						caller.Remove();
					}
				}
			}
		}
		
		public static void CleanUp(StateMachine machine, GameObject gameObject) {
			if (!Application.isPlaying) {
				StateLayer[] layers = gameObject.GetComponents<StateLayer>();
				State[] states = gameObject.GetComponents<State>();
				StateMachineCaller[] callers = gameObject.GetComponents<StateMachineCaller>();
				
				foreach (StateLayer layer in layers) {
					if (layer.machine == null || layer.machine != machine || layer.gameObject != gameObject) {
						layer.Remove();
					}
				}
			
				foreach (State state in states) {
					if (state.machine == null || state.machine != machine || state.gameObject != gameObject || state.layer == null) {
						state.Remove();
					}
				}
			
				foreach (StateMachineCaller caller in callers) {
					if (caller.machine == null || caller.machine != machine || caller.gameObject != gameObject) {
						caller.Remove();
					}
				}
			}
		}
		
		public static void CleanUpAll() {
			if (!Application.isPlaying) {
				StateLayer[] layers = UnityEngine.Object.FindObjectsOfType<StateLayer>();
				State[] states = UnityEngine.Object.FindObjectsOfType<State>();
				StateMachineCaller[] callers = UnityEngine.Object.FindObjectsOfType<StateMachineCaller>();
				
				foreach (StateLayer layer in layers) {
					if (layer.machine == null) {
						layer.Remove();
					}
				}
			
				foreach (State state in states) {
					if (state.machine == null || state.layer == null) {
						state.Remove();
					}
				}
			
				foreach (StateMachineCaller caller in callers) {
					if (caller.machine == null) {
						caller.Remove();
					}
				}
			}
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
			
			CleanUpAll();
		}
		#endif
	}
}
