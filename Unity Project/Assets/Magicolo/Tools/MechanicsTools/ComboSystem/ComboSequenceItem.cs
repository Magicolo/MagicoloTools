using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	[System.Serializable]
	public class ComboSequenceItem {

		public int inputIndex;
		public bool timeConstraints = true;
		public float minDelay;
		public float maxDelay = 1;
		public ComboSystem comboSystem;
		
		public ComboSequenceItem(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Initialize(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}

		public bool TimingIsValid(float counter) {
			return !timeConstraints || (counter >= minDelay && counter <= maxDelay);
		}
	}
}

