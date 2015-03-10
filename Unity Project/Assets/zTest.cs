using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class zTest : MonoBehaviourExtended, IComboListener {
	
	public KeyCode aKey = KeyCode.JoystickButton1;
	public KeyCode bKey = KeyCode.JoystickButton0;
	public KeyCode yKey = KeyCode.JoystickButton2;
	public KeyCode xKey = KeyCode.JoystickButton3;
	
	KeyCode[] keys;
	
	bool _comboSystemCached;
	ComboSystem _comboSystem;
	public ComboSystem comboSystem { 
		get { 
			_comboSystem = _comboSystemCached ? _comboSystem : GetComponent<ComboSystem>();
			_comboSystemCached = true;
			return _comboSystem;
		}
	}
	
	[Button("Test", "Test", NoPrefixLabel = true)] public bool test;
	void Test() {
		comboSystem.ResetCombo();
	}

	void Awake() {
		keys = new []{ aKey, bKey, yKey, xKey };
	}
	
	void Update() {
		for (int i = keys.Length - 1; i >= 0; i--) {
			if (Input.GetKeyDown(keys[i])) {
				comboSystem.Input(i);
			}
		}
	}
	
	void OnGUI() {
		string lastSuccessfulCombo = comboSystem.GetLastSuccessfulCombo();
		
		GUILayout.Label("Valid Input: " + comboSystem.GetValidInput<ComboInput1>().ToStringArray().Concat(" | "));
		GUILayout.Label("Valid Combos: " + comboSystem.GetValidCombos().Concat(" | "));
		GUILayout.Label("Current Input: " + comboSystem.GetCurrentInput<ComboInput1>().ToStringArray().Concat(" + "));
		GUILayout.Space(16);
		GUILayout.Label("Last Successful Combo: " + (string.IsNullOrEmpty(lastSuccessfulCombo) ? "" : string.Format("{0} ({1})", lastSuccessfulCombo, comboSystem.GetComboInput<ComboInput1>(lastSuccessfulCombo).ToStringArray().Concat(" + "))));
	}
	
	public void OnComboEnter() {
		Logger.Log("OnComboEnter");
	}

	public void OnComboStay() {
		Logger.Log(comboSystem.GetCurrentInput<ComboInput1>(), comboSystem.GetValidCombos());
	}

	public void OnComboExit() {
		Logger.Log("OnComboExit");
	}

	public void OnComboSuccess(string comboName) {
		Logger.Log("OnComboSuccess", comboName, comboSystem.GetComboInput<ComboInput1>(comboName), comboSystem.GetCurrentInput<ComboInput1>(), comboSystem.GetValidCombos());
	}

	public void OnComboFail(string comboName) {
		Logger.Log("OnComboFail", comboName, comboSystem.GetComboInput<ComboInput1>(comboName), comboSystem.GetCurrentInput<ComboInput1>(), comboSystem.GetValidCombos());
	}
}

public enum ComboInput1 {
	A,
	B,
	Y,
	X
}

public enum ComboInput2 {
	O,
	X,
	S,
	T
}
