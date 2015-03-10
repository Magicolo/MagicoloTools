using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class ComboMessenger {

		public List<IComboListener> listeners;
		public ComboSystem comboSystem;
		
		public ComboMessenger(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Initialize(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Start() {
			listeners = new List<IComboListener>(comboSystem.GetComponents<IComboListener>());
		}
	
		public void AddListener(IComboListener listener) {
			listeners.Add(listener);
		}
		
		public void RemoveListener(IComboListener listener) {
			listeners.Remove(listener);
		}
		
		public void SendOnComboEnter() {
			foreach (IComboListener listener in listeners) {
				listener.OnComboEnter();
			}
		}
				
		public void SendOnComboStay() {
			foreach (IComboListener listener in listeners) {
				listener.OnComboStay();
			}
		}
				
		public void SendOnComboExit() {
			foreach (IComboListener listener in listeners) {
				listener.OnComboExit();
			}
		}
		
		public void SendOnComboSuccess(ComboSequence sequence) {
			foreach (IComboListener listener in listeners) {
				listener.OnComboSuccess(sequence.Name);
			}
		}
		
		public void SendOnComboFail(ComboSequence sequence) {
			foreach (IComboListener listener in listeners) {
				listener.OnComboFail(sequence.Name);
			}
		}
	}
}
