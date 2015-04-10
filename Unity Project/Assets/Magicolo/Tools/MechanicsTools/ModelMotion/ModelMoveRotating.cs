using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelMoveRotating : State {
	
	[Range(0, 360)] public float offset;
	[Min] public float speed = 20;
	[Disable] public float currentFacingAngle;
	[Disable] public float targetFacingAngle;
	
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
		
		targetFacingAngle = Layer.HorizontalAxis > 0 ? offset : Layer.HorizontalAxis < 0 ? 180 + offset : targetFacingAngle;
	}
	
	public override void OnFixedUpdate() {
		currentFacingAngle = Mathf.LerpAngle(transform.localEulerAngles.y, targetFacingAngle, speed * Time.deltaTime);
		
		Layer.rigidbody.RotateTowards(currentFacingAngle, speed, Axis.Y);
	}
}
