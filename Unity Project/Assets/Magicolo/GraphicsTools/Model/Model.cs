using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GraphicsTools {
	public abstract class Model : MonoBehaviourExtended {

		public abstract Dictionary<string, Vector2[]> BoneNameVerticesDict { get; }
		
		public ModelPhysicsOptions physicsOptions;
		public ModelRenderOptions renderOptions;
	}
}

