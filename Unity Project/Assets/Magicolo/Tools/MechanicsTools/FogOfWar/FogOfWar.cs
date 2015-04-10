using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.MechanicsTools;

// TODO Add reduced update rates option
// TODO Add heightAgents for dynamic lighting
// TODO Pool colliders
// TODO FogAgent should be a MonoBehaviour

namespace Magicolo {
	[RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Magicolo/Mechanics/Fog Of War")]
	public class FogOfWar : MonoBehaviourExtended {

		[SerializeField, PropertyField(typeof(RangeAttribute), 1, 25)]
		int definition;
		public int Definition {
			get {
				return definition;
			}
			set {
				definition = Mathf.Clamp(value, 1, 25);
				
				if (Application.isPlaying) {
					CreateTexture();
					CreateHeightMap();
					CreateLineInfos();
					UpdateFow = true;
				}
			}
		}
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 5000)]
		int renderQueue;
		public int RenderQueue {
			get {
				return renderQueue;
			}
			set {
				renderQueue = value;
				
				if (Application.isPlaying) {
					spriteRenderer.material.renderQueue = renderQueue;
					UpdateFow = true;
				}
			}
		}
		
		[SerializeField, PropertyField]
		FilterMode filterMode = FilterMode.Bilinear;
		public FilterMode FilterMode {
			get {
				return filterMode;
			}
			set {
				filterMode = value;
				
				if (Application.isPlaying) {
					CreateTexture();
					UpdateFow = true;
				}
			}
		}
		
		[SerializeField, PropertyField]
		Color color = Color.black;
		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
				
