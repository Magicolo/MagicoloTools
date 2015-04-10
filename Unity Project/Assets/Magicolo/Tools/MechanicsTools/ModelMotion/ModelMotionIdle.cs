using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelMotionIdle : State {
	
    ModelMotion Layer {
    	get { return ((ModelMotion)layer); }
    }
    
    StateMachine Machine {
    	get { return ((StateMachine)machine); }
    }
	
	public override void OnEnter() {
		base.OnEnter();
		
	}
	
	public override void OnExit() {
		base.OnExit();
		
	}
	
	public override void OnUpdate() {
		base.OnUpdate();
		
	}
	
	public override void OnFixedUpdate() {
		base.OnFixedUpdate();
		
	}
}
