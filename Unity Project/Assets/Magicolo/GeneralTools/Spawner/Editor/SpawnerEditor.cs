using System;
using System.Collections;
using Magicolo.EditorTools;
using Magicolo.GeneralTools;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[CustomEditor(typeof(Spawner))]
	public class SpawnerEditor : CustomEditorBase {

		Spawner spawner;
		SpawnerPoolManager poolManager;
		SerializedProperty poolManagerProperty;
		SerializedProperty poolsProperty;
		SpawnerPool currentPool;
//		SerializedProperty currentPoolProperty;
		
		public override void OnEnable() {
			base.OnEnable();
			
			spawner = (Spawner)target;
			spawner.SetExecutionOrder(-15);
			spawner.hierarchyManager.FreezeTransforms();
		}
		
		public override void OnInspectorGUI() {
			spawner.hierarchyManager.FreezeTransforms();
			poolManager = spawner.poolManager;
			poolManagerProperty = serializedObject.FindProperty("poolManager");
			
			Begin();
			
			ShowPrefabDropArea();
			ShowPools();
			
			End();
		}

		void ShowPrefabDropArea() {
			poolsProperty = poolManagerProperty.FindPropertyRelative("pools");
			
			EditorGUILayout.HelpBox("Drop Prefabs Here", MessageType.Info);
			DropArea<GameObject>(true, OnPrefabDropped);
		}
		
		void OnPrefabDropped(GameObject droppedObject) {
			AddToArray(poolsProperty);
			
			Logger.Log(PrefabUtility.GetPrefabObject(droppedObject), PrefabUtility.GetPrefabType(droppedObject));
			poolManager.pools[poolManager.pools.Length - 1] = new SpawnerPool(droppedObject, spawner);
			poolsProperty.serializedObject.Update();
		}
		
		void ShowPools() {
			
			for (int i = 0; i < poolsProperty.arraySize; i++) {
				currentPool = poolManager.pools[i];
//				currentPoolProperty = poolsProperty.GetArrayElementAtIndex(i);
				
				if (currentPool.prefab.prefab == null) {
					DeleteFromArray(poolsProperty, i);
					break;
				}
				
				GUIStyle style = new GUIStyle("foldout");
				style.fontStyle = FontStyle.Bold;
				if (DeleteFoldOut(poolsProperty, i, currentPool.Name.ToGUIContent(), style)) {
					break;
				}
			}
		}
	}
}