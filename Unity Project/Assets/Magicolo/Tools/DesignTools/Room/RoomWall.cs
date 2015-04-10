using UnityEngine;
using System.Collections;

namespace Magicolo.DesignTools {
	[System.Serializable]
	public class RoomWall {

		[SerializeField, PropertyField]
		Material material;
		public Material Material {
			get {
				return material;
			}
			set {
				material = value;
				UpdateMaterials();
			}
		}
				
		[SerializeField, PropertyField]
		bool door;
		public bool Door {
			get {
				return door;
			}
			set {
				door = value;
				UpdateDimensions();
			}
		}

		[SerializeField, PropertyField(typeof(ClampAttribute))]
		Vector2 doorPosition = new Vector2(0.5F, 0);
		public Vector2 DoorPosition {
			get {
				return doorPosition;
			}
			set {
				doorPosition = value;
				UpdateDimensions();
			}
		}

		[SerializeField, PropertyField(typeof(ClampAttribute))]
		Vector2 doorSize = new Vector2(0.1F, 0.4F);
		public Vector2 DoorSize {
			get {
				return doorSize;
			}
			set {
				doorSize = value;
				UpdateDimensions();
			}
		}
		
		[HideInInspector] public Room room;
		[HideInInspector] public Transform wall;
		
		[HideInInspector] public Transform doorLeft;
		[HideInInspector] public MeshRenderer doorLeftRenderer;
		
		[HideInInspector] public Transform doorRight;
		[HideInInspector] public MeshRenderer doorRightRenderer;
		
		[HideInInspector] public Transform doorAbove;
		[HideInInspector] public MeshRenderer doorAboveRenderer;
		
		[HideInInspector] public Transform doorUnder;
		[HideInInspector] public MeshRenderer doorUnderRenderer;
		
		public RoomWall(string name, Room room) {
			this.room = room;
			this.wall = room.FindOrAddChild(name).transform;
			
			this.doorLeft = wall.FindOrAddChild("Door Left", PrimitiveType.Cube).transform;
			this.doorLeftRenderer = doorLeft.GetComponent<MeshRenderer>();
			
			this.doorRight = wall.FindOrAddChild("Door Right", PrimitiveType.Cube).transform;
			this.doorRightRenderer = doorRight.GetComponent<MeshRenderer>();
			
			this.doorAbove = wall.FindOrAddChild("Door Above", PrimitiveType.Cube).transform;
			this.doorAboveRenderer = doorAbove.GetComponent<MeshRenderer>();
			
			this.doorUnder = wall.FindOrAddChild("Door Under", PrimitiveType.Cube).transform;
			this.doorUnderRenderer = doorUnder.GetComponent<MeshRenderer>();
			
			this.material = doorLeftRenderer.sharedMaterial;
			UpdateMaterials();
			UpdateDimensions();
		}

		void UpdateDimensions() {
			if (door) {
				doorPosition.Set(Mathf.Clamp(doorPosition.x, doorSize.x / 2, 1 - doorSize.x / 2), Mathf.Clamp(doorPosition.y, 0, 1 - doorSize.y));
			
				doorLeft.localPosition = new Vector3((doorPosition.x - doorSize.x / 2) / 2 - 0.5F, 0, 0);
				doorLeft.localScale = new Vector3((doorPosition.x - doorSize.x / 2), 1, 1);
			
				doorRight.localPosition = new Vector3((doorPosition.x + doorSize.x / 2) / 2, 0, 0);
				doorRight.localScale = new Vector3(1 - (doorPosition.x + doorSize.x / 2), 1, 1);
			
				doorAbove.localPosition = new Vector3(doorPosition.x - 0.5F, (doorPosition.y - doorSize.y) / 2 + doorSize.y, 0);
				doorAbove.localScale = new Vector3(doorSize.x, 1 - (doorPosition.y + doorSize.y), 1);
			
				doorUnder.localPosition = new Vector3(doorPosition.x - 0.5F, doorPosition.y / 2 - 0.5F, 0);
				doorUnder.localScale = new Vector3(doorSize.x, doorPosition.y, 1);
			}
			else {
				doorLeft.localPosition = new Vector3(0, 0, 0);
				doorLeft.localScale = new Vector3(1, 1, 1);
			}
			
			doorLeft.gameObject.SetActive(doorLeft.lossyScale.x != 0 && doorLeft.lossyScale.y != 0 && doorLeft.lossyScale.z != 0);
			doorRight.gameObject.SetActive(door && doorRight.lossyScale.x != 0 && doorRight.lossyScale.y != 0 && doorRight.lossyScale.z != 0);
			doorAbove.gameObject.SetActive(door && doorAbove.lossyScale.x != 0 && doorAbove.lossyScale.y != 0 && doorAbove.lossyScale.z != 0);
			doorUnder.gameObject.SetActive(door && doorUnder.lossyScale.x != 0 && doorUnder.lossyScale.y != 0 && doorUnder.lossyScale.z != 0);
		}
		
		void UpdateMaterials() {
			doorLeftRenderer.sharedMaterial = material;
			doorRightRenderer.sharedMaterial = material;
			doorAboveRenderer.sharedMaterial = material;
			doorUnderRenderer.sharedMaterial = material;
		}
	}
}
