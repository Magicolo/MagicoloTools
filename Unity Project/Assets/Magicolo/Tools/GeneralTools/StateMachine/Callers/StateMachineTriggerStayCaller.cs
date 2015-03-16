using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerStayCaller : StateMachineCaller {

		void OnTriggerStay(Collider collision) {
			if (machine.IsActive) {
				machine.TriggerStay(collision);
			}
		}
	}
}