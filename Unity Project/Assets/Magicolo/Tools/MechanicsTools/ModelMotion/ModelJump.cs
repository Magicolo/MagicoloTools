using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelJump : StateLayer {
	
	public KeyCode jumpKey1 = KeyCode.UpArrow;
	public KeyCode jumpKey2 = KeyCode.JoystickButton0;
	public GroundCastSettings raySettings;
	
	[SerializeField, Disable] bool grounded;
	public bool Grounded {
		get {
			return grounded;
		}
		set {
			if (grounded != value) {
				grounded = value;
				animator.SetBool(groundedHash, grounded);
			}
		}
	}

	[SerializeField, Disable] float verticalVelocity;
	public float VerticalVelocity {
		get {
			return verticalVelocity;
		}
		set {
			if (verticalVelocity != value) {
				verticalVelocity = value;
				animator.SetFloat(verticalVelocityHash, verticalVelocity);
			}
		}
	}
	
	#region Hashes
	[Disable] public int groundedHash = Animator.StringToHash("Grounded");
	[Disable] public int jumpingHash = Animator.StringToHash("Jumping");
	[Disable] public int verticalVelocityHash = Animator.StringToHash("VerticalVelocity");
	#endregion
	
	#region Cached Components
	bool _animatorCached;
	Animator _animator;
	public Animator animator { 
		get { 
			_animator = _animatorCached ? _animator : GetComponent<Animator>();
			_animatorCached = true;
			return _animator;
		}
	}
	
	bool _rigidbodyCached;
	Rigidbody _rigidbody;
	new public Rigidbody rigidbody { 
		get { 
			_rigidbody = _rigidbodyCached ? _rigidbody : GetComponent<Rigidbody>();
			_rigidbodyCached = true;
			return _rigidbody;
		}
	}
	#endregion
		
    StateMachine Machine {
    	get { return ((StateMachine)machine); }
    }
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		Grounded = raySettings.HasHit(transform.position, -transform.up, Machine.Debug);
		VerticalVelocity = rigidbody.velocity.y;
	}
}
