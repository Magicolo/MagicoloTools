using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionStayCaller : StateMachineCaller {

		void OnCollisionStay(Collision collision) {
			if (machine.IsActive) {
				machine.CollisionStay(collision);
			}
		}
	}
}