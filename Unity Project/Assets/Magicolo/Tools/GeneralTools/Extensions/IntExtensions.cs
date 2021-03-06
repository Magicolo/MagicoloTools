﻿using UnityEngine;

namespace Magicolo {
	public static class IntExtensions {

		public static float PowSign(this int i, double power) {
			return Mathf.Pow(Mathf.Abs(i), (float)power) * i.Sign();
		}
	
		public static float PowSign(this int i) {
			return i.PowSign(2);
		}
	
		public static float Pow(this int i, double power) {
			return Mathf.Pow(i, (float)power);
		}
	
		public static float Pow(this int i) {
			return i.Pow(2);
		}
	
		public static int Round(this int i, double step) {
			if (step <= 0) {
				return i;
			}
		
			return (int)(Mathf.Round((float)(i * (1D / step))) / (1D / step));
		}
	
		public static int Round(this int i) {
			return i.Round(1);
		}
		
		public static int Sign(this int i) {
			return i < 0 ? -1 : 1;
		}
	}
}
