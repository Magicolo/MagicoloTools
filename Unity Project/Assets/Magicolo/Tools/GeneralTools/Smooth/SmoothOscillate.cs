using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	[AddComponentMenu("Magicolo/Smooth/Oscillate")]
	public class SmoothOscillate : MonoBehaviourExtended {

		public enum OscillationModes {
			Position,
			Rotation,
			Scale,
		}
	
		public OscillationModes mode;
		[Mask(Axis.XYZ)] public Axis axis = Axis.XYZ;
		public bool culling = true;
		
		[Separator]
		[Slider] public float frequencyRandomness;
		public Vector3 frequency = Vector3.one;
		
		[Separator]
		[Slider] public float amplitudeRandomness;
		public Vector3 amplitude = Vector3.one;
		
		[Separator]
		[Slider] public float centerRandomness;
		public Vector3 center;
	
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
					case OscillationModes.Position:
						transform.OscillateLocalPosition(frequency, amplitude, center, axis);
						break;
					case OscillationModes.Rotation:
						transform.OscillateLocalEulerAngles(frequency, amplitude, center, axis);
						break;
					case OscillationModes.Scale:
						transform.OscillateLocalScale(frequency, amplitude, center, axis);
						break;
				}
			}
		}
		
		public void ApplyRandomness() {
			frequency += frequency.SetValues(new Vector3(Random.Range(-frequencyRandomness * frequency.x, frequencyRandomness * frequency.x), Random.Range(-frequencyRandomness * frequency.y, frequencyRandomness * frequency.y), Random.Range(-frequencyRandomness * frequency.z, frequencyRandomness * frequency.z)), axis);
			amplitude += amplitude.SetValues(new Vector3(Random.Range(-amplitudeRandomness * amplitude.x, amplitudeRandomness * amplitude.x), Random.Range(-amplitudeRandomness * amplitude.y, amplitudeRandomness * amplitude.y), Random.Range(-amplitudeRandomness * amplitude.z, amplitudeRandomness * amplitude.z)), axis);
			center += center.SetValues(new Vector3(Random.Range(-centerRandomness * center.x, centerRandomness * center.x), Random.Range(-centerRandomness * center.y, centerRandomness * center.y), Random.Range(-centerRandomness * center.z, centerRandomness * center.z)), axis);
		}
	}
}
