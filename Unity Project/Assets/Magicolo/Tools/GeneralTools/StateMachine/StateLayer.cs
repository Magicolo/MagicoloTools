using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo.GeneralTools;

namespace Magicolo {
	public abstract class StateLayer : MonoBehaviourExtended, IStateLayer {
		
		public StateMachine machine;
		
		[SerializeField] State[] states = new State[0];
		[SerializeField] List<State> currentStates = new List<State>{ null };
		IState[] activeStates;
		Dictionary<string, IState> nameStateDict;

		public virtual void OnAwake() {
			BuildActiveStates();
			BuildStateDict();
			
			foreach (State state in states) {
				state.OnAwake();
			}
		}

		public virtual void OnStart() {
			for (int i = 0; i < currentStates.Count; i++) {
				SwitchState(currentStates[i], i);
			}
			
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i].OnStart();
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
				state = nameStateDict[stateName];
			}
			catch {
				Logger.LogError(string.Format("State named {0} was not found.", stateName));
			}
			
			return state;
		}
		
		public IState GetState(int stateIndex) {
			IState state = null;
			
			try {
				state = states[stateIndex];
			}
			catch {
				Logger.LogError(string.Format("State at index {0} was not found.", stateIndex));
			}
			
			return state;
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
		
		public IStateLayer GetLayer(int layerIndex) {
			return machine.GetLayer(layerIndex);
		}
		
		IState SwitchState(IState state, int index = 0) {
			state = state ?? EmptyState.Instance;
			
			GetActiveState(index).OnExit();
			activeStates[index] = state;
			currentStates[index] = state as State == null ? null : (State)state;
			state.OnEnter();
		
			return state;
		}

		void BuildActiveStates() {
			activeStates = new IState[currentStates.Count];
			
			for (int i = 0; i < activeStates.Length; i++) {
				activeStates[i] = EmptyState.Instance;
			}
		}
		
		void BuildStateDict() {
			nameStateDict = new Dictionary<string, IState>();
			
			nameStateDict[EmptyState.Instance.GetType().Name] = EmptyState.Instance;
			
			foreach (State state in states) {
				if (state != null) {
					nameStateDict[state.GetType().Name] = state;
					nameStateDict[StateMachineUtility.FormatState(state.GetType(), state.layer.GetType())] = state;
				}
			}
		}
	}
}

