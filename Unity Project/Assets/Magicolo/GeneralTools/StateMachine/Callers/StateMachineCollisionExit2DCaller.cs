using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionExit2DCaller : StateMachineCaller {

		void OnCollisionExit2D(Collision2D collision) {
			if (machine.IsActive) {
				machine.CollisionExit2D(collision);
			}
		}
	}
}