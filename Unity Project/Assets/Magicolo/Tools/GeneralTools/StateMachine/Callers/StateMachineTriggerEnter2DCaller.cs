using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineTriggerEnter2DCaller : StateMachineCaller {

		void OnTriggerEnter2D(Collider2D collision) {
			if (machine.IsActive) {
				machine.TriggerEnter2D(collision);
			}
		}
	}
}