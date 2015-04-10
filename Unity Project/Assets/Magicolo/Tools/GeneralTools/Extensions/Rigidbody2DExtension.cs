using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	public static class Rigidbody2DExtension {

		#region Velocity
		public static void SetVelocity(this Rigidbody2D rigidbody, Vector2 velocity, Axis axis = Axis.XY) {
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axis);
		}
		
		public static void SetVelocity(this Rigidbody2D rigidbody, float velocity, Axis axis = Axis.XY) {
			rigidbody.SetVelocity(new Vector2(velocity, velocity), axis);
		}
		
		public static void Accelerate(this Rigidbody2D rigidbody, Vector2 acceleration, Axis axis = Axis.XY) {
			rigidbody.SetVelocity(rigidbody.velocity + ((rigidbody.velocity + acceleration * Time.fixedDeltaTime) - rigidbody.velocity), axis);
		}
		
		public static void Accelerate(this Rigidbody2D rigidbody, float acceleration, Axis axis = Axis.XY) {
			rigidbody.Accelerate(new Vector2(acceleration, acceleration), axis);
		}
		
		public static void AccelerateTowards(this Rigidbody2D rigidbody, Vector2 targetAcceleration, float speed, InterpolationModes interpolation, Axis axis = Axis.XY) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetVelocity(rigidbody.velocity + (rigidbody.velocity.Lerp(targetAcceleration, Time.fixedDeltaTime * speed, axis) - rigidbody.velocity), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetVelocity(rigidbody.velocity + (rigidbody.velocity.LerpLinear(targetAcceleration, Time.fixedDeltaTime * speed, axis) - rigidbody.velocity), axis);
					break;
			}
		}
		
		public static void AccelerateTowards(this Rigidbody2D rigidbody, Vector2 targetAcceleration, float speed, Axis axis = Axis.XY) {
			rigidbody.AccelerateTowards(targetAcceleration, speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void AccelerateTowards(this Rigidbody2D rigidbody, float targetAcceleration, float speed, InterpolationModes interpolation, Axis axis = Axis.XY) {
			rigidbody.AccelerateTowards(new Vector2(targetAcceleration, targetAcceleration), speed, interpolation, axis);
		}
		
		public static void AccelerateTowards(this Rigidbody2D rigidbody, float targetAcceleration, float speed, Axis axis = Axis.XY) {
			rigidbody.AccelerateTowards(new Vector2(targetAcceleration, targetAcceleration), speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, Axis axis = Axis.XY) {
			rigidbody.SetVelocity(rigidbody.velocity.Oscillate(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axis), axis);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Axis axis = Axis.XY) {
			OscillateVelocity(rigidbody, frequency, amplitude, Vector2.zero, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody2D rigidbody, float frequency, float amplitude, Axis axis = Axis.XY) {
			OscillateVelocity(rigidbody, new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), Vector2.zero, axis);
		}
		
		public static void OscillateVelocity(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, Axis axis = Axis.XY) {
			OscillateVelocity(rigidbody, new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), axis);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody2D rigidbody, Vector2 position, Axis axis = Axis.XY) {
			rigidbody.MovePosition(rigidbody.transform.position.SetValues((Vector3)position, axis));
		}
		
		public static void SetPosition(this Rigidbody2D rigidbody, float position, Axis axis = Axis.XY) {
			rigidbody.SetPosition(new Vector2(position, position), axis);
		}
		
		public static void Translate(this Rigidbody2D rigidbody, Vector2 translation, Axis axis = Axis.XY) {
			rigidbody.SetPosition(rigidbody.transform.position + (Vector3)translation * Time.fixedDeltaTime, axis);
		}
		
		public static void Translate(this Rigidbody2D rigidbody, float translation, Axis axis = Axis.XY) {
			rigidbody.Translate(new Vector2(translation, translation), axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, InterpolationModes interpolation, Axis axis = Axis.XY) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp((Vector3)targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear((Vector3)targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, Axis axis = Axis.XY) {
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, InterpolationModes interpolation, Axis axis = Axis.XY) {
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, Axis axis = Axis.XY) {
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, InterpolationModes.Quadratic, axis);
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, Axis axis = Axis.XY) {
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate((Vector3)frequency, (Vector3)amplitude, (Vector3)center, rigidbody.transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Axis axis = Axis.XY) {
			rigidbody.OscillatePosition(frequency, amplitude, Vector2.zero, axis);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, Axis axis = Axis.XY) {
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), axis);
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, Axis axis = Axis.XY) {
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), Vector2.zero, axis);
		}
		#endregion
		
		#region Rotation
		public static void SetEulerAngles(this Rigidbody2D rigidbody, float angle) {
			rigidbody.MoveRotation(angle);
		}
		
		public static void Rotate(this Rigidbody2D rigidbody, float rotation) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.z + rotation * Time.fixedDeltaTime);
		}
			
		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed, InterpolationModes interpolation) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAngles(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, Axis.Z).z);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAnglesLinear(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, Axis.Z).z);
					break;
			}
		}
		
		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed) {
			rigidbody.RotateTowards(targetAngle, speed, InterpolationModes.Quadratic);
		}

		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude, float center) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.Oscillate(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), rigidbody.GetInstanceID() / 1000, Axis.Z).z);
		}
		
		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude) {
			rigidbody.OscillateEulerAngles(frequency, amplitude, 0);
		}
		#endregion
	}
}