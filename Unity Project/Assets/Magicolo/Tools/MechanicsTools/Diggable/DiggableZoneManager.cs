using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	public class DiggableZoneManager : MonoBehaviourExtended {

		[HideInInspector] public DiggableZone[] zones;
		public Diggable diggable;
	
		public void Initialize(Diggable diggable) {
			this.diggable = diggable;
		
			zones = new DiggableZone[diggable.width * diggable.height];
			DiggableZone originalZone = new DiggableZone(Vector2.zero, new Vector2(diggable.width, diggable.height), diggable);
			originalZone.Update();
		}
	
		public DiggableZone GetZone(Vector2 coordinates) {
			return GetZone((int)coordinates.x, (int)coordinates.y);
		}
	
		public DiggableZone GetZone(float x, float y) {
			return GetZone((int)x, (int)y);
		}
	
		public DiggableZone GetZone(int x, int y) {
			DiggableZone zone = null;
		
			try {
				zone = zones[y * diggable.width + x];
			}
			catch {
				Logger.LogError(string.Format("Zone at coordinates [{0}, {1}] was not found.", x, y));
			}
		
			return zone;
		}
	
		public void SetZone(int x, int y, DiggableZone zone) {
			zones[y * diggable.width + x] = zone;
		}
	
		public void SetZone(Vector2 coordinates, DiggableZone zone) {
			SetZone((int)coordinates.x, (int)coordinates.y, zone);
		}
	
		public void SetZones(Rect rect, DiggableZone zone) {
			SetZones(rect.position, rect.size, zone);
		}
	
		public void SetZones(Vector2 coordinates, Vector2 size, DiggableZone zone) {
			for (int y = 0; y < size.y; y++) {
				for (int x = 0; x < size.x; x++) {
					SetZone(new Vector2(coordinates.x + x, coordinates.y + y), zone);
				}
			}
		}
	}
}