using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class Oscillator {

		public float frequency = 1;
		public float amplitude = 1;
		public float center;
		public float offset;
		
		public float Oscillate() {
			return amplitude * Mathf.Sin(frequency * Time.time + offset) + center;
		}
	}
}