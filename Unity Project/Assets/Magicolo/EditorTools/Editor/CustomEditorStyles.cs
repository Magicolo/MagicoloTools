using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.EditorTools {
	[System.Serializable]
	public static class CustomEditorStyles {

		public static GUIStyle BoldFoldout {
			get {
				GUIStyle style = new GUIStyle("foldout");
				style.fontStyle = FontStyle.Bold;
				return style;
			}
		}
	}
}
