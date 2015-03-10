using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineUpdateCaller : StateMachineCaller {

		void Update() {
			if (machine.IsActive) {
				machine.OnUpdate();
			}
		}
	}
}