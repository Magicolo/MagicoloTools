using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerStay2DCaller : StateMachineCaller {

		void OnTriggerStay2D(Collider2D collision) {
			if (machine.IsActive) {
				machine.TriggerStay2D(collision);
			}
		}
	}
}