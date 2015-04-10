using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class FogAgent {
	
	[SerializeField, PropertyField]
	Transform transform;
	public Transform Transform {
		get {
			return transform;
		}
		set {
			transform = value;
			hasChanged = true;
		}
	}
	
	[SerializeField, PropertyField]
	Vector3 offset;
	public Vector3 Offset {
		get {
			return offset;
		}
		set {
			offset = value;
			hasChanged = true;
		}
	}
	
	
	[SerializeField, PropertyField(typeof(MinAttribute))]
	float sightRadius = 5;
	public float SightRadius {
		get {
			return sightRadius;
		}
		set {
			sightRadius = value;
			hasChanged = true;
		}
	}
	
	[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
	float strength = 1;
	public float Strength {
		get {
			return strength;
		}
		set {
			strength = Mathf.Clamp(value, 0, 1);
			hasChanged = true;
		}
	}
	
	[SerializeField, PropertyField(typeof(RangeAttribute), 0.25F, 1)]
	float falloff = 1;
	public float Falloff {
		get {
			return falloff;
		}
		set {
			falloff = Mathf.Clamp(value, 0.25F, 1);
			hasChanged = true;
		}
	}
	
	[SerializeField, PropertyField]
	bool inverted;
	public bool Inverted {
		get {
			return inverted;
		}
		set {
			inverted = value;
		}
	}
		
	Vector3 position;
	public Vector3 Position {
		get {
			return position;
		}
	}
	
	Rect rect;
	public Rect Rect {
		get {
			return rect;
		}
	}
	
	bool hasChanged = true;
	
	public FogAgent(Transform transform) {
		Transform = transform;
	}
	
	public FogAgent(Transform transform, Vector3 offset, float sightRadius, float strength, float falloff, bool inverted) {
		Transform = transform;
		Offset = offset;
		SightRadius = sightRadius;
		Strength = strength;
		Falloff = falloff;
		Inverted = inverted;
	}
	
	public void Update() {
		Vector3 lastPosition = position;
		position = transform.position + offset;
		hasChanged |= position != lastPosition;
		rect = new Rect(position.x - SightRadius, position.y - SightRadius, SightRadius * 2, SightRadius * 2);
	}
	
	public bool HasChanged() {
		bool changed = hasChanged;
		hasChanged = false;
		
		return changed;
	}
}

