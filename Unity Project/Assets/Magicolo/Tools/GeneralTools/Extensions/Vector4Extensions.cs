using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector4Extensions {
	
		const float epsilon = 0.001F;
		
		public static Vector4 SetValues(this Vector4 vector, Vector4 values, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? values.x : vector.x;
			vector.y = axis.Contains(Axis.Y) ? values.y : vector.y;
			vector.z = axis.Contains(Axis.Z) ? values.z : vector.z;
			vector.w = axis.Contains(Axis.W) ? values.w : vector.w;
			
			return vector;
		}
		
		public static Vector4 SetValues(this Vector4 vector, Vector4 values) {
			return vector.SetValues(values, Axis.XYZW);
		}
				
		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time, Axis axis) {
			vector.x = axis.Contains(Axis.X) && Mathf.Abs(target.x - vector.x) > epsilon ? Mathf.Lerp(vector.x, target.x, time) : vector.x;
			vector.y = axis.Contains(Axis.Y) && Mathf.Abs(target.y - vector.y) > epsilon ? Mathf.Lerp(vector.y, target.y, time) : vector.y;
			vector.z = axis.Contains(Axis.Z) && Mathf.Abs(target.z - vector.z) > epsilon ? Mathf.Lerp(vector.z, target.z, time) : vector.z;
			vector.w = axis.Contains(Axis.W) && Mathf.Abs(target.w - vector.w) > epsilon ? Mathf.Lerp(vector.w, target.w, time) : vector.w;
			
			return vector;
		}
			
		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time) {
			return vector.Lerp(target, time, Axis.XYZW);
		}
		
		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time, Axis axis) {
			Vector4 difference = target - vector;
			Vector4 direction = Vector4.zero.SetValues(difference, axis);
			float distance = direction.magnitude;
					
			Vector4 adjustedDirection = direction.normalized * time;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector4.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(target, axis);
			}
			
			return vector;
		}
		
		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time) {
			return vector.LerpLinear(target, time, Axis.XYZW);
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time, Axis axis) {
			vector.x = axis.Contains(Axis.X) && Mathf.Abs(targetAngles.x - vector.x) > epsilon ? Mathf.LerpAngle(vector.x, targetAngles.x, time) : vector.x;
			vector.y = axis.Contains(Axis.Y) && Mathf.Abs(targetAngles.y - vector.y) > epsilon ? Mathf.LerpAngle(vector.y, targetAngles.y, time) : vector.y;
			vector.z = axis.Contains(Axis.Z) && Mathf.Abs(targetAngles.z - vector.z) > epsilon ? Mathf.LerpAngle(vector.z, targetAngles.z, time) : vector.z;
			vector.w = axis.Contains(Axis.W) && Mathf.Abs(targetAngles.w - vector.w) > epsilon ? Mathf.LerpAngle(vector.w, targetAngles.w, time) : vector.w;
			
			return vector;
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time) {
			return vector.LerpAngles(targetAngles, time, Axis.XYZW);
		}

		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time, Axis axis) {
			Vector4 difference = new Vector4(Mathf.DeltaAngle(vector.x, targetAngles.x), Mathf.DeltaAngle(vector.y, targetAngles.y), Mathf.DeltaAngle(vector.z, targetAngles.z), Mathf.DeltaAngle(vector.w, targetAngles.w));
			Vector4 direction = Vector4.zero.SetValues(difference, axis);
			float distance = direction.magnitude * Mathf.Rad2Deg;
					
			Vector4 adjustedDirection = direction.normalized * time;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector4.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(targetAngles, axis);
			}
			
			return vector;
		}
		
		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time) {
			return vector.LerpAnglesLinear(targetAngles, time, Axis.XYZW);
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset) : vector.x;
			vector.y = axis.Contains(Axis.Y) ? center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset) : vector.y;
			vector.z = axis.Contains(Axis.Z) ? center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset) : vector.z;
			vector.w = axis.Contains(Axis.W) ? center.w + amplitude.w * Mathf.Sin(frequency.w * Time.time + offset) : vector.w;
			
			return vector;
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset) {
			return vector.Oscillate(frequency, amplitude, center, offset, Axis.XYZW);
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, Axis axis) {
			return vector.Oscillate(frequency, amplitude, center, 0, axis);
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center) {
			return vector.Oscillate(frequency, amplitude, center, 0, Axis.XYZW);
		}

		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? vector.x * otherVector.x : vector.x;
			vector.y = axis.Contains(Axis.Y) ? vector.y * otherVector.y : vector.y;
			vector.z = axis.Contains(Axis.Z) ? vector.z * otherVector.z : vector.z;
			vector.w = axis.Contains(Axis.W) ? vector.w * otherVector.w : vector.w;
			
			return vector;
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector) {
			return vector.Mult(otherVector, Axis.XYZW);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector, Axis axis) {
			return vector.Mult((Vector4)otherVector, axis);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector) {
			return vector.Mult((Vector4)otherVector, Axis.XYZW);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector, Axis axis) {
			return vector.Mult((Vector4)otherVector, axis);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector) {
			return vector.Mult((Vector4)otherVector, Axis.XYZW);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector4 otherVector, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? vector.x / otherVector.x : vector.x;
			vector.y = axis.Contains(Axis.Y) ? vector.y / otherVector.y : vector.y;
			vector.z = axis.Contains(Axis.Z) ? vector.z / otherVector.z : vector.z;
			vector.w = axis.Contains(Axis.W) ? vector.w / otherVector.w : vector.w;
			
			return vector;
		}
	
		public static Vector4 Div(this Vector4 vector, Vector4 otherVector) {
			return vector.Div(otherVector, Axis.XYZW);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector2 otherVector, Axis axis) {
			return vector.Div((Vector4)otherVector, axis);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector2 otherVector) {
			return vector.Div((Vector4)otherVector, Axis.XYZW);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector3 otherVector, Axis axis) {
			return vector.Div((Vector4)otherVector, axis);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector3 otherVector) {
			return vector.Div((Vector4)otherVector, Axis.XYZW);
		}
	
		public static Vector4 Pow(this Vector4 vector, double power, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? vector.x.Pow(power) : vector.x;
			vector.y = axis.Contains(Axis.Y) ? vector.y.Pow(power) : vector.y;
			vector.z = axis.Contains(Axis.Z) ? vector.z.Pow(power) : vector.z;
			vector.w = axis.Contains(Axis.W) ? vector.w.Pow(power) : vector.w;
			
			return vector;
		}
	
		public static Vector4 Pow(this Vector4 vector, double power) {
			return vector.Pow(power, Axis.XYZW);
		}
	
		public static Vector4 Round(this Vector4 vector, double step, Axis axis) {
			vector.x = axis.Contains(Axis.X) ? vector.x.Round(step) : vector.x;
			vector.y = axis.Contains(Axis.Y) ? vector.y.Round(step) : vector.y;
			vector.z = axis.Contains(Axis.Z) ? vector.z.Round(step) : vector.z;
			vector.w = axis.Contains(Axis.W) ? vector.w.Round(step) : vector.w;
			
			return vector;
		}
	
		public static Vector4 Round(this Vector4 vector, double step) {
			return vector.Round(step, Axis.XYZW);
		}
	
		public static Vector4 Round(this Vector4 vector) {
			return vector.Round(1, Axis.XYZW);
		}
	
		public static float Average(this Vector4 vector, Axis axis) {
			float average = 0;
			int axisCount = 0;
		
			if (axis.Contains(Axis.X)) {
				average += vector.x;
				axisCount += 1;
			}
		
			if (axis.Contains(Axis.Y)) {
				average += vector.y;
				axisCount += 1;
			}
		
			if (axis.Contains(Axis.Z)) {
				average += vector.z;
				axisCount += 1;
			}
		
			if (axis.Contains(Axis.W)) {
				average += vector.w;
				axisCount += 1;
			}
		
			return average / axisCount;
		}
	
		public static float Average(this Vector4 vector) {
			return ((Vector4)vector).Average(Axis.XYZW);
		}
	}
}
