using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.GeneralTools {
	public interface IState : IStateMachineCallable, IStateMachineLayerable, IStateMachineStateable, IStateMachineSwitchable {
		
		IStateLayer layer { get; }
		IStateMachine machine { get; }
		bool IsActive { get; }
		
		void OnAwake();
		
		void OnStart();
		
		void OnEnter();
			
		void OnExit();
	}
}

