using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class ComboSequence : INamable {

		[SerializeField]
		string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public bool locked;
		
		public ComboSequenceItem[] items = new ComboSequenceItem[0];
		public ComboSystem comboSystem;
		
		public ComboSequence(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Initialize(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
			
			foreach (ComboSequenceItem item in items) {
				item.Initialize(comboSystem);
			}
		}
		
		public override string ToString() {
			return Name;
		}
	}
}
