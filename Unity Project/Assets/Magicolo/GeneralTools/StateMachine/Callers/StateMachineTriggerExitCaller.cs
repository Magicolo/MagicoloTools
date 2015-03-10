using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerExitCaller : StateMachineCaller {

		void OnTriggerExit(Collider collision) {
			if (machine.IsActive) {
				machine.TriggerExit(collision);
			}
		}
	}
}