using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class ComboInputManager {

		public List<ComboSequence> validCombos = new List<ComboSequence>();
		public List<int> currentInput = new List<int>();
		public ComboSequence lastSuccessfulCombo;
		public bool comboStarted;
		public ComboSystem comboSystem;
		
		int currentInputIndex;
		float inputCounter;
		
		public ComboInputManager(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Initialize(ComboSystem comboSystem) {
			this.comboSystem = comboSystem;
		}
		
		public void Start() {
			ResetCombo();
		}
		
		public void Update() {
			if (!comboStarted) {
				return;
			}
			
			inputCounter += Time.deltaTime;
			
			for (int i = validCombos.Count - 1; i >= 0; i--) {
				ComboSequence sequence = validCombos[i];
				
				if (currentInputIndex >= sequence.items.Length) {
					validCombos.RemoveAt(i);
					continue;
				}
				
				ComboSequenceItem sequenceItem = sequence.items[currentInputIndex];
				
				if (sequenceItem.timeConstraints && inputCounter > sequenceItem.maxDelay) {
					validCombos.RemoveAt(i);
					comboSystem.messenger.SendOnComboFail(sequence);
				}
			}
			
			if (validCombos.Count == 0) {
				EndCombo();
			}
			else {
				comboSystem.messenger.SendOnComboStay();
			}
		}

		public void Input(int input) {
			if (!comboStarted) {
				BeginCombo();
			}
			
			currentInput.Add(input);
			
			for (int i = validCombos.Count - 1; i >= 0; i--) {
				ComboSequence sequence = validCombos[i];
				
				if (currentInputIndex >= sequence.items.Length) {
					validCombos.RemoveAt(i);
					continue;
				}
				
				ComboSequenceItem sequenceItem = sequence.items[currentInputIndex];
				
				if (sequenceItem.inputIndex == input && sequenceItem.TimingIsValid(inputCounter)) {
					if (currentInputIndex == sequence.items.Length - 1) {
						lastSuccessfulCombo = validCombos.Pop(i);
						comboSystem.messenger.SendOnComboSuccess(sequence);
					}
				}
				else if (currentInputIndex > 0) {
					validCombos.RemoveAt(i);
					comboSystem.messenger.SendOnComboFail(sequence);
				}
				else {
					validCombos.RemoveAt(i);
				}
			}
			
			inputCounter = 0;
			currentInputIndex += 1;
			
			if (validCombos.Count == 0) {
				EndCombo();
			}
		}
		
		public void Input(Enum input) {
			Input(input.GetHashCode());
		}

		public int[] GetCurrentInput() {
			int[] input = new int[currentInput.Count];
			
			for (int i = 0; i < input.Length; i++) {
				input[i] = currentInput[i];
			}
			
			return input;
		}

		public T[] GetCurrentInput<T>() {
			if (typeof(T) != comboSystem.comboManager.inputEnumType) {
				Logger.LogError(string.Format("Type of 'T' must be {0}.", comboSystem.comboManager.inputEnumType.Name));
				return null;
			}
			
			T[] input = new T[currentInput.Count];
			
			for (int i = 0; i < input.Length; i++) {
				input[i] = (T)comboSystem.comboManager.inputEnumValues.GetValue(currentInput[i]);
			}
			
			return input;
		}

		public int[] GetValidInput() {
			List<int> input = new List<int>();
			
			for (int i = validCombos.Count - 1; i >= 0; i--) {
				int value = validCombos[i].items[currentInputIndex].inputIndex;
				
				if (!input.Contains(value)) {
					input.Add(value);
				}
			}
			
			return input.ToArray();
		}

		public T[] GetValidInput<T>() {
			if (typeof(T) != comboSystem.comboManager.inputEnumType) {
				Logger.LogError(string.Format("Type of 'T' must be {0}.", comboSystem.comboManager.inputEnumType.Name));
				return null;
			}
			
			List<T> input = new List<T>();
			
			for (int i = validCombos.Count - 1; i >= 0; i--) {
				T value = (T)comboSystem.comboManager.inputEnumValues.GetValue(validCombos[i].items[currentInputIndex].inputIndex);
				
				if (!input.Contains(value)) {
					input.Add(value);
				}
			}
			
			return input.ToArray();
		}

		public ComboSequence[] GetValidCombos() {
			return validCombos.ToArray();
		}
		
		public void BeginCombo() {
			ResetCombo();
			
			if (!comboStarted) {
				comboStarted = true;
				comboSystem.messenger.SendOnComboEnter();
			}
			
		}
		
		public void EndCombo() {
			if (comboStarted) {
				comboStarted = false;
				comboSystem.messenger.SendOnComboExit();
			}
			
			ResetCombo();
		}
		
		public void ResetCombo() {
			validCombos = new List<ComboSequence>(comboSystem.comboManager.GetUnlockedCombos());
			currentInput.Clear();
//			lastSuccessfulCombo = null;
			currentInputIndex = 0;
			inputCounter = 0;
		}
	}
}
