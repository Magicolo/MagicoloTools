using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	public class LineOfSightInfo {

		public int x;
		public int y;
		public int directionX;
		public int directionY;
		public PointInfo[] points;
		
		int counter;

		public LineOfSightInfo(int x, int y, int directionX, int directionY) {
			this.x = x;
			this.y = y;
			this.directionX = directionX;
			this.directionY = directionY;
		}
		
		public void GeneratePoints(int amount) {
			points = new PointInfo[amount];
			points[0] = new PointInfo(x, y, amount);
		}
		
		public PointInfo GetCurrentPoint() {
			return points[counter];
		}
		
		public PointInfo GetNextPoint() {
			counter += 1;
			
			PointInfo point = points[counter];
			
			if (point == null) {
				PointInfo previousPoint = points[counter - 1];
				points[counter] = point = new PointInfo(previousPoint.coordinateX + directionX, previousPoint.coordinateY + directionY, points.Length);
			}
			
			return point;
		}
		
		public void Reset() {
			counter = 0;
		}
		
		public override string ToString() {
			return string.Format("{0}(({1}, {2}), ({3}, {4}), {5})", GetType().Name.Split('.').Last(), x, y, directionX, directionY, points.Length);
		}
	}
}