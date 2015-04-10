using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.EditorTools {
	public class CustomAttributePropertyDrawerBase : CustomPropertyDrawerBase {
	
		public string prefixLabel;
		public bool noFieldLabel;
		public bool noPrefixLabel;
		public bool noIndex;
		public bool disableOnPlay;
		public bool disableOnStop;
		public string disableBool;
		public int indent;
		public int index;
		public Event currentEvent;
	
		public bool drawPrefixLabel = true;
		public float scrollbarThreshold;
		public GUIContent currentLabel = GUIContent.none;
	
		public SerializedProperty arrayProperty;
	
		static MethodInfo getPropertyDrawerMethod;
		public static MethodInfo GetPropertyDrawerMethod {
			get {
				if (getPropertyDrawerMethod == null) {
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
						foreach (Type type in assembly.GetTypes()) {
							if (type.Name == "ScriptAttributeUtility") {
								getPropertyDrawerMethod = type.GetMethod("GetDrawerTypeForType", ObjectExtensions.AllFlags);
							}
						}
					}
				}
				return getPropertyDrawerMethod;
			}
		}
	
		public override void Begin(Rect position, SerializedProperty property, GUIContent label) {
			base.Begin(position, property, label);
			
			noFieldLabel = ((CustomAttributeBase)attribute).NoFieldLabel;
			noPrefixLabel = ((CustomAttributeBase)attribute).NoPrefixLabel;
			noIndex = ((CustomAttributeBase)attribute).NoIndex;
			prefixLabel = ((CustomAttributeBase)attribute).PrefixLabel;
			disableOnPlay = ((CustomAttributeBase)attribute).DisableOnPlay;
			disableOnStop = ((CustomAttributeBase)attribute).DisableOnStop;
			disableBool = ((CustomAttributeBase)attribute).DisableBool;
			indent = ((CustomAttributeBase)attribute).Indent;
			
			scrollbarThreshold = Screen.width - position.width > 19 ? 298 : 313;
			currentEvent = Event.current;
			
			bool inverseBool = !string.IsNullOrEmpty(disableBool) && disableBool.StartsWith("!");
			bool boolDisabled = !string.IsNullOrEmpty(disableBool) && property.serializedObject.targetObject.GetValueFromMemberAtPath<bool>(inverseBool ? disableBool.Substring(1) : disableBool);
			boolDisabled = inverseBool ? !boolDisabled : boolDisabled;
			
			EditorGUI.BeginDisabledGroup((Application.isPlaying && disableOnPlay) || (!Application.isPlaying && disableOnStop) || boolDisabled);
			EditorGUI.indentLevel += indent;
			
			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType)) {
				index = AttributeUtility.GetIndexFromLabel(label);
				arrayProperty = property.GetParent();
 			
				if (noIndex) {
					if (string.IsNullOrEmpty(prefixLabel)) {
						label.text = label.text.Substring(0, label.text.Length - 2);
					}
				}
				else if (!string.IsNullOrEmpty(prefixLabel)) {
					prefixLabel += " " + index;
				}
			}
		
		
			if (drawPrefixLabel) {
				if (!noPrefixLabel) {
					if (!string.IsNullOrEmpty(prefixLabel)) {
						label.text = prefixLabel;
					}
					
					position = EditorGUI.PrefixLabel(position, label);
				}
			}
			else {
				if (noPrefixLabel) {
					label.text = "";
				}
				else if (!string.IsNullOrEmpty(prefixLabel)) {
					label.text = prefixLabel;
				}
			}
			
			currentPosition = position;
			currentLabel = label;
		}
	
		public override void End() {
			base.End();
			
			EditorGUI.indentLevel -= indent;
			EditorGUI.EndDisabledGroup();
		}
	
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	
		public PropertyDrawer GetPropertyDrawer(Type propertyAttributeType, params object[] arguments) {
			Type propertyDrawerType = GetPropertyDrawerMethod.Invoke(null, new object[] { propertyAttributeType }) as Type;
			if (propertyDrawerType != null) {
				PropertyAttribute propertyAttribute = Activator.CreateInstance(propertyAttributeType, arguments) as PropertyAttribute;
				PropertyDrawer propertyDrawer = Activator.CreateInstance(propertyDrawerType) as PropertyDrawer;
				propertyDrawer.SetValueToMember("m_Attribute", propertyAttribute);
				propertyDrawer.SetValueToMember("m_FieldInfo", fieldInfo);
				return propertyDrawer;
			}
			return null;
		}
	
		public PropertyDrawer GetPropertyDrawer(Attribute propertyAttribute, params object[] arguments) {
			return GetPropertyDrawer(propertyAttribute.GetType(), arguments);
		}
	}
}