using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo {
	[AddComponentMenu("Magicolo/General/State Machine")]
	public class StateMachine : MonoBehaviourExtended, IStateMachine {
		
		[SerializeField]
		bool debug = false;
		public bool Debug {
			get {
				return debug;
			}
		}
		
		bool isActive;
		public bool IsActive {
			get {
				return isActive;
			}
		}
		
		[SerializeField] StateLayer[] layers = new StateLayer[0];
		
		Dictionary<string, IStateLayer> nameLayerDict;
		Dictionary<string, IStateLayer> NameLayerDict {
			get {
				if (nameLayerDict == null) {
					BuildLayerDict();
				}
				return nameLayerDict;
			}
		}
		
		void OnEnable() {
			isActive = true;
			
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnEnter();
			}
		}
		
		void OnDisable() {
			isActive = false;
			
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnExit();
			}
		}
		
		void Awake() {
			BuildLayerDict();
			
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnAwake();
			}
		}
		
		void Start() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnStart();
			}
		}
		
		public void OnUpdate() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnUpdate();
			}
		}
	
		public void OnFixedUpdate() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnFixedUpdate();
			}
		}
		
		public void OnLateUpdate() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnLateUpdate();
			}
		}

		public void CollisionEnter(Collision collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionEnter(collision);
			}
		}
	
		public void CollisionStay(Collision collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionStay(collision);
			}
		}

		public void CollisionExit(Collision collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionExit(collision);
			}
		}
	
		public void CollisionEnter2D(Collision2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionEnter2D(collision);
			}
		}
	
		public void CollisionStay2D(Collision2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionStay2D(collision);
			}
		}

		public void CollisionExit2D(Collision2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].CollisionExit2D(collision);
			}
		}
	
		public void TriggerEnter(Collider collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerEnter(collision);
			}
		}
	
		public void TriggerStay(Collider collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerStay(collision);
			}
		}

		public void TriggerExit(Collider collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerExit(collision);
			}
		}
	
		public void TriggerEnter2D(Collider2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerEnter2D(collision);
			}
		}
	
		public void TriggerStay2D(Collider2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerStay2D(collision);
			}
		}

		public void TriggerExit2D(Collider2D collision) {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].TriggerExit2D(collision);
			}
		}

		public T GetLayer<T>() where T : IStateLayer {
			return (T)GetLayer(typeof(T).Name);
		}
		
		public IStateLayer GetLayer(System.Type layerType) {
			return GetLayer(layerType.Name);
		}
		
		public IStateLayer GetLayer(string layerName) {
			IStateLayer layer = null;
			
			try {
				layer = NameLayerDict[layerName];
			}
			catch {
				Logger.LogError(string.Format("Layer named {0} was not found.", layerName));
			}
			
			return layer;
		}

		public IStateLayer[] GetLayers() {
			return layers.Clone() as IStateLayer[];
		}
		
		void BuildLayerDict() {
			nameLayerDict = new Dictionary<string, IStateLayer>();
			
			foreach (StateLayer layer in layers) {
				if (layer != null) {
					nameLayerDict[layer.GetType().Name] = layer;
					nameLayerDict[StateMachineUtility.FormatLayer(layer.GetType())] = layer;
				}
			}
		}
	
		void Reset() {
			StateMachineUtility.CleanUp(null, gameObject);
		}
	}
}
