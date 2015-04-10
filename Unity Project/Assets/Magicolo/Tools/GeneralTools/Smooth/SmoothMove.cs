using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[AddComponentMenu("Magicolo/Smooth/Move")]
	public class SmoothMove : MonoBehaviourExtended {

		public enum MoveModes {
			Position,
			Rotation,
			Scale,
		}
	
		public MoveModes mode;
		[Mask(Axis.XYZ)] public Axis axis = Axis.XYZ;
		public bool culling = true;
	
		[Separator]
		[Slider] public float randomness;
		public Vector3 speed = Vector3.one;
	
		bool _rendererCached;
		Renderer _renderer;
		new public Renderer renderer { 
			get { 
				_renderer = _rendererCached ? _renderer : GetComponent<Renderer>();
				_rendererCached = true;
				return _renderer;
			}
		}
		
		void Awake() {
			ApplyRandomness();
		}
		
		void Update() {
			if (!culling || renderer.isVisible) {
				switch (mode) {
					case MoveModes.Position:
						transform.TranslateLocal(speed, axis);
						break;
					case MoveModes.Rotation:
						transform.RotateLocal(speed, axis);
						break;
					case MoveModes.Scale:
						transform.ScaleLocal(speed, axis);
						break;
				}
			}
		}
		
		public void ApplyRandomness() {
			speed += speed.SetValues(new Vector3(Random.Range(-randomness * speed.x, randomness * speed.x), Random.Range(-randomness * speed.y, randomness * speed.y), Random.Range(-randomness * speed.z, randomness * speed.z)), axis);
		}
	}
}