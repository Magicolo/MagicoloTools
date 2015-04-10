using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo.MechanicsTools;

namespace Magicolo {
	[ExecuteInEditMode]
	[AddComponentMenu("Magicolo/Mechanics/Combo System")]
	public class ComboSystem : MonoBehaviourExtended {

		[SerializeField] ComboSequenceManager _comboManager;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboSequenceManager comboManager {
			get {
				return _comboManager;
			}
		}
		
		[SerializeField] ComboInputManager _inputManager;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboInputManager inputManager {
			get {
				return _inputManager;
			}
		}
		
		[SerializeField] ComboMessenger _messenger;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboMessenger messenger {
			get {
				return _messenger;
			}
		}

		public bool ComboIsStarted {
			get {
				return inputManager.comboStarted;
			}
		}
		
		void Initialize() {
			InitializeManagers();
			
			if (Application.isPlaying) {
				InitializeRuntime();
				StartAll();
			}
		}
		
		void InitializeManagers() {
			_comboManager = _comboManager ?? new ComboSequenceManager(this);
			_comboManager.Initialize(this);
		}
		
		void InitializeRuntime() {
			_inputManager = new ComboInputManager(this);
			_inputManager.Initialize(this);
			_messenger = new ComboMessenger(this);
			_messenger.Initialize(this);
		}

		void StartAll() {
			comboManager.Start();
			inputManager.Start();
			messenger.Start();
		}
		
		void Awake() {
			Initialize();
		}
		
		void Update() {
			if (Application.isPlaying) {
				inputManager.Update();
			}
		}
		
		public void Input(int input) {
			inputManager.Input(input);
		}
		
		public void Input(System.Enum input) {
			inputManager.Input(input);
		}
		
		public void ResetCombo() {
			inputManager.ResetCombo();
		}
			
		public void AddListener(IComboListener listener) {
			messenger.AddListener(listener);
		}
		
		public void RemoveListener(IComboListener listener) {
			messenger.RemoveListener(listener);
		}
		
		public int[] GetComboInput(string comboName) {
			return comboManager.GetComboInput(comboName);
		}
	
		public T[] GetComboInput<T>(string comboName) {
			return comboManager.GetComboInput<T>(comboName);
		}
	
		public int[] GetCurrentInput() {
			return inputManager.GetCurrentInput();
		}
		
		public T[] GetCurrentInput<T>() {
			return inputManager.GetCurrentInput<T>();
		}
		
		public int[] GetValidInput() {
			return inputManager.GetValidInput();
		}
		
		public T[] GetValidInput<T>() {
			return inputManager.GetValidInput<T>();
		}
		
		public string[] GetAllCombos() {
			return comboManager.GetCombos().ToStringArray();
		}
		
		public string[] GetValidCombos() {
			return inputManager.GetValidCombos().ToStringArray();
		}
		
		public string[] GetLockedCombos() {
			return comboManager.GetLockedCombos().ToStringArray();
		}
		
		public string[] GetUnlockedCombos() {
			return comboManager.GetUnlockedCombos().ToStringArray();
		}
		
		public string GetLastSuccessfulCombo() {
			return inputManager.lastSuccessfulCombo == null ? "" : inputManager.lastSuccessfulCombo.Name;
		}
		
		public void SetComboLocked(string comboName, bool locked) {
			comboManager.SetComboLocked(comboName, locked);
		}
	
		public void SetComboTimeConstraints(string comboName, bool enable, float minDelay, float maxDelay) {
			comboManager.SetComboTimeConstraints(comboName, enable, minDelay, maxDelay);
		}
		
		public void SetComboTimeConstraints(string comboName, bool enable) {
			comboManager.SetComboTimeConstraints(comboName, enable);
		}
	
		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable, float minDelay, float maxDelay) {
			comboManager.SetComboTimeConstraints(comboName, inputIndex, enable, minDelay, maxDelay);
		}

		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable) {
			comboManager.SetComboTimeConstraints(comboName, inputIndex, enable);
		}
	}
}
