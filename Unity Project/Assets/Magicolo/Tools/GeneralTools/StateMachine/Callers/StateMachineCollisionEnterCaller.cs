using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineCollisionEnterCaller : StateMachineCaller {

		void OnCollisionEnter(Collision collision) {
			if (machine.IsActive) {
				machine.CollisionEnter(collision);
			}
		}
	}
}