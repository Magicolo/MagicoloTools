using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelJumpFalling : State {
	
    ModelJump Layer {
    	get { return ((ModelJump)layer); }
    }
	
	public override void OnEnter() {
		base.OnEnter();
		
	}
	
	public override void OnExit() {
		base.OnExit();
		
	}
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		if (Layer.Grounded) {
			SwitchState("Idle");
			return;
		}
	}
}
