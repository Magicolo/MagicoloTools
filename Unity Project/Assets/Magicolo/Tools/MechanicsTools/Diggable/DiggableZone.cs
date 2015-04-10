using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.MechanicsTools {
	[System.Serializable]
	public class DiggableZone {

		public Rect rect;
		public Diggable diggable;
		public GameObject gameObject;
		public BoxCollider boxCollider;
		public CapsuleCollider capsuleCollider;
		public bool smallestZone;
	
		public Vector2 position {
			get {
				return rect.position;
			}
		}
	
		public Vector2 size {
			get {
				return rect.size;
			}
		}
	
		public DiggableZone(Vector2 coordinates, Vector2 size, Diggable diggable) {
			this.rect = new Rect(coordinates.x, coordinates.y, size.x, size.y);
			this.diggable = diggable;
		
			diggable.zoneManager.SetZones(rect, this);
			smallestZone = size.x <= 1 && size.y <= 1;
			SetCollider();
		}
	
		public void Update() {
			if (size.x > 64 || size.y > 64) {
				Break();
				return;
			}
		
			for (int y = 0; y < size.y; y++) {
				for (int x = 0; x < size.x; x++) {
					Color pixel = diggable.GetPixel(position.x + x, position.y + y);
				
					if (pixel.a < diggable.alphaThreshold) {
						Break();
						return;
					}
				}
			}
		}
	
		public void Break() {
			if (smallestZone) {
				if (Application.isPlaying) {
					diggable.SpawnFX(position + size / 2);
				}
			
				diggable.zoneManager.SetZone(position, null);
			}
			else {
				DiggableZone[] childZones = {
					new DiggableZone(position, size / 2, diggable),
					new DiggableZone(position + new Vector2(size.x / 2, 0), size / 2, diggable),
					new DiggableZone(position + new Vector2(0, size.y / 2), size / 2, diggable),
					new DiggableZone(position + size / 2, size / 2, diggable)
				};
		
				foreach (DiggableZone childZone in childZones) {
					childZone.Update();
				}
			}
		
			if (gameObject != null) {
				gameObject.Remove();
			}
		}
	
		void SetCollider() {
			if (smallestZone) {
				gameObject = diggable.SpawnCapsuleCollider(rect);
			}
			else {
				gameObject = diggable.SpawnBoxCollider(rect);
			}
		}
	}
}
