using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionStay2DCaller : StateMachineCaller {

		void OnCollisionStay2D(Collision2D collision) {
			if (machine.IsActive) {
				machine.CollisionStay2D(collision);
			}
		}
	}
}