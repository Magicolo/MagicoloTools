using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[CustomEditor(typeof(StateMachine)), CanEditMultipleObjects]
	public class StateMachineEditor : CustomEditorBase {

		StateMachine machine;
		GameObject machineObject;
		StateLayer[] existingLayers;
		State[] existingStates;
		StateMachineCaller[] existingCallers;
		
		Type selectedLayerType;
		List<Type> layerTypes;
		List<string> layerTypesName;
		
		public override void OnEnable() {
			base.OnEnable();
			
			machine = (StateMachine)target;
			machineObject = machine.gameObject;
			
			HideMachineComponents();
			
			if (machine.GetComponents<StateMachine>().Length > 1) {
				Logger.LogError("There can be only one StateMachine per GameObject.");
				machine.Remove();
			}
		}

		public override void OnInspectorGUI() {
			Begin();
			
			HideMachineComponents();
			ShowAddLayer();
			ShowDebug();
			ShowLayers(serializedObject.FindProperty("layers"));
			Separator();
			ShowGenerateButton(serializedObject.FindProperty("layers"));
			ReorderComponents();
			CleanUp();
			
			End();
		}

		void HideMachineComponents() {
			existingLayers = machineObject.GetComponents<StateLayer>();
			existingStates = machineObject.GetComponents<State>();
			existingCallers = machine.GetComponents<StateMachineCaller>();
			
			Array.ForEach(existingLayers, layer => layer.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(existingStates, state => state.hideFlags = HideFlags.HideInInspector);
			Array.ForEach(existingCallers, caller => caller.hideFlags = HideFlags.HideInInspector);
		}

		void ShowAddLayer() {
			layerTypes = new List<Type>();
			layerTypesName = new List<string>{ "Add Layer" };
			
			foreach (Type layerType in StateMachineUtility.LayerStateDict.Keys) {
				if (Array.TrueForAll(existingLayers, layer => layer.GetType() != layerType)) {
					layerTypes.Add(layerType);
					layerTypesName.Add(StateMachineUtility.LayerTypeNameDict[layerType]);
				}
			}
			
			if (Selection.gameObjects.Length <= 1) {
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
				GUIStyle style = new GUIStyle("popup");
				style.fontStyle = FontStyle.Bold;
				style.alignment = TextAnchor.MiddleCenter;
				
				int layerTypeIndex = EditorGUILayout.Popup(layerTypes.IndexOf(selectedLayerType) + 1, layerTypesName.ToArray(), style) - 1;
				selectedLayerType = layerTypeIndex == -1 ? null : layerTypes[Mathf.Clamp(layerTypeIndex, 0, layerTypes.Count - 1)];
			
				EditorGUI.EndDisabledGroup();
				
				if (selectedLayerType != null) {
					StateMachineUtility.AddLayer(machine, selectedLayerType, machine);
					selectedLayerType = null;
				}
			}
			else {
				GUI.Box(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), "Multi-editing is not supported.", new GUIStyle(EditorStyles.helpBox));
			}
		}
		
		void ShowDebug() {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("debug"));
		}
		
		void ShowLayers(SerializedProperty layersProperty) {
			CleanUpLayers(layersProperty);
			
			for (int i = 0; i < layersProperty.arraySize; i++) {
				ShowLayer(layersProperty, i);
			}
		}
		
		void ShowLayer(SerializedProperty layersProperty, int index) {
			SerializedProperty layerProperty = layersProperty.GetArrayElementAtIndex(index);
			StateLayer layer = layerProperty.GetValue<StateLayer>();
			SerializedObject layerSerialized = new SerializedObject(layer);
			SerializedProperty statesProperty = layerSerialized.FindProperty("stateReferences");
			SerializedProperty activeStatesProperty = layerSerialized.FindProperty("activeStateReferences");

			StateMachineUtility.AddMissingStates(machine, layer);
			
			BeginBox(GetBoxStyle(layer));
			
			Rect rect = EditorGUILayout.BeginHorizontal();
				
			ShowAddSubLayer(layer, rect);
				
			if (DeleteFoldOut(layersProperty, index, GetLayerLabel(layer), GetLayerStyle(layer))) {
				StateMachineUtility.RemoveLayer(layer);
				return;
			}
				
			EditorGUILayout.EndHorizontal();
			
			CleanUpStates(statesProperty);
			
			if (layerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				List<string> currentLayerStatesName = new List<string>{ "Empty" };
				
				foreach (IState state in statesProperty.GetValues<IState>()) {
					currentLayerStatesName.Add(state is IStateLayer ? state.GetType().Name.Split('.').Last() : StateMachineUtility.FormatState(state.GetType(), layer));
				}
				
				for (int i = 0; i < activeStatesProperty.arraySize; i++) {
					SerializedProperty activeStateProperty = activeStatesProperty.GetArrayElementAtIndex(i);
					UnityEngine.Object activeState = activeStateProperty.objectReferenceValue;
					
					if (Selection.gameObjects.Length <= 1) {
						Rect dragArea = EditorGUILayout.BeginHorizontal();
				
						EditorGUI.BeginChangeCheck();
				
						int stateIndex = EditorGUILayout.Popup(string.Format("Active State ({0})", i), statesProperty.IndexOf(activeState) + 1, currentLayerStatesName.ToArray(), GUILayout.MinWidth(200)) - 1;
						activeState = stateIndex == -1 ? null : statesProperty.GetValue<UnityEngine.Object>(Mathf.Clamp(stateIndex, 0, statesProperty.arraySize - 1));
						activeStateProperty.SetValue(activeState);
					
						if (EditorGUI.EndChangeCheck() && Application.isPlaying) {
							layer.SwitchState(activeState == null ? typeof(EmptyState) : activeState.GetType(), i);
						}
				
						if (i == 0) {
							SmallAddButton(activeStatesProperty);
						}
						else if (DeleteButton(activeStatesProperty, i)) {
							break;
						}
					
						EditorGUILayout.EndHorizontal();
					
						Reorderable(activeStatesProperty, i, true, EditorGUI.IndentedRect(dragArea));
					}
					else {
						GUI.Box(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), "Multi-editing is not supported.", new GUIStyle(EditorStyles.helpBox));
					}
				}
				
				Separator();
				ShowLayerFields(layerSerialized);
				
				bool stateSeparator = statesProperty.arraySize > 0;
				layerSerialized.ApplyModifiedProperties();
				
				ShowStates(statesProperty, layer);
				
				if (stateSeparator) {
					Separator();
				}
				
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}

		void ShowAddSubLayer(StateLayer parent, Rect rect) {
			layerTypesName[0] = "Add";
			
			if (Selection.gameObjects.Length <= 1) {
				rect.x = Screen.width - 72 - EditorGUI.indentLevel * 16;
				rect.y += 1;
				rect.width = 36 + EditorGUI.indentLevel * 16;
				
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
				GUIStyle style = new GUIStyle("MiniToolbarPopup");
				style.fontStyle = FontStyle.Bold;
				style.alignment = TextAnchor.MiddleCenter;
				
				int layerTypeIndex = EditorGUI.Popup(rect, layerTypes.IndexOf(selectedLayerType) + 1, layerTypesName.ToArray(), style) - 1;
				selectedLayerType = layerTypeIndex == -1 ? null : layerTypes[Mathf.Clamp(layerTypeIndex, 0, layerTypes.Count - 1)];
			
				if (selectedLayerType != null) {
					StateMachineUtility.AddLayer(machine, selectedLayerType, parent);
					selectedLayerType = null;
				}
			
				EditorGUI.EndDisabledGroup();
			}
		}
		
		void ShowLayerFields(SerializedObject layerSerialized) {
			SerializedProperty iterator = layerSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
			
			if (!iterator.NextVisible(false)) {
				return;
			}
			while (true) {
				EditorGUI.BeginChangeCheck();
				
				EditorGUILayout.PropertyField(iterator, true);
				
				if (EditorGUI.EndChangeCheck()) {
					iterator.SetValueToSelected();
				}
				
				if (!iterator.NextVisible(false)) {
					break;
				}
			}
			
			Separator();
		}
		
		void ShowStates(SerializedProperty statesProperty, StateLayer layer) {
			for (int i = 0; i < statesProperty.arraySize; i++) {
				SerializedProperty stateProperty = statesProperty.GetArrayElementAtIndex(i);
				State state = stateProperty.GetValue<UnityEngine.Object>() as State;
				
				if (state == null) {
					ShowLayer(statesProperty, i);
					continue;
				}
				
				BeginBox(GetBoxStyle(state));
				
				Foldout(stateProperty, StateMachineUtility.FormatState(state.GetType(), layer).ToGUIContent(), GetStateStyle(state));
				Reorderable(statesProperty, i, true);
				
				ShowState(stateProperty);
				
				EndBox();
			}
		}
		
		void ShowState(SerializedProperty stateProperty) {
			SerializedObject stateSerialized = new SerializedObject(stateProperty.objectReferenceValue);
			
			if (stateProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				ShowStateFields(stateSerialized);
				
				EditorGUI.indentLevel -= 1;
			}
			
			stateSerialized.ApplyModifiedProperties();
		}
		
		void ShowStateFields(SerializedObject stateSerialized) {
			SerializedProperty iterator = stateSerialized.GetIterator();
			iterator.NextVisible(true);
			iterator.NextVisible(false);
			iterator.NextVisible(false);
				
			if (!iterator.NextVisible(false)) {
				return;
			}
			
			while (true) {
				EditorGUI.BeginChangeCheck();
				
				EditorGUILayout.PropertyField(iterator, true);
				
				if (EditorGUI.EndChangeCheck()) {
					iterator.SetValueToSelected();
				}
				
				if (!iterator.NextVisible(false)) {
					break;
				}
			}
			
			Separator();
		}
		
		void ShowGenerateButton(SerializedProperty layersProperty) {
			if (layersProperty.arraySize == 0) {
				if (LargeButton("Generate".ToGUIContent(), true)) {
					StateMachineGeneratorWindow.Create();
				}
			}
		}

		void CleanUp() {
			foreach (UnityEngine.Object selectedObject in targets) {
				StateMachine selectedMachine = selectedObject as StateMachine;
				
				if (selectedMachine != null) {
					StateMachineUtility.UpdateCallbacks(selectedMachine);
					StateMachineUtility.CleanUp(selectedMachine, selectedMachine.gameObject);
				}
			}
		}
		
		void CleanUpLayers(SerializedProperty layersProperty) {
			if (!Application.isPlaying && machine != null) {
				for (int i = layersProperty.arraySize - 1; i >= 0; i--) {
					if (layersProperty.GetValue<UnityEngine.Object>(i) == null) {
						DeleteFromArray(layersProperty, i);
					}
				}
			}
		}
		
		void CleanUpStates(SerializedProperty statesProperty) {
			if (!Application.isPlaying && machine != null) {
				for (int i = statesProperty.arraySize - 1; i >= 0; i--) {
					if (statesProperty.GetValue<UnityEngine.Object>(i) == null) {
						DeleteFromArray(statesProperty, i);
					}
				}
			}
		}

		void ReorderComponents() {
			int firstStateOrLayerIndex = 0;
			
			Component[] components = machine.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++) {
				Component component = components[i];
				
				if (component as IState != null || component as IStateLayer != null || component as StateMachineCaller != null) {
					firstStateOrLayerIndex = firstStateOrLayerIndex == 0 ? i : firstStateOrLayerIndex;
				}
				else if (firstStateOrLayerIndex > 0) {
					for (int j = 0; j < i - firstStateOrLayerIndex; j++) {
						UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
					}
				}
			}
		}
		
		GUIContent GetLayerLabel(StateLayer layer) {
			string label = layer.GetType().Name;
				
			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab) {
				IState[] activeStates = layer.GetActiveStates();
				string[] activeStateNames = new string[activeStates.Length];
				
				for (int i = 0; i < activeStateNames.Length; i++) {
					activeStateNames[i] = activeStates[i] is IStateLayer ? activeStates[i].GetType().Name.Split('.').Last() : StateMachineUtility.FormatState(activeStates[i].GetType(), layer);
				}
				
				label += " (" + activeStateNames.Concat(", ") + ")";
			}
			
			return label.ToGUIContent();
		}

		GUIStyle GetLayerStyle(StateLayer layer) {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor * 1.4F;
		
			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab) {
				textColor = layer.IsActive ? new Color(0, 1, 0, 10) : new Color(1, 0, 0, 10);
			}

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}

		GUIStyle GetStateStyle(State state) {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor * 1.4F;
		
			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab) {
				textColor = state.IsActive ? new Color(0, 1, 0, 10) : new Color(1, 0, 0, 10);
			}

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}
		
		GUIStyle GetBoxStyle(IState state) {
			GUIStyle style = new GUIStyle("box");
			
			if (Application.isPlaying && PrefabUtility.GetPrefabType(machine) != PrefabType.Prefab) {
				style = state.IsActive ? CustomEditorStyles.GreenBox : CustomEditorStyles.RedBox;
			}
			
			return style;
		}
	}
}
