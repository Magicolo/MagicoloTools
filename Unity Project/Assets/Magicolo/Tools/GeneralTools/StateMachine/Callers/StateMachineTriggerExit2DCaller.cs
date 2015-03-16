using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerExit2DCaller : StateMachineCaller {

		void OnTriggerExit2D(Collider2D collision) {
			if (machine.IsActive) {
				machine.TriggerExit2D(collision);
			}
		}
	}
}