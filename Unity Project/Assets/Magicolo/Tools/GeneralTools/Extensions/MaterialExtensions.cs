using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public static class MaterialExtensions {

		public static void SetColor(this Material material, Color color, Channels channels) {
			material.color = material.color.SetValues(color, channels);
		}
		
		public static void SetColor(this Material material, float color, Channels channels) {
			material.SetColor(new Color(color, color, color, color), channels);
		}
	}
}

