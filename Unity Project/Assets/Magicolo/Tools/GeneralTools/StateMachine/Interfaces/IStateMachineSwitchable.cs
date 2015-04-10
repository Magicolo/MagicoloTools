using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public interface IStateMachineSwitchable {

    	T SwitchState<T>(int index = 0) where T : IState ;
				
		IState SwitchState(System.Type stateType, int index = 0);
		
		IState SwitchState(string stateName, int index = 0);
		
		bool StateIsActive<T>(int index = 0) where T : IState;
		
		bool StateIsActive(System.Type stateType, int index = 0);
		
		bool StateIsActive(string stateName, int index = 0);
		
		T GetActiveState<T>(int index = 0) where T : IState;
		
		IState GetActiveState(int index = 0);
		
		IState[] GetActiveStates();
	}
}

