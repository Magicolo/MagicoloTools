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
	}
}

