using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineFixedUpdateCaller : StateMachineCaller {

		void FixedUpdate() {
			if (machine.IsActive) {
				machine.OnFixedUpdate();
			}
		}
	}
}