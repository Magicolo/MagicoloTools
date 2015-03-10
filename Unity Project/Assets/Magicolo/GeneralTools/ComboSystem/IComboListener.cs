using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	public interface IComboListener {

		void OnComboEnter();
	
		void OnComboStay();
	
		void OnComboExit();
	
		void OnComboSuccess(string comboName);
	
		void OnComboFail(string comboName);
	}
}
