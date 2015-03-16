using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionExitCaller : StateMachineCaller {

		void OnCollisionExit(Collision collision) {
			if (machine.IsActive) {
				machine.CollisionExit(collision);
			}
		}
	}
}