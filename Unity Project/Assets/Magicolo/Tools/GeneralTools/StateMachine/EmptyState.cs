using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo.GeneralTools;

namespace Magicolo {
	[System.Serializable]
	public class EmptyState : IState {

		static EmptyState instance;
		public static EmptyState Instance {
			get {
				if (instance == null) {
					instance = new EmptyState();
				}
				return instance;
			}
		}

		public IStateLayer layer {
			get {
				throw new System.NotImplementedException();
			}
		}

		public IStateMachine machine {
			get {
				throw new System.NotImplementedException();
			}
		}

		public bool IsActive {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		public void OnEnter() {
		}

		public void OnExit() {
		}

		public void OnAwake() {
		}
		
		public void OnStart() {
		}

		public void OnUpdate() {
		}

		public void OnFixedUpdate() {
		}

		public void OnLateUpdate() {
		}

		public void CollisionEnter(Collision collision) {
		}

		public void CollisionStay(Collision collision) {
		}

		public void CollisionExit(Collision collision) {
		}

		public void CollisionEnter2D(Collision2D collision) {
		}

		public void CollisionStay2D(Collision2D collision) {
		}

		public void CollisionExit2D(Collision2D collision) {
		}

		public void TriggerEnter(Collider collision) {
		}

		public void TriggerStay(Collider collision) {
		}

		public void TriggerExit(Collider collision) {
		}

		public void TriggerEnter2D(Collider2D collision) {
		}

		public void TriggerStay2D(Collider2D collision) {
		}

		public void TriggerExit2D(Collider2D collision) {
		}

		public T SwitchState<T>(int index = 0) where T : IState {
			throw new System.NotImplementedException();
		}

		public IState SwitchState(System.Type stateType, int index = 0) {
			throw new System.NotImplementedException();
		}

		public IState SwitchState(string stateName, int index = 0) {
			throw new System.NotImplementedException();
		}

		public bool StateIsActive<T>(int index = 0) where T : IState {
			throw new System.NotImplementedException();
		}

		public bool StateIsActive(System.Type stateType, int index = 0) {
			throw new System.NotImplementedException();
		}

		public bool StateIsActive(string stateName, int index = 0) {
			throw new System.NotImplementedException();
		}

		public T GetActiveState<T>(int index = 0) where T : IState {
			throw new System.NotImplementedException();
		}

		public IState GetActiveState(int index = 0) {
			throw new System.NotImplementedException();
		}

		public IState[] GetActiveStates() {
			throw new System.NotImplementedException();
		}

		public T GetState<T>() where T : IState {
			throw new System.NotImplementedException();
		}

		public IState GetState(System.Type stateType) {
			throw new System.NotImplementedException();
		}

		public IState GetState(string stateName) {
			throw new System.NotImplementedException();
		}

		public IState GetState(int stateIndex) {
			throw new System.NotImplementedException();
		}

		public IState[] GetStates() {
			throw new System.NotImplementedException();
		}

		public T GetLayer<T>() where T : IStateLayer {
			throw new System.NotImplementedException();
		}

		public IStateLayer GetLayer(System.Type layerType) {
			throw new System.NotImplementedException();
		}

		public IStateLayer GetLayer(string layerName) {
			throw new System.NotImplementedException();
		}

		public IStateLayer GetLayer(int layerIndex) {
			throw new System.NotImplementedException();
		}

		public IStateLayer[] GetLayers() {
			throw new System.NotImplementedException();
		}
	}
}

