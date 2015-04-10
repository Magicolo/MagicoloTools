using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public interface IStateMachine : IStateMachineCallable, IStateMachineLayerable {

		bool Debug { get ; }
		bool IsActive { get ; }
		
		IStateLayer[] GetLayers();
	}
}