using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class RigidbodyExtensions {

		#region Velocity
		public static void SetVelocity(this Rigidbody rigidbody, Vector3 velocity, Axis axis = Axis.XYZ) {
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axis);
		}
		
		public static void SetVelocity(this Rigidbody rigidbody, float velocity, Axis axis = Axis.XYZ) {
			rigidbody.SetVelocity(new Vector3(velocity, velocity, velocity), axis);
		}
		
		public static void Accelerate(this Rigidbody rigidbody, Vector3 acceleration, Axis axis = Axis.XYZ) {
			rigidbody.SetVelocity(rigidbody.velocity + ((rigidbody.velocity + acceleration * Time.fixedDeltaTime) - rigidbody.velocity), axis);
		}
		
		public static void Accelerate(this Rigidbody rigidbody, float acceleration, Axis axis = Axis.XYZ) {
			rigidbody.Accelerate(new Vector3(acceleration, acceleration, acceleration), axis);
		}
		
		public static void AccelerateTowards(this Rigidbody rigidbody, Vector3 targetAcceleration, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetVelocity(rigidbody.velocity + (rigidbody.velocity.Lerp(targetAcceleration, Time.fixedDeltaTime * speed, axis) - rigidbody.velocity), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetVelocity(rigidbody.velocity + (rigidbody.velocity.LerpLinear(targetAcceleration, Time.fixedDeltaTime * speed, axis) - rigidbody.velocity), axis);
					break;
			}
		}
		
		public static void AccelerateTowards(this Rigidbody rigidbody, Vector3 targetAcceleration, float speed, Axis axis = Axis.XYZ) {
			rigidbody.AccelerateTowards(targetAcceleration, speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void AccelerateTowards(this Rigidbody rigidbody, float targetAcceleration, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			rigidbody.AccelerateTowards(new Vector3(targetAcceleration, targetAcceleration, targetAcceleration), speed, interpolation, axis);
		}
		
		public static void AccelerateTowards(this Rigidbody rigidbody, float targetAcceleration, float speed, Axis axis = Axis.XYZ) {
			rigidbody.AccelerateTowards(new Vector3(targetAcceleration, targetAcceleration, targetAcceleration), speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, Axis axis = Axis.XYZ) {
			rigidbody.SetVelocity(rigidbody.velocity.Oscillate(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axis), axis);
		}

		public static void OscillateVelocity(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Axis axis = Axis.XYZ) {
			OscillateVelocity(rigidbody, frequency, amplitude, Vector3.zero, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody rigidbody, float frequency, float amplitude, Axis axis = Axis.XYZ) {
			OscillateVelocity(rigidbody, new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody rigidbody, float frequency, float amplitude, float center, Axis axis = Axis.XYZ) {
			OscillateVelocity(rigidbody, new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		#endregion
		
		#region Position
		public static void SetPosition(this Rigidbody rigidbody, Vector3 position, Axis axis = Axis.XYZ) {
			rigidbody.MovePosition(rigidbody.transform.position.SetValues(position, axis));
		}
		
		public static void SetPosition(this Rigidbody rigidbody, float position, Axis axis = Axis.XYZ) {
			rigidbody.SetPosition(new Vector3(position, position, position), axis);
		}
		
		public static void Translate(this Rigidbody rigidbody, Vector3 translation, Axis axis = Axis.XYZ) {
			rigidbody.SetPosition(rigidbody.transform.position + translation * Time.fixedDeltaTime, axis);
		}
		
		public static void Translate(this Rigidbody rigidbody, float translation, Axis axis = Axis.XYZ) {
			rigidbody.Translate(new Vector3(translation, translation, translation), axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, Axis axis = Axis.XYZ) {
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, Axis axis = Axis.XYZ) {
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, Axis axis = Axis.XYZ) {
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Axis axis = Axis.XYZ) {
			rigidbody.OscillatePosition(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillatePosition(this Rigidbody rigidbody, float frequency, float amplitude, float center, Axis axis = Axis.XYZ) {
			rigidbody.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, float frequency, float amplitude, Axis axis = Axis.XYZ) {
			rigidbody.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		#endregion
		
		#region Rotation
		public static void SetEulerAngles(this Rigidbody rigidbody, Vector3 angles, Axis axis = Axis.XYZ) {
			rigidbody.MoveRotation(Quaternion.Euler(rigidbody.transform.eulerAngles.SetValues(angles, axis)));
		}
		
		public static void SetEulerAngles(this Rigidbody rigidbody, float angle, Axis axis = Axis.XYZ) {
			rigidbody.SetEulerAngles(new Vector3(angle, angle, angle), axis);
		}
		
		public static void Rotate(this Rigidbody rigidbody, Vector3 rotation, Axis axis = Axis.XYZ) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles + rotation * Time.fixedDeltaTime, axis);
		}
		
		public static void Rotate(this Rigidbody rigidbody, float rotation, Axis axis = Axis.XYZ) {
			rigidbody.Rotate(new Vector3(rotation, rotation, rotation), axis);
		}
			
		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAngles(targetAngles, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAnglesLinear(targetAngles, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, Axis axis = Axis.XYZ) {
			rigidbody.RotateTowards(targetAngles, speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, InterpolationModes interpolation, Axis axis = Axis.XYZ) {
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, interpolation, axis);
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, Axis axis = Axis.XYZ) {
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, InterpolationModes.Quadratic, axis);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, Axis axis = Axis.XYZ) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.Oscillate(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateEulerAngles(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Axis axis = Axis.XYZ) {
			rigidbody.OscillateEulerAngles(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, float frequency, float amplitude, float center, Axis axis = Axis.XYZ) {
			rigidbody.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateEulerAngles(this Rigidbody rigidbody, float frequency, float amplitude, Axis axis = Axis.XYZ) {
			rigidbody.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		#endregion
	}
}

