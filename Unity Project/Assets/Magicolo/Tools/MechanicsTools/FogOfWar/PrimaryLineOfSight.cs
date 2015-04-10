using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	public class PrimaryLineOfSight {

		public LineOfSightInfo info;
		public int x;
		public int y;
		public float radius;
		public float strength;
		public float falloff;
		public bool inverted;
		public float[,] alphaMap;
		public float[,] heightMap;
		
		public float alpha;
		public float halfRadius;
		public float eighthRadius;
		public int width;
		public int height;
		public List<LineOfSightInfo>[,] lineInfos;
		public int childrenCount;
		
		public PrimaryLineOfSight(LineOfSightInfo info, int x, int y, float radius, float strength, float falloff, bool inverted, float[,] alphaMap, float[,] heightMap, List<LineOfSightInfo>[,] lineInfos) {
			this.info = info;
			this.x = x;
			this.y = y;
			this.radius = radius;
			this.halfRadius = radius / 2;
			this.eighthRadius = radius / 8;
			this.strength = strength;
			this.falloff = falloff;
			this.inverted = inverted;
			this.alphaMap = alphaMap;
			this.heightMap = heightMap;
			this.lineInfos = lineInfos;
			this.alpha = strength;
			
			width = alphaMap.GetLength(0);
			height = alphaMap.GetLength(1);
		}
		
		public void Progress() {
			PointInfo point = info.GetNextPoint();
			
			x += info.directionX;
			y += info.directionY;
			
			if (alpha > 0) {
				if (point.distance > halfRadius) {
					alpha *= falloff - (point.distance - halfRadius) / halfRadius;
				}
				else if (point.distance > eighthRadius) {
					alpha *= 0.99F;
				}
			}
			
			if (x >= 0 && x < width && y >= 0 && y < height) {
				alpha *= 1 - heightMap[x, y];
				alphaMap[x, y] += inverted ? 1 - alpha : alpha;
			}
			
			List<LineOfSightInfo> childInfos = lineInfos[point.coordinateX, point.coordinateY];
			new SecondaryLineOfSight(childInfos[0], this).Complete();
			new SecondaryLineOfSight(childInfos[1], this).Complete();
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