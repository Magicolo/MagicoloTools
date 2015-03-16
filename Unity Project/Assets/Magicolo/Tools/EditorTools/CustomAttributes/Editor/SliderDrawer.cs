using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(SliderAttribute))]
	public class SliderDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			float min = ((SliderAttribute)attribute).min;
			float max = ((SliderAttribute)attribute).max;
		
			EditorGUI.BeginChangeCheck();
			
			object value = property.GetValue();
			if (value is int) {
				property.SetValue(EditorGUI.IntSlider(currentPosition, label, (int)value, (int)min, (int)max));
			}
			else if (value is float) {
				property.SetValue(EditorGUI.Slider(currentPosition, label, (float)value, min, max));
			}
			else if (value is double) {
				property.SetValue(EditorGUI.Slider(currentPosition, label, (float)(double)value, min, max));
			}
			else {
				EditorGUI.HelpBox(currentPosition, "The type of the field must be numerical.", MessageType.Error);
			}
			
			if (EditorGUI.EndChangeCheck()) {
				property.Clamp(min, max);
			}
			
			End();
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return 16;
		}
	}
}
