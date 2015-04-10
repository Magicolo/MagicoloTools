using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public interface IStateLayer : IState {
		
		IState[] GetStates();
	}
}

