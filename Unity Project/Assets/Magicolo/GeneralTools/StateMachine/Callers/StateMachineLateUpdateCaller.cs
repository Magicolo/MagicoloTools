using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineLateUpdateCaller : StateMachineCaller {

		void LateUpdate() {
			if (machine.IsActive) {
				machine.OnLateUpdate();
			}
		}
	}
}