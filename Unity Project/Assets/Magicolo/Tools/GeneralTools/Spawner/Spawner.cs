using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.GeneralTools {
	[ExecuteInEditMode]
	public class Spawner : MonoBehaviour {

		static Spawner instance;
		static Spawner Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<Spawner>();
				}
			
				return instance;
			}
		}
	
		#region Components
		public SpawnerPoolManager poolManager;
		public SpawnerEditorHelper editorHelper;
		public SpawnerHierarchyManager hierarchyManager;
		#endregion
	
		public void Initialize() {
			if (SingletonCheck()) {
				return;
			}
		
			InitializeManagers();
		
			if (Application.isPlaying) {
				StartAll();
			}
		}
	
		public void InitializeManagers() {
			poolManager = poolManager ?? new SpawnerPoolManager(Instance);
			poolManager.Initialize(Instance);
		
			hierarchyManager = hierarchyManager ?? new SpawnerHierarchyManager(Instance);
			hierarchyManager.Initialize(Instance);
		}
	
		public void InitializeHelpers() {
			editorHelper = editorHelper ?? new SpawnerEditorHelper(Instance);
			editorHelper.Initialize(Instance);
		}
	
		public void StartAll() {
			poolManager.Start();
		}
	
		public bool SingletonCheck() {
			if (Instance != null && Instance != this) {
				if (!Application.isPlaying) {
					Logger.LogError(string.Format("There can only be one {0}.", GetType().Name));
				}
			
				gameObject.Remove();
				
				return true;
			}
			
			return false;
		}

		void Awake() {
			Initialize();
		}
	
		#if UNITY_EDITOR
	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnReloadScripts() {
		if (Instance != null) {
			Instance.InitializeHelpers();
		}
	}
	#endif
	
		public static GameObject Spawn() {
			return null;
		}
	}
}
