using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.GeneralTools {
	public interface IState {

		void OnEnter();
			
		void OnExit();
		
		void OnAwake();
		
		void OnStart();
		
		void OnUpdate();
		
		void OnFixedUpdate();
			
		void OnLateUpdate();
			
		void CollisionEnter(Collision collision);
	
		void CollisionStay(Collision collision);

		void CollisionExit(Collision collision);
	
		void CollisionEnter2D(Collision2D collision);
	
		void CollisionStay2D(Collision2D collision);

		void CollisionExit2D(Collision2D collision);
	
		void TriggerEnter(Collider collision);
	
		void TriggerStay(Collider collision);

		void TriggerExit(Collider collision);
	
		void TriggerEnter2D(Collider2D collision);
	
		void TriggerStay2D(Collider2D collision);

		void TriggerExit2D(Collider2D collision);
	}
}

