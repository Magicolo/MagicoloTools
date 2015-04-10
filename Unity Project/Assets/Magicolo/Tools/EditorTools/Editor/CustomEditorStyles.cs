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
		
		static GUIStyle greenBox;
		public static GUIStyle GreenBox {
			get {
				greenBox = greenBox ?? ColoredBox(new Color(0.2F, 0.4F, 0.2F, 1), 1);
				
				return greenBox;
			}
		}
		
		static GUIStyle redBox;
		public static GUIStyle RedBox {
			get {
				redBox = redBox ?? ColoredBox(new Color(0.4F, 0.2F, 0.2F, 1), 1);
				
				return redBox;
			}
		}
		
		public static GUIStyle ColoredBox(Color boxColor, int border, float alphaFalloff) {
			GUIStyle style = new GUIStyle("box");
			Texture2D texture = new Texture2D(64, 64);
			Color[] pixels = new Color[texture.height * texture.width];
			float alpha = boxColor.a;
			
			for (int y = 0; y < texture.height; y++) {
				for (int x = 0; x < texture.width; x++) {
					bool isBorder = (x < border || x > texture.width - border - 1 || y < border || y > texture.height - border - 1);
					Color pixel = isBorder ? boxColor : boxColor.SetValues(boxColor / 2, Channels.RGB);
					pixel.a = isBorder ? alpha : alpha * alphaFalloff;
					pixels[y * texture.width + x] = pixel;
				}
			}
			
			texture.SetPixels(pixels);
			texture.Apply();
			style.normal.background = texture;
			
			return style;
		}

		public static GUIStyle ColoredBox(Color boxColor, int border) {
			return ColoredBox(boxColor, border, 1);
		}
		
		public static GUIStyle ColoredBox(Color boxColor) {
			return ColoredBox(boxColor, 1, 1);
		}
	}
}
