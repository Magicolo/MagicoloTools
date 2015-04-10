using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(ToggleAttribute))]
	public class ToggleDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			GUIContent trueLabel = ((ToggleAttribute)attribute).trueLabel ?? label;
			GUIContent falseLabel = ((ToggleAttribute)attribute).falseLabel ?? label;
		
			ToggleButton(property, trueLabel, falseLabel);
			
			End();
		}
	}
}