using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public interface IStateMachineLayerable {

		T GetLayer<T>() where T : IStateLayer;
		
		IStateLayer GetLayer(System.Type layerType);
		
		IStateLayer GetLayer(string layerName);
	}
}