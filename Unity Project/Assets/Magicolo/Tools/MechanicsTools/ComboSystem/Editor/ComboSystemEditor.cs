using System;
using System.Reflection;
using Magicolo.MechanicsTools;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[CustomEditor(typeof(ComboSystem)), CanEditMultipleObjects]
	public class ComboSystemEditor : CustomEditorBase {
		
		ComboSystem comboSystem;
		ComboSequenceManager comboManager;
		SerializedProperty comboManagerProperty;
		SerializedProperty combosProperty;
		ComboSequence currentSequence;
		SerializedProperty currentSequenceProperty;
		SerializedProperty sequenceItemsProperty;
		ComboSequenceItem currentSequenceItem;
		SerializedProperty currentSequenceItemProperty;
		int currentSequenceItemIndex;
		
		static Dictionary<string, Type> comboEnumTypeDict;
		public static Dictionary<string, Type> ComboEnumTypeDict {
			get {
				if (comboEnumTypeDict == null) {
					BuildComboEnumTypes();
				}
				
				return comboEnumTypeDict;
			}
		}
		
		public override void OnEnable() {
			base.OnEnable();
			
			comboSystem = (ComboSystem)target;
			comboSystem.SetExecutionOrder(-23);
		}
		
		public override void OnInspectorGUI() {
			comboManager = comboSystem.comboManager;
			comboManagerProperty = serializedObject.FindProperty("_comboManager");
			
			Begin();
			
			ShowComboEnums();
			Separator();
			ShowComboSequences();
			
			End();
		}
		
		void ShowComboEnums() {
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			Popup(comboManagerProperty.FindPropertyRelative("inputEnumName"), ComboEnumTypeDict.GetKeyArray(), "Input Enum".ToGUIContent());
			string enumName = comboManagerProperty.FindPropertyRelative("inputEnumName").GetValue<string>();
			string enumTypeName = ComboEnumTypeDict.ContainsKey(enumName) ? ComboEnumTypeDict[enumName].FullName : "";
			
			comboManagerProperty.FindPropertyRelative("inputEnumTypeName").SetValue(enumTypeName);
			
			EditorGUI.EndDisabledGroup();
		}

		void ShowComboSequences() {
			if (string.IsNullOrEmpty(comboManagerProperty.FindPropertyRelative("inputEnumName").GetValue<string>())){
				EditorGUILayout.HelpBox("Create an Enum starting with the prefix 'Combo' with your input options.", MessageType.Info);
				return;
			}
			
			combosProperty = comboManagerProperty.FindPropertyRelative("combos");
			
			if (AddFoldOut(combosProperty, "Combos".ToGUIContent())) {
				comboManager.combos[comboManager.combos.Length - 1] = new ComboSequence(comboSystem);
				comboManager.combos[comboManager.combos.Length - 1].SetUniqueName("default", "", comboManager.combos);
			}
			
			if (combosProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < combosProperty.arraySize; i++) {
					currentSequence = comboManager.combos[i];
					currentSequenceProperty = combosProperty.GetArrayElementAtIndex(i);
					
					BeginBox();
					
					if (DeleteFoldOut(combosProperty, i, GetComboSequenceName(currentSequence).ToGUIContent(), GetComboSequenceStyle())) {
						break;
					}
					
					ShowComboSequence();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowComboSequence() {
			sequenceItemsProperty = currentSequenceProperty.FindPropertyRelative("items");
			
			if (currentSequenceProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				UniqueNameField(currentSequence, comboManager.combos, "Name".ToGUIContent());
				EditorGUILayout.PropertyField(currentSequenceProperty.FindPropertyRelative("locked"));
				
				ShowComboSequenceItems();
								
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowComboSequenceItems() {
			if (AddFoldOut(sequenceItemsProperty, "Input".ToGUIContent())) {
				currentSequence.items[currentSequence.items.Length - 1] = new ComboSequenceItem(comboSystem);
			}
				
			if (sequenceItemsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < sequenceItemsProperty.arraySize; i++) {
					currentSequenceItem = currentSequence.items[i];
					currentSequenceItemProperty = sequenceItemsProperty.GetArrayElementAtIndex(i);
					currentSequenceItemIndex = i;
					
					BeginBox();
					
					Rect rect = EditorGUILayout.BeginHorizontal();
					
					currentSequenceItem.inputIndex = EditorGUILayout.Popup(currentSequenceItem.inputIndex, GetComboEnumNames(), GUILayout.Width(Screen.width - EditorGUI.indentLevel * 16 - 32));
					
					if (DeleteButton(sequenceItemsProperty, i)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					
					Reorderable(sequenceItemsProperty, i, true, EditorGUI.IndentedRect(rect));
					
					ShowComboSequenceItem();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowComboSequenceItem() {
			if (currentSequenceItemIndex > 0) {
				EditorGUILayout.BeginHorizontal();
				
				SerializedProperty timeConstraintsProperty = currentSequenceItemProperty.FindPropertyRelative("timeConstraints");
				EditorGUILayout.PrefixLabel("Time Constraints", new GUIStyle("label"), new GUIStyle("boldLabel"));
				timeConstraintsProperty.SetValue(GUILayout.Toggle(timeConstraintsProperty.GetValue<bool>(), ""));
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUI.indentLevel += 1;
				EditorGUI.BeginDisabledGroup(!currentSequenceItem.timeConstraints);
				
				ShowMinDelay();
				ShowMaxDelay();
				
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel -= 1;
			}
			else {
				currentSequenceItem.timeConstraints = false;
			}
				
			EditorGUI.indentLevel += 1;
			Separator();
			EditorGUI.indentLevel -= 1;
		}

		void ShowMinDelay() {
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(currentSequenceItemProperty.FindPropertyRelative("minDelay"));
			
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				currentSequenceItem.maxDelay = Mathf.Max(currentSequenceItem.maxDelay, currentSequenceItem.minDelay);
				currentSequenceItem.minDelay = Mathf.Clamp(currentSequenceItem.minDelay, 0, currentSequenceItem.maxDelay);
			}
		}

		void ShowMaxDelay() {
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(currentSequenceItemProperty.FindPropertyRelative("maxDelay"));
			
			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
				currentSequenceItem.minDelay = Mathf.Clamp(currentSequenceItem.minDelay, 0, currentSequenceItem.maxDelay);
				currentSequenceItem.maxDelay = Mathf.Max(currentSequenceItem.maxDelay, currentSequenceItem.minDelay);
			}
		}
		
		string GetComboSequenceName(ComboSequence sequence) {
			string displayName = sequence.Name + " (";
			
			foreach (ComboSequenceItem item in sequence.items) {
				displayName += GetComboSequenceItemName(item);
				
				if (item != sequence.items.Last()) {
					displayName += " + ";
				}
			}
			
			displayName += ")";
			
			return displayName;
		}
		
		string GetComboSequenceItemName(ComboSequenceItem item) {
			string itemName = "";
			string[] names = GetComboEnumNames();
				
			item.inputIndex = Mathf.Clamp(item.inputIndex, 0, Mathf.Max(names.Length - 1, 0));
			itemName = names[item.inputIndex];
			
			return itemName;
		}
		
		string[] GetComboEnumNames() {			
			if (ComboEnumTypeDict.ContainsKey(comboManager.inputEnumName)) {
				return Enum.GetNames(ComboEnumTypeDict[comboManager.inputEnumName]);
			}
			
			return new string[0];
		}
		
		GUIStyle GetComboSequenceStyle() {
			GUIStyle style = CustomEditorStyles.BoldFoldout;
			Color textColor = style.normal.textColor * 1.4F;
		
			if (Application.isPlaying) {
				if (currentSequence.locked) {
					textColor *= 0.75F;
				}
				else if (comboSystem.inputManager.comboStarted) {
					textColor = comboSystem.inputManager.validCombos.Contains(currentSequence) ? new Color(0, 1, 0, 10) : new Color(1, 0, 0, 10);
				}
				else if (comboSystem.inputManager.lastSuccessfulCombo == currentSequence) {
					textColor = new Color(1, 1, 0, 10);
				}
				else {
					textColor = new Color(1, 0, 0, 10);
				}
			}

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
			
			return style;
		}
		
		static void BuildComboEnumTypes() {
			comboEnumTypeDict = new Dictionary<string, Type>();
			
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (Type type in assembly.GetTypes()) {
					if (type.IsSubclassOf(typeof(Enum)) && type.Name.Contains("Combo")) {
						comboEnumTypeDict[FormatComboEnum(type)] = type;
					}
				}
			}
		}
		
		static string FormatComboEnum(Type enumType) {
			return FormatComboEnum(enumType.Name);
		}
		
		static string FormatComboEnum(string enumName) {
			return enumName.Split('.').Last().Replace("Combo", "");
		}
		
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts() {
			comboEnumTypeDict = null;
		}
	}
}
