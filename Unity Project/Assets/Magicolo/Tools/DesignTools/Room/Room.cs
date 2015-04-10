using UnityEngine;
using System.Collections;
using Magicolo.DesignTools;

namespace Magicolo {
	[System.Serializable]
	[ExecuteInEditMode, AddComponentMenu("Magicolo/Design/Room")]
	public class Room : MonoBehaviour {

		[SerializeField, PropertyField(typeof(MinAttribute))]
		Vector3 dimensions = new Vector3(50, 20, 50);
		public Vector3 Dimensions {
			get {
				return dimensions;
			}
			set {
				dimensions = value;
				UpdateDimensions();
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float thickness = 1;
		public float Thickness {
			get {
				return thickness;
			}
			set {
				thickness = value;
				UpdateDimensions();
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
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
		
		public RoomWall leftWall;
		public RoomWall rightWall;
		public RoomWall frontWall;
		public RoomWall rearWall;
		public RoomWall floor;
		public RoomWall roof;
		
		[HideInInspector] public bool initialized;
	
		void Awake() {
			if (!Application.isPlaying && !initialized) {
				leftWall = new RoomWall("Left Wall", this);
				rightWall = new RoomWall("Right Wall", this);
				frontWall = new RoomWall("Front Wall", this);
				rearWall = new RoomWall("Rear Wall", this);
				floor = new RoomWall("Floor", this);
				roof = new RoomWall("Roof", this);
				material = leftWall.Material;
			
				initialized = true;
				UpdateDimensions();
			}
		}

		void UpdateDimensions() {
			leftWall.wall.localPosition = new Vector3(-dimensions.x / 2, dimensions.y / 2, 0);
			leftWall.wall.localEulerAngles = new Vector3(0, 90, 0);
			leftWall.wall.localScale = new Vector3(dimensions.z + thickness, dimensions.y - thickness, thickness);
		
			rightWall.wall.localPosition = new Vector3(dimensions.x / 2, dimensions.y / 2, 0);
			rightWall.wall.localEulerAngles = new Vector3(0, 90, 0);
			rightWall.wall.localScale = new Vector3(dimensions.z + thickness, dimensions.y - thickness, thickness);
		
			frontWall.wall.localPosition = new Vector3(0, dimensions.y / 2, -dimensions.z / 2);
			frontWall.wall.localEulerAngles = new Vector3(0, 0, 0);
			frontWall.wall.localScale = new Vector3(dimensions.x - thickness, dimensions.y - thickness, thickness);
		
			rearWall.wall.localPosition = new Vector3(0, dimensions.y / 2, dimensions.z / 2);
			rearWall.wall.localEulerAngles = new Vector3(0, 0, 0);
			rearWall.wall.localScale = new Vector3(dimensions.x - thickness, dimensions.y - thickness, thickness);
		
			floor.wall.localPosition = new Vector3(0, 0, 0);
			floor.wall.localEulerAngles = new Vector3(90, 0, 0);
			floor.wall.localScale = new Vector3(dimensions.x + thickness, dimensions.z + thickness, thickness);
		
			roof.wall.localPosition = new Vector3(0, dimensions.y, 0);
			roof.wall.localEulerAngles = new Vector3(90, 0, 0);
			roof.wall.localScale = new Vector3(dimensions.x + thickness, dimensions.z + thickness, thickness);
		}

		void UpdateMaterials() {
			leftWall.Material = material;
			rightWall.Material = material;
			frontWall.Material = material;
			rearWall.Material = material;
			floor.Material = material;
			roof.Material = material;
		}
	}
}
