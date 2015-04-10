using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo.GeneralTools;

namespace Magicolo {
	public abstract class StateLayer : MonoBehaviourExtended, IStateLayer {
		
		public IStateLayer layer {
			get {
				return parentReference as IStateLayer;
			}
		}
		
		public IStateMachine machine {
			get {
				return machineReference;
			}
		}
		
		bool isActive;
		public bool IsActive {
			get {
				return isActive;
			}
		}
		
		[SerializeField] Object parentReference = null;
		[SerializeField] StateMachine machineReference = null;
		[SerializeField] Object[] stateReferences = new Object[0];
		[SerializeField] List<Object> activeStateReferences = new List<Object>{ null };
		
		IState[] states = new IState[0];
		IState[] activeStates = new IState[0];

		Dictionary<string, IState> nameStateDict;
		Dictionary<string, IState> NameStateDict {
			get {
				if (nameStateDict == null || !Application.isPlaying) {
					BuildStateDict();
				}
				
				return nameStateDict;
			}
		}

		public virtual void OnAwake() {
			BuildActiveStates();
			BuildStateDict();
			
			for (int i = 0; i < states.Length; i++) {
				states[i].OnAwake();
			}
		}

		public virtual void OnStart() {
			for (int i = 0; i < states.Length; i++) {
				states[i].OnStart();
			}
		}

		public virtual void OnEnter() {
			isActive = true;
			
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnEnter();
			}
		}

		public virtual void OnExit() {
			isActive = false;
			
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnExit();
			}
		}

		public virtual void OnUpdate() {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnUpdate();
			}
		}
		
		public virtual void OnFixedUpdate() {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnFixedUpdate();
			}
		}
			
		public virtual void OnLateUpdate() {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnLateUpdate();
			}
		}
				
		public virtual void CollisionEnter(Collision collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionEnter(collision);
			}
		}
	
		public virtual void CollisionStay(Collision collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionStay(collision);
			}
		}

		public virtual void CollisionExit(Collision collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionExit(collision);
			}
		}
	
		public virtual void CollisionEnter2D(Collision2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionEnter2D(collision);
			}
		}
	
		public virtual void CollisionStay2D(Collision2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionStay2D(collision);
			}
		}

		public virtual void CollisionExit2D(Collision2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].CollisionExit2D(collision);
			}
		}
	
		public virtual void TriggerEnter(Collider collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerEnter(collision);
			}
		}
	
		public virtual void TriggerStay(Collider collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerStay(collision);
			}
		}

		public virtual void TriggerExit(Collider collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerExit(collision);
			}
		}
	
		public virtual void TriggerEnter2D(Collider2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerEnter2D(collision);
			}
		}
	
		public virtual void TriggerStay2D(Collider2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerStay2D(collision);
			}
		}

		public virtual void TriggerExit2D(Collider2D collision) {
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].TriggerExit2D(collision);
			}
		}

		public T SwitchState<T>(int index = 0) where T : IState {
			IState state = SwitchState(GetState(typeof(T).Name), index);
			return state is T ? (T)state : default(T);
		}
				
		public IState SwitchState(System.Type stateType, int index = 0) {
			return SwitchState(GetState(stateType), index);
		}
		
		public IState SwitchState(string stateName, int index = 0) {
			return SwitchState(GetState(stateName), index);
		}
		
		public bool StateIsActive<T>(int index = 0) where T : IState {
			return StateIsActive(typeof(T).Name, index);
		}
		
		public bool StateIsActive(System.Type stateType, int index = 0) {
			return StateIsActive(stateType.Name, index);
		}
		
		public bool StateIsActive(string stateName, int index = 0) {
			return GetActiveState(index) == GetState(stateName);
		}
		
		public T GetActiveState<T>(int index = 0) where T : IState {
			IState activeState = GetActiveState(index);
			return activeState is T ? (T)activeState : default(T);
		}
		
		public IState GetActiveState(int index = 0) {
			IState activeState = null;
			
			try {
				activeState = activeStates[index] ?? EmptyState.Instance;
			}
			catch {
				Logger.LogError(string.Format("State was not found at index {0}.", index));
			}
			
			return activeState;
		}
		
		public IState[] GetActiveStates() {
			return activeStates.Clone() as IState[];
		}
		
		public T GetState<T>() where T : IState {
			IState state = GetState(typeof(T).Name);
			return state is T ? (T)state : default(T);
		}
		
		public IState GetState(System.Type stateType) {
			return GetState(stateType.Name);
		}
		
		public IState GetState(string stateName) {
			IState state = null;
			
			try {
				state = NameStateDict[stateName];
			}
			catch {
				Logger.LogError(string.Format("State named {0} was not found.", stateName));
			}
			
			return state;
		}

		public IState[] GetStates() {
			return NameStateDict.GetValueArray();
		}
		
		public T GetLayer<T>() where T : IStateLayer {
			return machine.GetLayer<T>();
		}
		
		public IStateLayer GetLayer(System.Type layerType) {
			return machine.GetLayer(layerType);
		}
		
		public IStateLayer GetLayer(string layerName) {
			return machine.GetLayer(layerName);
		}

		IState SwitchState(IState state, int index = 0) {
			state = state ?? EmptyState.Instance;
			
			if (IsActive) {
				GetActiveState(index).OnExit();
			}
			
			activeStates[index] = state;
			activeStateReferences[index] = state as Object;
			
			if (IsActive) {
				state.OnEnter();
			}
		
			return state;
		}

		void BuildActiveStates() {
			activeStates = new IState[activeStateReferences.Count];
			
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i] = activeStateReferences[i] as IState ?? EmptyState.Instance;
			}
		}
		
		void BuildStateDict() {
			nameStateDict = new Dictionary<string, IState>();
			states = new IState[stateReferences.Length];
			
			nameStateDict[EmptyState.Instance.GetType().Name] = EmptyState.Instance;
			nameStateDict[StateMachineUtility.FormatState(EmptyState.Instance.GetType(), "")] = EmptyState.Instance;
			
			for (int i = 0; i < stateReferences.Length; i++) {
				IState state = (IState)stateReferences[i];
				
				if (state != null) {
					nameStateDict[state.GetType().Name] = state;
					nameStateDict[state is StateLayer ? state.GetType().Name : StateMachineUtility.FormatState(state.GetType(), state.layer.GetType())] = state;
					states[i] = state;
				}
			}
		}
	}
}

