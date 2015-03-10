using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public abstract class MonoBehaviourExtended : MonoBehaviour {

		public enum CoroutineStates {
			None,
			Playing,
			Paused,
			Stopped
		}
		
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
		
		public void InvokeMethod(string methodName, float delay, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, delay, arguments));
		}
	
		public void InvokeMethod(string methodName, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, 0, arguments));
		}
	
		public void InvokeMethodRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, delay, repeatRate, arguments));
		}
		
		public void InvokeMethodRepeating(string methodName, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, 0, repeatRate, arguments));
		}
	
		IEnumerator InvokeDelayed(string methodName, float delay, params object[] arguments) {
			yield return new WaitForSeconds(delay);
			InvokeMethod(methodName, arguments);
		}
	
		IEnumerator InvokeDelayedRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			yield return new WaitForSeconds(delay);
		
			while (this != null && enabled) {
				InvokeMethod(methodName, arguments);
				yield return new WaitForSeconds(repeatRate);
			}
		}
	}
}

