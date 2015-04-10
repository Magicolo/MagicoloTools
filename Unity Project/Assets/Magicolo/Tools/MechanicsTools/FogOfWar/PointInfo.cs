using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	public class PointInfo {

		public int x;
		public int y;
		public int coordinateX;
		public int coordinateY;
		public float distance;
		
		public PointInfo(int x, int y, int amount) {
			this.x = x - amount;
			this.y = y - amount;
			this.coordinateX = x;
			this.coordinateY = y;
			this.distance = Mathf.Sqrt(this.x * this.x + this.y * this.y);
		}
		
		public override string ToString() {
			return string.Format("{0}(({1}, {2}), {3}, ({4}, {5}))", GetType().Name.Split('.').Last(), x, y, distance, coordinateX, coordinateY);
		}
	}
}