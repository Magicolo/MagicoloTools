using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class ComboSequenceManager {

		public string inputEnumName;
		public string inputEnumTypeName;
		public Type inputEnumType;
		public Array inputEnumValues;
		public ComboSequence[] combos = new ComboSequence[0];
		public ComboSystem comboSystem;
		
		Dictionary<string, ComboSequence> comboDict;
		
		public ComboSequenceManager(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Initialize(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
			
			foreach (ComboSequence sequence in combos) {
				sequence.Initialize(comboSystem);
			}
		}
		
		public void Start() {
			BuildComboDict();
			
			inputEnumType = Type.GetType(inputEnumTypeName);
			inputEnumValues = inputEnumType == null ? new object[0] : Enum.GetValues(inputEnumType);
		}

		public ComboSequence[] GetCombos() {
			ComboSequence[] combosCopy = new ComboSequence[combos.Length];
			combos.CopyTo(combosCopy, 0);
			
			return combosCopy;
		}
		
		public ComboSequence GetCombo(string comboName) {
			ComboSequence combo = null;
			
			try {
				combo = comboDict[comboName];
			}
			catch {
				Logger.LogError(string.Format("Combo named {0} was not found.", comboName));
			}
			
			return combo;
		}

		public ComboSequence[] GetLockedCombos() {
			List<ComboSequence> lockedCombos = new List<ComboSequence>();
			
			for (int i = 0; i < combos.Length; i++) {
				ComboSequence combo = combos[i];
				
				if (combo.locked) {
					lockedCombos.Add(combo);
				}
			}
			
			return lockedCombos.ToArray();
		}
		
		public ComboSequence[] GetUnlockedCombos() {
			List<ComboSequence> unlockedCombos = new List<ComboSequence>();
			
			for (int i = 0; i < combos.Length; i++) {
				ComboSequence combo = combos[i];
				
				if (!combo.locked) {
					unlockedCombos.Add(combo);
				}
			}
			
			return unlockedCombos.ToArray();
		}
		
		public int[] GetComboInput(string comboName) {
			ComboSequence combo = GetCombo(comboName);
			int[] input = new int[combo.items.Length];
			
			for (int i = 0; i < input.Length; i++) {
				input[i] = combo.items[i].inputIndex;
			}
			
			return input;
		}

		public T[] GetComboInput<T>(string comboName) {
			if (typeof(T) != inputEnumType) {
				Logger.LogError(string.Format("Type of 'T' must be {0}.", inputEnumType.Name));
				return null;
			}
			
			ComboSequence combo = GetCombo(comboName);
			T[] input = new T[combo.items.Length];
			
			for (int i = 0; i < input.Length; i++) {
				input[i] = (T)inputEnumValues.GetValue(combo.items[i].inputIndex);
			}
			
			return input;
		}
		
		public void SetComboLocked(string comboName, bool locked) {
			GetCombo(comboName).locked = locked;
		}

		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable, float minDelay, float maxDelay) {
			ComboSequenceItem item = GetCombo(comboName).items[inputIndex];
			
			SetComboTimeConstraints(item, enable, minDelay, maxDelay);
		}
		
		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable) {
			ComboSequenceItem item = GetCombo(comboName).items[inputIndex];
			
			SetComboTimeConstraints(item, enable, item.minDelay, item.maxDelay);
		}
		
		public void SetComboTimeConstraints(string comboName, bool enable, float minDelay, float maxDelay) {
			ComboSequence combo = GetCombo(comboName);
			
			foreach (ComboSequenceItem item in combo.items) {
				SetComboTimeConstraints(item, enable, minDelay, maxDelay);
			}
		}
		
		public void SetComboTimeConstraints(string comboName, bool enable) {
			ComboSequence combo = GetCombo(comboName);
			
			foreach (ComboSequenceItem item in combo.items) {
				SetComboTimeConstraints(item, enable, item.minDelay, item.maxDelay);
			}
		}
		
		public void SetComboTimeConstraints(ComboSequenceItem item, bool enable, float minDelay, float maxDelay) {
			item.timeConstraints = enable;
			item.minDelay = minDelay;
			item.maxDelay = maxDelay;
		}
		
		void BuildComboDict() {
			comboDict = new Dictionary<string, ComboSequence>();
			
			foreach (ComboSequence combo in combos) {
				comboDict[combo.Name] = combo;
			}
		}
	}
}
