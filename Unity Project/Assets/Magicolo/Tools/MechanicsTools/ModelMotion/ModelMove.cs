using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ModelMove : StateLayer {
	
	[Min] public float moveThreshold;
	[Min] public float inputPower = 1;
	
	[SerializeField, Disable] float horizontalAxis;
	public float HorizontalAxis {
		get {
			return horizontalAxis;
		}
		set {
			if (horizontalAxis != value) {
				horizontalAxis = value;
				animator.SetFloat(absHorizontalAxisHash, AbsHorizontalAxis);
			}
		}
	}
	
	public float AbsHorizontalAxis {
		get {
			return Mathf.Abs(horizontalAxis);
		}
	}
	
	#region Hashes
	[Disable] public int absHorizontalAxisHash = Animator.StringToHash("AbsHorizontalAxis");
	[Disable] public int jumpHash = Animator.StringToHash("Jump");
	[Disable] public int groundedHash = Animator.StringToHash("Grounded");
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

	public override void OnUpdate() {
		base.OnUpdate();
		
		HorizontalAxis = Input.GetAxis("Horizontal").PowSign(inputPower);
		
		transform.SetPosition(0, Axis.Z);
	}
}
