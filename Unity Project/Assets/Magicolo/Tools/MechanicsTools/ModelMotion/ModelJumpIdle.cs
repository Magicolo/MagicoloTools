using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelJumpIdle : State {
	
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
		
		if (Input.GetKeyDown(Layer.jumpKey1) || Input.GetKeyDown(Layer.jumpKey2)) {
			SwitchState("Jumping");
			return;
		}
		
		if (!Layer.Grounded) {
			SwitchState("Falling");
			return;
		}
	}
}
