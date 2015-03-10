using System;
using UnityEngine;
using System.Collections;

namespace Magicolo.GeneralTools {
	[AddComponentMenu("Magicolo/Message Relay")]
	public class MessageRelay : MonoBehaviourExtended {
		public GameObject[] receivers;
		public MessagesToRelay messagesToRelay;
	
		void OnCollisionEnter(Collision collision) {
			if (messagesToRelay.onCollisionEnter) {
				SendMessageToReceivers("OnCollisionEnterRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnCollisionStay(Collision collision) {
			if (messagesToRelay.onCollisionStay) {
				SendMessageToReceivers("OnCollisionStayRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnCollisionExit(Collision collision) {
			if (messagesToRelay.onCollisionExit) {
				SendMessageToReceivers("OnCollisionExitRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnTriggerEnter(Collider collision) {
			if (messagesToRelay.onTriggerEnter) {
				SendMessageToReceivers("OnTriggerEnterRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnTriggerStay(Collider collision) {
			if (messagesToRelay.onTriggerStay) {
				SendMessageToReceivers("OnTriggerStayRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnTriggerExit(Collider collision) {
			if (messagesToRelay.onTriggerExit) {
				SendMessageToReceivers("OnTriggerExitRelay", new RelayInfo(gameObject, collision));
			}
		}

		void OnCollisionEnter2D(Collision2D collision) {
			if (messagesToRelay.onCollisionEnter2D) {
				SendMessageToReceivers("OnCollisionEnter2DRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnCollisionStay2D(Collision2D collision) {
			if (messagesToRelay.onCollisionStay2D) {
				SendMessageToReceivers("OnCollisionStay2DRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnCollisionExit2D(Collision2D collision) {
			if (messagesToRelay.onCollisionExit2D) {
				SendMessageToReceivers("OnCollisionExit2DRelay", new RelayInfo(gameObject, collision));
			}
		}
		
		void OnTriggerEnter2D(Collider2D collision) {
			if (messagesToRelay.onTriggerEnter2D) {
				SendMessageToReceivers("OnTriggerEnter2DRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnTriggerStay2D(Collider2D collision) {
			if (messagesToRelay.onTriggerStay2D) {
				SendMessageToReceivers("OnTriggerStay2DRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnTriggerExit2D(Collider2D collision) {
			if (messagesToRelay.onTriggerExit2D) {
				SendMessageToReceivers("OnTriggerExit2DRelay", new RelayInfo(gameObject, collision));
			}
		}
	
		void OnBecameVisible() {
			if (messagesToRelay.onBecameVisible) {
				SendMessageToReceivers("OnBecameVisibleRelay", new RelayInfo(gameObject));
			}
		}
	
		void OnBecameInvisible() {
			if (messagesToRelay.onBecameInvisible) {
				SendMessageToReceivers("OnBecameInvisibleRelay", new RelayInfo(gameObject));
			}
		}

		void SendMessageToReceivers(string methodName, RelayInfo relayInfo) {
			if (receivers != null) {
				foreach (GameObject receiver in receivers) {
					receiver.SendMessage(methodName, relayInfo, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		[Serializable]
		public class MessagesToRelay {
			public bool onCollisionEnter;
			public bool onCollisionStay;
			public bool onCollisionExit;
			public bool onTriggerEnter;
			public bool onTriggerStay;
			public bool onTriggerExit;
			public bool onCollisionEnter2D;
			public bool onCollisionStay2D;
			public bool onCollisionExit2D;
			public bool onTriggerEnter2D;
			public bool onTriggerStay2D;
			public bool onTriggerExit2D;
			public bool onBecameVisible;
			public bool onBecameInvisible;
		}
	}

	public class RelayInfo {
		public GameObject source;
		public GameObject gameObject;
		public Transform transform;
		public Rigidbody rigidbody;
		public Rigidbody2D rigidbody2D;
		public Collision collision;
		public Collider collider;
		public Collision2D collision2D;
		public Collider2D collider2D;
		
		public RelayInfo(GameObject source) {
			if (source == null) throw new ArgumentNullException("source");
			this.source = source;
		}
		
		public RelayInfo(GameObject source, Collision collision) {
			if (source == null) throw new ArgumentNullException("source");
			if (collision == null) throw new ArgumentNullException("collision");
			this.source = source;
			this.collision = collision;
			this.gameObject = collision.gameObject;
			this.transform = collision.transform;
			this.rigidbody = collision.rigidbody;
			this.collider = collision.collider;
		}
	
		public RelayInfo(GameObject source, Collider collider) {
			if (source == null) throw new ArgumentNullException("source");
			if (collider == null) throw new ArgumentNullException("collider");
			this.source = source;
			this.gameObject = collider.gameObject;
			this.transform = collider.transform;
			this.rigidbody = collider.GetComponent<Rigidbody>();
			this.collider = collider;
		}
		
		public RelayInfo(GameObject source, Collision2D collision2D) {
			if (source == null) throw new ArgumentNullException("source");
			if (collision2D == null) throw new ArgumentNullException("collision2D");
			this.source = source;
			this.collision2D = collision2D;
			this.gameObject = collision2D.gameObject;
			this.transform = collision2D.transform;
			this.rigidbody2D = collision2D.rigidbody;
			this.collider2D = collision2D.collider;
		}
			
		public RelayInfo(GameObject source, Collider2D collider2D) {
			if (source == null) throw new ArgumentNullException("source");
			if (collider2D == null) throw new ArgumentNullException("collider2D");
			this.source = source;
			this.collider2D = collider2D;
			this.gameObject = collider2D.gameObject;
			this.transform = collider2D.transform;
			this.rigidbody2D = collider2D.GetComponent<Rigidbody2D>();
		}
	}
}
