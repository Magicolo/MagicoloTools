using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionEnter2DCaller : StateMachineCaller {

		void OnCollisionEnter2D(Collision2D collision) {
			if (machine.IsActive) {
				machine.CollisionEnter2D(collision);
			}
		}
	}
}