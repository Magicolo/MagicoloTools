using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[AddComponentMenu("Magicolo/Oscillation/Transform")]
	public class OscillateTransform : MonoBehaviourExtended {

		public enum OscillationModes {
			Position,
			Rotation,
			Scale,
		}
	
		public enum Axis {
			X,
			Y,
			Z,
			XY,
			XZ,
			YZ,
			XYZ
		}
	
		public OscillationModes mode;
		[Disable(DisableOnStop = false)] public Axis axis = Axis.XYZ;
		[Separator]
		[Slider(DisableOnPlay = true)] public float frequencyRandomness;
		public Vector3 frequency = Vector3.one;
		[Separator]
		[Slider(DisableOnPlay = true)] public float amplitudeRandomness;
		public Vector3 amplitude = Vector3.one;
		[Separator]
		[Slider(DisableOnPlay = true)] public float centerRandomness;
		public Vector3 center;
	
		string axisString;
		
		void Awake() {
			axisString = axis.ToString();
			frequency += frequency.SetValues(new Vector3(Random.Range(-frequencyRandomness * frequency.x, frequencyRandomness * frequency.x), Random.Range(-frequencyRandomness * frequency.y, frequencyRandomness * frequency.y), Random.Range(-frequencyRandomness * frequency.z, frequencyRandomness * frequency.z)), axisString);
			amplitude += amplitude.SetValues(new Vector3(Random.Range(-amplitudeRandomness * amplitude.x, amplitudeRandomness * amplitude.x), Random.Range(-amplitudeRandomness * amplitude.y, amplitudeRandomness * amplitude.y), Random.Range(-amplitudeRandomness * amplitude.z, amplitudeRandomness * amplitude.z)), axisString);
			center += center.SetValues(new Vector3(Random.Range(-centerRandomness * center.x, centerRandomness * center.x), Random.Range(-centerRandomness * center.y, centerRandomness * center.y), Random.Range(-centerRandomness * center.z, centerRandomness * center.z)), axisString);
		}
		
		void Update() {
			switch (mode) {
				case OscillationModes.Position:
					transform.OscillateLocalPosition(frequency, amplitude, center, axisString);
					break;
				case OscillationModes.Rotation:
					transform.OscillateLocalEulerAngles(frequency, amplitude, center, axisString);
					break;
				case OscillationModes.Scale:
					transform.OscillateLocalScale(frequency, amplitude, center, axisString);
					break;
			}
		}
	}
}
