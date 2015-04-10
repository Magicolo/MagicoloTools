using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public abstract class MonoBehaviourExtended : MonoBehaviour {

		#region Components
		bool _gameObjectCached;
		GameObject _gameObject;
		new public GameObject gameObject { 
			get { 
				_gameObject = _gameObjectCached ? _gameObject : base.gameObject;
				_gameObjectCached = true;
				return _gameObject;
			}
		}
		
		bool _transformCached;
		Transform _transform;
		new public Transform transform { 
			get { 
				_transform = _transformCached ? _transform : GetComponent<Transform>();
				_transformCached = true;
				return _transform;
			}
		}
		#endregion
		
		public void Invoke(string methodName, float delay, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, delay, arguments));
		}
	
		public void Invoke(string methodName, params object[] arguments) {
			this.InvokeMethod(methodName, arguments);
		}
	
		public void InvokeRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, delay, repeatRate, arguments));
		}
		
		public void InvokeRepeating(string methodName, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, 0, repeatRate, arguments));
		}
	
		IEnumerator InvokeDelayed(string methodName, float delay, params object[] arguments) {
			yield return new WaitForSeconds(delay);
			
			Invoke(methodName, arguments);
		}
	
		IEnumerator InvokeDelayedRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			yield return new WaitForSeconds(delay);
		
			while (this != null && enabled) {
				Invoke(methodName, arguments);
				
				yield return new WaitForSeconds(repeatRate);
			}
		}
	}
}