				if (Application.isPlaying) {
					CreateTexture();
					UpdateFow = true;
				}
			}
		}
		
		[Range(0, 25)] public float fadeSpeed = 5;
		[Range(0, 1)] public float flicker = 0.05F;
		
		[SerializeField, PropertyField]
		bool inverted;
		public bool Inverted {
			get {
				return inverted;
			}
			set {
				inverted = value;
				
				if (Application.isPlaying) {
					CreateTexture();
					UpdateFow = true;
				}
			}
		}
		
		public bool manualUpdate;
		
		[SerializeField, PropertyField(typeof(ToggleAttribute))]
		bool generateHeightMap;
		public bool GenerateHeightMap {
			get {
				return generateHeightMap;
			}
			set {
				generateHeightMap = value;
				
				if (Application.isPlaying) {
					CreateHeightMap();
					UpdateFow = true;
				}
			}
		}
		
		[Empty(DisableBool = "!GenerateHeightMap", Indent = 1)] public LayerMask layerMask;
		
		[Empty(PrefixLabel = "Agent")] public List<FogAgent> fogAgents;
		
		bool updateFow = true;
		public bool UpdateFow {
			get {
				return updateFow;
			}
			set {
				updateFow = value;
			}
		}
		
		bool _spriteRendererCached;
		SpriteRenderer _spriteRenderer;
		public SpriteRenderer spriteRenderer {
			get {
				_spriteRenderer = _spriteRendererCached ? _spriteRenderer : GetComponent<SpriteRenderer>();
				_spriteRendererCached = true;
				return _spriteRenderer;
			}
		}
		
		int mapWidth;
		public int Width {
			get {
				return mapWidth;
			}
		}

		int mapHeight;
		public int Height {
			get {
				return mapHeight;
			}
		}
		
		Rect currentArea;
		public Rect Area {
			get {
				return currentArea;
			}
		}
		
		int mapDiagonal;
		Texture2D texture;
		Vector3 currentPosition;
		Vector3 currentScale;
		float deltaTime;
		float flickerAmount;
		Color[] currentPixels;
		float[,] currentAlphaMap;
		float[,] currentHeightMap;
		List<LineOfSightInfo>[,] currentLineInfos;
		
		void Awake() {
			spriteRenderer.material.renderQueue = renderQueue;
			UpdateAgents();
			CreateTexture();
			CreateHeightMap();
			CreateLineInfos();
		}
		
		void Start() {
			StartCoroutine(UpdateRoutine());
		}
		
		void UpdateAgents() {
			currentScale = transform.lossyScale;
			currentPosition = transform.position - currentScale / 2;
			currentArea = new Rect(currentPosition.x, currentPosition.y, currentScale.x, currentScale.y);
			
			foreach (FogAgent fogAgent in fogAgents) {
				fogAgent.Update();
			}
			
		}
		
		IEnumerator UpdateRoutine() {
			while (true) {
				if (!manualUpdate) {
					UpdateFow = true;
				}
				
				if (UpdateFow && enabled && spriteRenderer.isVisible && gameObject.activeInHierarchy) {
					deltaTime = Time.deltaTime;
					flickerAmount = (Random.value * 2 - 1) * flicker;
						
					UpdateAgents();
					
					Thread updateThread = new Thread(new ThreadStart(UpdateFowAsync));
					updateThread.Start();
					
					while (updateThread.ThreadState == ThreadState.Running) {
						yield return new WaitForSeconds(0);
					}
					
					texture.SetPixels(currentPixels);
					texture.Apply();
					
					UpdateFow = false;
				}
				
				yield return new WaitForSeconds(0);
			}
		}
		
		void UpdateFowAsync() {
			try {
				float[,] alphaMap = new float[mapWidth, mapHeight];
				
				UpdateAlphaMap(currentAlphaMap);
				UpdateTexture(currentAlphaMap);
				
				currentAlphaMap = alphaMap;
			}
			catch (System.Exception exception) {
				Logger.LogError(exception);
			}
		}
		
		void UpdateAlphaMap(float[,] alphaMap) {
			for (int i = fogAgents.Count - 1; i >= 0; i--) {
				ModifyFog(alphaMap, fogAgents[i]);
			}
		}
		
		void UpdateTexture(float[,] alphaMap) {
			int xLength = alphaMap.GetLength(0);
			int yLength = alphaMap.GetLength(1);
			float adjustedFadeSpeed = fadeSpeed * deltaTime;
			
			if (currentPixels.Length == xLength * yLength) {
				int pixelCounter = 0;
				
				for (int y = 0; y < yLength; y++) {
					for (int x = 0; x < xLength; x++) {
						float currentAlpha = currentPixels[pixelCounter].a;
						float alpha = Inverted ? alphaMap[x, y] : 1 - alphaMap[x, y];
						alpha += flickerAmount * alpha;
						float difference = alpha - currentAlpha;
						
						if (difference > adjustedFadeSpeed) {
							currentPixels[pixelCounter].a += adjustedFadeSpeed;
						}
						else if (difference < -adjustedFadeSpeed) {
							currentPixels[pixelCounter].a -= adjustedFadeSpeed;
						}
						else if (difference != 0) {
							currentPixels[pixelCounter].a = alpha;
						}
						
						pixelCounter += 1;
					}
				}
			}
		}
		
		void ModifyFog(float[,] alphaMap, Vector3 position, float sightRadius, float strength, float falloff, bool invert) {
			Vector2 texturePosition = WorldToPixel(position);
			float pixelSightRadius = Mathf.Min(sightRadius * Definition, Mathf.Floor(mapDiagonal / 2 - 1));
			bool insideRect = currentArea.Contains(position);
			int x = (int)texturePosition.x.Round();
			int y = (int)texturePosition.y.Round();
			
			if (pixelSightRadius <= 0 || strength <= 0 || fadeSpeed <= 0) {
				return;
			}
			
			if (x >= 0 && x < alphaMap.GetLength(0) && y >= 0 && y < alphaMap.GetLength(1)) {
				alphaMap[(int)texturePosition.x.Round(), (int)texturePosition.y.Round()] = invert ? 1 - strength : strength;
			}
			
			List<LineOfSightInfo> centerInfos = currentLineInfos[0, 0];
			
			for (int i = 0; i < centerInfos.Count; i++) {
				PrimaryLineOfSight lineOfSight = new PrimaryLineOfSight(centerInfos[i], x, y, pixelSightRadius, strength, falloff, invert, alphaMap, currentHeightMap, currentLineInfos);
				
				if (insideRect || (position.x < currentArea.xMin && lineOfSight.info.directionX >= 0) || (position.x > currentArea.xMax && lineOfSight.info.directionX <= 0) || (position.y < currentArea.yMin && lineOfSight.info.directionY >= 0) || (position.y > currentArea.yMax && lineOfSight.info.directionY <= 0)) {
					lineOfSight.Complete();
				}
			}
		}
		
		void ModifyFog(float[,] alphaMap, FogAgent fogAgent) {
			if (fogAgent != null && fogAgent.Rect.Intersects(currentArea)) {
				ModifyFog(alphaMap, fogAgent.Position, fogAgent.SightRadius, fogAgent.Strength, fogAgent.Falloff, fogAgent.Inverted);
			}
		}
		
		void CreateTexture() {
			mapWidth = (int)(transform.lossyScale.x * definition).Round();
			mapHeight = (int)(transform.lossyScale.y * definition).Round();
			mapDiagonal = (int)Mathf.Ceil(Mathf.Sqrt(mapWidth * mapWidth + mapHeight * mapHeight)) * 2;
			currentAlphaMap = new float[mapWidth, mapHeight];
			texture = new Texture2D(mapWidth, mapHeight, TextureFormat.RGBA32, false);
			texture.filterMode = filterMode;
			texture.wrapMode = TextureWrapMode.Clamp;
			spriteRenderer.material.SetTexture("_AlphaMap", texture);
			
			currentPixels = new Color[mapWidth * mapHeight];
			
			for (int i = 0; i < currentPixels.Length; i++) {
				currentPixels[i] = Color;
				currentPixels[i].a = Inverted ? 0 : 1;
			}
			
			texture.SetPixels(currentPixels);
		}

		void CreateHeightMap() {
			currentHeightMap = new float[mapWidth, mapHeight];
			
			if (GenerateHeightMap) {
				for (int y = 0; y < mapHeight; y++) {
					for (int x = 0; x < mapWidth; x++) {
						Vector3 position = new Vector3(currentArea.xMin + (x + 0.5F) / Definition, currentArea.yMin + (y + 0.5F) / Definition, transform.position.z);
						
						if (Physics.Raycast(position - Vector3.forward * 100, Vector3.forward, Mathf.Infinity, layerMask)) {
							currentHeightMap[x, y] = 1;
						}
					}
				}
			}
		}
		
		void CreateLineInfos() {
			currentLineInfos = new List<LineOfSightInfo>[mapDiagonal * 2, mapDiagonal * 2];
			List<LineOfSightInfo> centerInfos = new List<LineOfSightInfo>();
			
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, 0));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 0, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, -1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, 0));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, -1, 1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 0, 1));
			centerInfos.Add(new LineOfSightInfo(mapDiagonal, mapDiagonal, 1, 1));
			
			currentLineInfos[0, 0] = centerInfos;
			
			foreach (LineOfSightInfo lineInfo in centerInfos) {
				lineInfo.GeneratePoints(mapDiagonal);
				
				for (int i = 1; i < lineInfo.points.Length; i++) {
					PointInfo point = lineInfo.GetNextPoint();
					List<LineOfSightInfo> infos = new List<LineOfSightInfo>();
					
					Vector2 direction = new Vector2(lineInfo.directionX, lineInfo.directionY).Rotate(-45).Round();
					infos.Add(new LineOfSightInfo(point.coordinateX, point.coordinateY, (int)direction.x, (int)direction.y));
					
					direction = new Vector2(lineInfo.directionX, lineInfo.directionY).Rotate(45).Round();
					infos.Add(new LineOfSightInfo(point.coordinateX, point.coordinateY, (int)direction.x, (int)direction.y));
					
					infos.ForEach(info => info.GeneratePoints(mapDiagonal));
					
					currentLineInfos[point.coordinateX, point.coordinateY] = infos;
				}
			}
		}
		
		Vector2 WorldToPixel(Vector3 worldPoint) {
			return new Vector2((worldPoint.x - currentPosition.x) * Definition, (worldPoint.y - currentPosition.y) * Definition);
		}
		
		public void AddAgent(FogAgent agent) {
			fogAgents.Add(agent);
			UpdateFow = true;
		}
		
		public void RemoveAgent(FogAgent agent) {
			fogAgents.Remove(agent);
			UpdateFow = true;
		}
		
		public void RemoveAgent(int index) {
			fogAgents.RemoveAt(index);
			UpdateFow = true;
		}
		
		public FogAgent GetAgent(Transform agentTransform) {
			return GetAgent(fogAgents.FindIndex(agent => agent.Transform = agentTransform));
		}
		
		public FogAgent GetAgent(int index) {
			FogAgent agent = null;
			
			try {
				agent = fogAgents[index];
			}
			catch {
				Logger.LogError(string.Format("Fog agent at index {0} could not be found.", index));
			}
			
			return agent;
		}
		
		public Color GetColor(Vector3 worldPoint) {
			return GetColor(WorldToPixel(worldPoint));
		}
		
		public Color GetColor(Vector2 pixel) {
			return currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()];
		}
		
		public void SetColor(Vector3 worldPoint, Color color) {
			SetColor(WorldToPixel(worldPoint), color, Channels.RGB);
		}
		
		public void SetColor(Vector2 pixel, Color color) {
			SetColor(pixel, color, Channels.RGB);
		}
		
		public void SetColor(Vector3 worldPoint, Color color, Channels channels) {
			SetColor(WorldToPixel(worldPoint), color, channels);
		}
		
		public void SetColor(Vector2 pixel, Color color, Channels channels) {
			Color currentColor = currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()];
			currentPixels[(int)pixel.y.Round() * mapWidth + (int)pixel.x.Round()] = currentColor.SetValues(color, channels);
		}
		
		public float GetAlpha(Vector3 worldPoint) {
			return GetAlpha(WorldToPixel(worldPoint));
		}
		
		public float GetAlpha(Vector2 pixel) {
			return currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		}
		
		public float GetHeight(Vector3 worldPoint) {
			return GetHeight(WorldToPixel(worldPoint));
		}
		
		public float GetHeight(Vector2 pixel) {
			return currentHeightMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		}
		
		public void SetHeight(Vector3 worldPoint, float height) {
			if (currentArea.Contains(worldPoint)) {
				SetHeight(WorldToPixel(worldPoint), height);
			}
		}
		
		public void SetHeight(Vector2 pixel, float height) {
			currentHeightMap[(int)pixel.x.Round(), (int)pixel.y.Round()] = Mathf.Clamp01(height);
		}
		
		public void SetHeightMap(float[,] heightMap) {
			this.currentHeightMap = heightMap;
		}
		
		public bool IsFogged(Vector3 worldPoint) {
			return currentArea.Contains(worldPoint) && IsFogged(WorldToPixel(worldPoint), 0);
		}
		
		public bool IsFogged(Vector2 pixel) {
			return IsFogged(pixel, 0);
		}
		
		public bool IsFogged(Vector3 worldPoint, float alphaThreshold) {
			return currentArea.Contains(worldPoint) && IsFogged(WorldToPixel(worldPoint), alphaThreshold);
		}
		
		public bool IsFogged(Vector2 pixel, float alphaThreshold) {
			return currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()] <= alphaThreshold;
		}
		
		void Reset() {
			spriteRenderer.sprite = HelperFunctions.LoadAssetInFolder<Sprite>("SquareSprite.png", "GraphicsTools");
			spriteRenderer.sharedMaterial = HelperFunctions.LoadAssetInFolder<Material>("FogOfWar.mat", "FogOfWar");
			renderQueue = spriteRenderer.sharedMaterial.renderQueue;
			
			Definition = 1;
		}
	}
}
