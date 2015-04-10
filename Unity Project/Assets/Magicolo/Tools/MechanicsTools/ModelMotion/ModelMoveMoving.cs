using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelMoveMoving : State {
	
	[Min] public float speed = 3;
	[Min] public float acceleration = 100;
	[Min] public float inputPower = 1;
	[Disable] public float currentSpeed;
	
    ModelMove Layer {
    	get { return ((ModelMove)layer); }
    }
	
	public override void OnEnter() {
		base.OnEnter();
		
	}
	
	public override void OnExit() {
		base.OnExit();
		
	}
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		if (Layer.AbsHorizontalAxis <= Layer.moveThreshold) {
			SwitchState("Idle");
		}
	}
	
	public override void OnFixedUpdate() {
		currentSpeed = Layer.HorizontalAxis.PowSign(inputPower) * speed;
		
		Layer.rigidbody.AccelerateTowards(currentSpeed, acceleration, Axis.X);
	}
}
