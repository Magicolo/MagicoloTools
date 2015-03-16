using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GraphicsTools {
	[System.Serializable]
	public class ModelRenderOptions {
		
		public enum RenderTypes {
			Background = 1000,
			Geometry = 2000,
			Transparent = 3000,
			Overlay = 4000
		}
		
		public string shader = "Transparent/Diffuse";
		public int offset;
		public RenderTypes renderType = RenderTypes.Transparent;
		public List<Material> materials;
		public bool initialized;
	}
}

