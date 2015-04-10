using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.MechanicsTools;

namespace Magicolo {
	[RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Magicolo/Mechanics/Diggable")]
	public class Diggable : MonoBehaviourExtended {

		public Sprite map;
		[Min] public float scale = 0.1F;
		[Range(0, 1)] public float alphaThreshold = 0.5F;
		public GameObject fxPrefab;
		[Min] public int fxPoolSize = 500;
	
		[HideInInspector] public int layer;
		[HideInInspector] public int width;
		[HideInInspector] public int height;
		[HideInInspector] public Texture2D mapTexture;
		[HideInInspector] public DiggableZoneManager zoneManager;
		[HideInInspector] public FogOfWar fogOfWar;
		[HideInInspector] public ParticleSystem[] fxPool;
		[HideInInspector] public GameObject fxPoolObject;
		[HideInInspector] public GameObject colliderContainer;
	
		public const float margin = 0.001F;
	
		bool _spriteRendererCached;
		SpriteRenderer _spriteRenderer;
		public SpriteRenderer spriteRenderer { 
			get { 
				_spriteRenderer = _spriteRendererCached ? _spriteRenderer : gameObject.GetComponent<SpriteRenderer>();
				_spriteRendererCached = true;
				return _spriteRenderer;
			}
		}
	
		Sprite runtimeSprite;
		Texture2D runtimeMapTexture;
		int fxIndex;
	
		void Awake() {
			hideFlags = HideFlags.NotEditable;
		
			runtimeMapTexture = new Texture2D(mapTexture.width, mapTexture.height, TextureFormat.RGBA32, false);
			runtimeMapTexture.name = mapTexture.name;
			runtimeMapTexture.filterMode = mapTexture.filterMode;
			runtimeMapTexture.wrapMode = mapTexture.wrapMode;
			runtimeMapTexture.SetPixels(mapTexture.GetPixels());
			runtimeMapTexture.Apply();
			runtimeSprite = Sprite.Create(runtimeMapTexture, new Rect(0, 0, mapTexture.width, mapTexture.height), Vector2.zero, 1);
			runtimeSprite.name = map.name;
			spriteRenderer.sprite = runtimeSprite;
			
			fogOfWar = GetComponentInChildren<FogOfWar>();
		}
	
		void Reset() {
			GameObject toRemove = this.FindChild("Zone Manager");
			if (toRemove != null) {
				toRemove.Remove();
			}
			
			toRemove = this.FindChild("Colliders");
			if (toRemove != null) {
				toRemove.Remove();
			}
			
			toRemove = this.FindChild("FX Pool");
			if (toRemove != null) {
				toRemove.Remove();
			}
		}
	
		[Button("Update", "UpdateMap", NoPrefixLabel = true)] public bool updateMap;
		void UpdateMap() {
			Reset();
		
			if (map == null || scale <= 0) {
				return;
			}
		
			layer = LayerMask.NameToLayer("Diggable");
			transform.localScale = Vector3.one * scale;
			spriteRenderer.sprite = map;
		
			mapTexture = map.texture;
			width = mapTexture.width;
			height = mapTexture.height;
		
			colliderContainer = gameObject.AddChild("Colliders");
			zoneManager = gameObject.AddChild("Zone Manager").AddComponent<DiggableZoneManager>();
			zoneManager.Initialize(this);

			CreateFXPool();
		}
	
		void CreateFXPool() {
			if (fxPrefab == null) {
				return;
			}
		
			fxPoolObject = gameObject.AddChild("FX Pool");
			fxPool = new ParticleSystem[fxPoolSize];
		
			for (int i = 0; i < fxPoolSize; i++) {
				GameObject fxObject = Instantiate(fxPrefab);
				Transform fxTransform = fxObject.transform;
			
				fxTransform.parent = fxPoolObject.transform;
				fxTransform.localPosition = Vector3.zero;
				fxTransform.localScale = Vector3.one;
			
				fxPool[i] = fxObject.GetComponent<ParticleSystem>();
			}
		}
	
		public void Dig(Vector3 worldPoint) {
			Dig(WorldToPixel(worldPoint));
		}
	
		public void Dig(Vector2 pixel) {
			Dig((int)pixel.x, (int)pixel.y);
		}
	
		public void Dig(int x, int y) {
			runtimeMapTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
			runtimeMapTexture.Apply();
		
			DiggableZone zone = zoneManager.GetZone(x, y);
		
			if (zone != null) {
				zone.Break();
			}
			
			if (fogOfWar != null) {
				Vector3 worldPoint = PixelToWorld(new Vector2(x, y));
				fogOfWar.SetHeight(worldPoint, 0);
			}
		}

		public GameObject SpawnBoxCollider(Rect rect) {
			return SpawnBoxCollider(rect.position, rect.size);
		}
	
		public GameObject SpawnBoxCollider(Vector2 pixel, Vector2 size) {
			BoxCollider boxCollider = SpawnCollider<BoxCollider>(pixel, size);
			
			boxCollider.center = new Vector2(0.5F, 0.5F);
			boxCollider.size = new Vector3(1 - margin, 1 - margin, 1);
		
			return boxCollider.gameObject;
		}
	
		public GameObject SpawnCapsuleCollider(Rect rect) {
			return SpawnCapsuleCollider(rect.position, rect.size);
		}
	
		public GameObject SpawnCapsuleCollider(Vector2 pixel, Vector2 size) {
			CapsuleCollider capsuleCollider = SpawnCollider<CapsuleCollider>(pixel, size);
			
			capsuleCollider.center = new Vector2(0.5F, 0.5F);
			capsuleCollider.radius = 0.5F - margin;
			capsuleCollider.direction = 2;
		
			return capsuleCollider.gameObject;
		}
	
		T SpawnCollider<T>(Vector2 pixel, Vector2 size) where T : Collider {
			GameObject colliderObject = colliderContainer.AddChild("Collider");
			colliderObject.layer = layer;
		
			Transform colliderTransform = colliderObject.transform;
			colliderTransform.SetLocalScale(size, Axis.XY);
			colliderTransform.SetScale(scale, Axis.Z);
			colliderTransform.SetLocalPosition(pixel, Axis.XY);
		
			return colliderObject.AddComponent<T>();
		}
	
		public void SpawnFX(Vector2 pixel) {
			if (fxPrefab != null) {
				ParticleSystem fx = fxPool[fxIndex];
				fxIndex = (fxIndex + 1) % fxPoolSize;
				fx.transform.localPosition = pixel;
				fx.Simulate(0, true);
				fx.Play();
			}
		}

		public Color GetPixel(float x, float y) {
			return GetPixel((int)x, (int)y);
		}
	
		public Color GetPixel(int x, int y) {
			return Application.isPlaying ? runtimeMapTexture.GetPixel(x, y) : mapTexture.GetPixel(x, y);
		}
	
		public void SetPixel(int x, int y, Color pixel) {
			runtimeMapTexture.SetPixel(x, y, pixel);
		}
	
		Vector2 WorldToPixel(Vector3 worldPoint) {
			return new Vector2((worldPoint.x - transform.position.x) / scale, (worldPoint.y - transform.position.y) / scale);
		}
		
		Vector3 PixelToWorld(Vector2 pixel) {
			return new Vector3(pixel.x * scale + transform.position.x, pixel.y * scale + transform.position.y, transform.position.z);
		}
	}
}
