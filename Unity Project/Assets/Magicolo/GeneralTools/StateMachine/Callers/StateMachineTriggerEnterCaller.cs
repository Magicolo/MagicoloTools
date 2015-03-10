using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerEnterCaller : StateMachineCaller {

		void OnTriggerEnter(Collider collision) {
			if (machine.IsActive) {
				machine.TriggerEnter(collision);
			}
		}
	}
}