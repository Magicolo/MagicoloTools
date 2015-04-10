using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	public class SecondaryLineOfSight {

		public LineOfSightInfo info;
		public PrimaryLineOfSight parent;
		public int x;
		public int y;
		
		public float alpha;

		public SecondaryLineOfSight(LineOfSightInfo info, PrimaryLineOfSight parent) {
			this.info = info;
			this.parent = parent;
			this.alpha = parent.alpha;
			
			this.x = parent.x;
			this.y = parent.y;
		}

		public void Progress() {
			PointInfo point = info.GetNextPoint();
			
			x += info.directionX;
			y += info.directionY;
			
			if (alpha > 0) {
				if (point.distance > parent.halfRadius) {
					alpha *= parent.falloff - (point.distance - parent.halfRadius) / parent.halfRadius;
				}
				else if (point.distance > parent.eighthRadius) {
					alpha *= 0.99F;
				}
			}
			
			if (x >= 0 && x < parent.width && y >= 0 && y < parent.height) {
				alpha *= 1 - parent.heightMap[x, y];
				parent.alphaMap[x, y] += parent.inverted ? (1 - alpha) / 2 : alpha / 2;
			}
		}

		public void Complete() {
			info.Reset();
			
			while (alpha > 0) {
				Progress();
			}
			
			info.Reset();
		}
	}
}