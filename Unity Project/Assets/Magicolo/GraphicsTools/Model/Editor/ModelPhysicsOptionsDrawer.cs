using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GraphicsTools {
	[CustomPropertyDrawer(typeof(ModelPhysicsOptions))]
	public class ModelPhysicsOptionsDrawer : CustomPropertyDrawerBase {

		Model model;
		ModelPhysicsOptions physicsOptions;
		SerializedProperty physicsOptionsProperty;
		List<string> deactivatedColliders;
		SerializedProperty deactivatedCollidersProperty;
		
		float height;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			Begin(position, property, label);
			
			model = target as Model;
			physicsOptions = model.physicsOptions;
			physicsOptionsProperty = property;
			physicsOptionsProperty.isExpanded = true;
			
			currentPosition.height = lineHeight;
			EditorGUI.LabelField(currentPosition, physicsOptionsProperty.displayName, new GUIStyle("boldLabel"));
			currentPosition.y += currentPosition.height + 2;
			
			ShowPhysicsOptions();
			
			height = currentPosition.y - position.y;
			
			End();
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return property.isExpanded ? height == 0 ? EditorGUI.GetPropertyHeight(property, label, true) - 18 : height : EditorGUI.GetPropertyHeight(property, label, true);
		}
		
		void Initialize() {
			UpdateBoneColliders();
			SetLayer();
			physicsOptionsProperty.FindPropertyRelative("initialized").SetValue(true);
		}
		
		void ShowPhysicsOptions() {
			if (!physicsOptions.initialized) {
				Initialize();
			}
			
			if (physicsOptionsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginChangeCheck();
				
				ToggleButton(physicsOptionsProperty.FindPropertyRelative("generateColliders"), "Colliders Enabled".ToGUIContent(), "Colliders Disabled".ToGUIContent());
				PropertyField(physicsOptionsProperty.FindPropertyRelative("colliderType"));
				PropertyField(physicsOptionsProperty.FindPropertyRelative("colliderSize"));
				
				if (EditorGUI.EndChangeCheck()) {
					UpdateBoneColliders();
				}
				
				ShowBones();
				
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowBones() {
			deactivatedColliders = physicsOptions.deactivatedColliders;
			deactivatedCollidersProperty = physicsOptionsProperty.FindPropertyRelative("deactivatedColliders");
			
			EditorGUI.BeginChangeCheck();
			deactivatedCollidersProperty.isExpanded = EditorGUI.Foldout(currentPosition, deactivatedCollidersProperty.isExpanded, "Bones");
			if (EditorGUI.EndChangeCheck()) {
				deactivatedCollidersProperty.serializedObject.ApplyModifiedProperties();
			}
			
			currentPosition.y += currentPosition.height;
			
			if (deactivatedCollidersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				List<string> boneNames = new List<string>(model.BoneNameVerticesDict.Keys);
				for (int i = 0; i < boneNames.Count; i++) {
					string boneName = boneNames[i].Replace('.', '_');
					bool isActive = !deactivatedColliders.Contains(boneName);
					
					EditorGUI.BeginChangeCheck();
					isActive = EditorGUI.ToggleLeft(currentPosition, boneName, isActive);
					currentPosition.y += currentPosition.height;
					if (EditorGUI.EndChangeCheck()) {
						if (isActive) {
							deactivatedColliders.Remove(boneName);
						}
						else if (!deactivatedColliders.Contains(boneName)) {
							deactivatedColliders.Add(boneName);
						}
						
						UpdateBoneColliders();
					}
				}
				
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void UpdateBoneColliders() {
			DestroyBoneColliders();
			
			if (physicsOptions.generateColliders) {
				GenerateBoneColliders();
			}
		}
		
		void GenerateBoneColliders() {
			foreach (string boneName in model.BoneNameVerticesDict.Keys) {
				GameObject bone = model.FindChildRecursive(boneName.Replace('.', '_'));
				GameObject boneCollider = bone.FindChild(bone.name + "Collider");
				
				if (boneCollider == null) {
					boneCollider = bone.AddChild(bone.name + "Collider");
					boneCollider.transform.SetPosition(model.transform.position);
					boneCollider.transform.rotation = Quaternion.identity;
					boneCollider.transform.SetScale(-model.transform.localScale.x, "X");
					
				}
				
				if (boneCollider != null && !physicsOptions.deactivatedColliders.Contains(bone.name)) {
					switch (physicsOptions.colliderType) {
						case ModelPhysicsOptions.ColliderTypes.Sphere:
							GenerateSphereCollider(boneCollider, model.BoneNameVerticesDict[boneName]);
							break;
						case ModelPhysicsOptions.ColliderTypes.Box:
							GenerateBoxCollider(boneCollider, model.BoneNameVerticesDict[boneName]);
							break;
						case ModelPhysicsOptions.ColliderTypes.Circle2D:
							GenerateCircleCollider2D(boneCollider, model.BoneNameVerticesDict[boneName]);
							break;
						case ModelPhysicsOptions.ColliderTypes.Box2D:
							GenerateBoxCollider2D(boneCollider, model.BoneNameVerticesDict[boneName]);
							break;
						case ModelPhysicsOptions.ColliderTypes.Polygon2D:
							GeneratePolygonCollider2D(boneCollider, model.BoneNameVerticesDict[boneName]);
							break;
					}
				}
			}
		}
		
		void GenerateSphereCollider(GameObject boneCollider, Vector2[] points) {
			float l = float.MaxValue;
			float r = float.MinValue;
			float d = float.MaxValue;
			float u = float.MinValue;
			
			for (int i = 0; i < points.Length; i++) {
				if (points[i].x < l) {
					l = points[i].x;
				}
				
				if (points[i].x > r) {
					r = points[i].x;
				}
				
				if (points[i].y < d) {
					d = points[i].y;
				}
				
				if (points[i].y > u) {
					u = points[i].y;
				}
			}
			
			float radius = Mathf.Max(Mathf.Abs(l - r), Mathf.Abs(d - u)) / 2 * physicsOptions.colliderSize;
			if (radius >= 0.001F) {
				SphereCollider sphere = boneCollider.GetOrAddComponent<SphereCollider>();
				sphere.radius = radius;
				sphere.center = new Vector3(l + r, u + d, 0) / 2;
			}
		}
		
		void GenerateBoxCollider(GameObject boneCollider, Vector2[] points) {
			float l = float.MaxValue;
			float r = float.MinValue;
			float d = float.MaxValue;
			float u = float.MinValue;
			
			for (int i = 0; i < points.Length; i++) {
				if (points[i].x < l) {
					l = points[i].x;
				}
				
				if (points[i].x > r) {
					r = points[i].x;
				}
				
				if (points[i].y < d) {
					d = points[i].y;
				}
				
				if (points[i].y > u) {
					u = points[i].y;
				}
			}
			
			Vector3 size = new Vector3(Mathf.Abs(l - r), Mathf.Abs(d - u), 1) * physicsOptions.colliderSize;
			if (size.x >= 0.001F || size.y >= 0.001F) {
				BoxCollider box = boneCollider.GetOrAddComponent<BoxCollider>();
				box.size = new Vector2(Mathf.Max(size.x, 0.001F), Mathf.Max(size.y, 0.001F));
				box.center = new Vector2(l + r, u + d) / 2;
			}
		}
		
		void GenerateCircleCollider2D(GameObject boneCollider, Vector2[] points) {
			float l = float.MaxValue;
			float r = float.MinValue;
			float d = float.MaxValue;
			float u = float.MinValue;
			
			for (int i = 0; i < points.Length; i++) {
				if (points[i].x < l) {
					l = points[i].x;
				}
				
				if (points[i].x > r) {
					r = points[i].x;
				}
				
				if (points[i].y < d) {
					d = points[i].y;
				}
				
				if (points[i].y > u) {
					u = points[i].y;
				}
			}
			
			float radius = Mathf.Max(Mathf.Abs(l - r), Mathf.Abs(d - u)) / 2 * physicsOptions.colliderSize;
			if (radius >= 0.001F) {
				CircleCollider2D circle2D = boneCollider.GetOrAddComponent<CircleCollider2D>();
				circle2D.radius = radius;
				circle2D.offset = new Vector2(l + r, u + d) / 2;
			}
		}
		
		void GenerateBoxCollider2D(GameObject boneCollider, Vector2[] points) {
			float l = float.MaxValue;
			float r = float.MinValue;
			float d = float.MaxValue;
			float u = float.MinValue;
			
			for (int i = 0; i < points.Length; i++) {
				if (points[i].x < l) {
					l = points[i].x;
				}
				
				if (points[i].x > r) {
					r = points[i].x;
				}
				
				if (points[i].y < d) {
					d = points[i].y;
				}
				
				if (points[i].y > u) {
					u = points[i].y;
				}
			}
			
			Vector2 size = new Vector2(Mathf.Abs(l - r), Mathf.Abs(d - u)) * physicsOptions.colliderSize;
			if (size.x >= 0.055F || size.y >= 0.055F) {
				BoxCollider2D box2D = boneCollider.GetOrAddComponent<BoxCollider2D>();
				box2D.size = new Vector2(Mathf.Max(size.x, 0.055F), Mathf.Max(size.y, 0.055F));
				box2D.offset = new Vector2(l + r, u + d) / 2;
			}
		}
		
		void GeneratePolygonCollider2D(GameObject boneCollider, Vector2[] points) {
			float previousPointDistanceMultiplier = Random.Range(0.5F, 2F);
			float centerDistanceMultiplier = Random.Range(0.5F, 2F);
			float wrongDirectionMultiplier = Random.Range(0.5F, 2F);
			float minDistance = Random.Range(0.001F, 0.1F);
			
			List<Vector2> path = new List<Vector2>();
			List<Vector2> remainingPoints = new List<Vector2>(points);
			
			// Find center point
			Vector2 centerPoint = Vector2.zero;
			
			foreach (Vector2 point in points) {
				centerPoint += point;
			}
			
			centerPoint /= points.Length;
			
			// Find first point
			Vector2 firstPoint = new Vector2(float.MaxValue, 0);
			
			foreach (Vector2 point in remainingPoints) {
				if (point.x < firstPoint.x) {
					firstPoint = point;
				}
			}
			
			path.Add(remainingPoints.Pop(firstPoint));
			
			// Going up-left
			Vector2 bestPoint = -firstPoint;
			
			while (true) {
				float bestScore = float.MaxValue;
				bestPoint = firstPoint;
				
				for (int i = remainingPoints.Count - 1; i >= 0; i--) {
					Vector2 point = remainingPoints[i];
					float distanceFromPreviousPoint = Vector2.Distance(point, path.Last());
					
					if (distanceFromPreviousPoint < minDistance) {
						remainingPoints.Remove(point);
					}
					else if (point.y > path.Last().y) {
						float score = Vector2.Distance(point, path.Last()) * previousPointDistanceMultiplier;
						score -= Vector2.Distance(point, centerPoint) * centerDistanceMultiplier;
						score += point.x - path.Last().x * wrongDirectionMultiplier;
						
						if (score < bestScore) {
							bestScore = score;
							bestPoint = point;
						}
					}
				}
				
				if (bestPoint == firstPoint) {
					break;
				}
				
				path.Add(remainingPoints.Pop(bestPoint));
			}
			
			// Going right-up
			bestPoint = -firstPoint;
			
			while (true) {
				float bestScore = float.MaxValue;
				bestPoint = firstPoint;
				
				for (int i = remainingPoints.Count - 1; i >= 0; i--) {
					Vector2 point = remainingPoints[i];
					float distanceFromPreviousPoint = Vector2.Distance(point, path.Last());
					
					if (distanceFromPreviousPoint < minDistance) {
						remainingPoints.Remove(point);
					}
					else if (point.x > path.Last().x) {
						float score = Vector2.Distance(point, path.Last()) * previousPointDistanceMultiplier;
						score -= Vector2.Distance(point, centerPoint) * centerDistanceMultiplier;
						score += path.Last().y - point.y * wrongDirectionMultiplier;
						
						if (score < bestScore) {
							bestScore = score;
							bestPoint = point;
						}
					}
				}
				
				if (bestPoint == firstPoint) {
					break;
				}
				
				path.Add(remainingPoints.Pop(bestPoint));
			}
			
			// Going down-right
			bestPoint = -firstPoint;
			
			while (true) {
				float bestScore = float.MaxValue;
				bestPoint = firstPoint;
				
				for (int i = remainingPoints.Count - 1; i >= 0; i--) {
					Vector2 point = remainingPoints[i];
					float distanceFromPreviousPoint = Vector2.Distance(point, path.Last());
					
					if (distanceFromPreviousPoint < minDistance) {
						remainingPoints.Remove(point);
					}
					else if (point.y < path.Last().y) {
						float score = Vector2.Distance(point, path.Last()) * previousPointDistanceMultiplier;
						score -= Vector2.Distance(point, centerPoint) * centerDistanceMultiplier;
						score += path.Last().x - point.x * wrongDirectionMultiplier;
						if (score < bestScore) {
							bestScore = score;
							bestPoint = point;
						}
					}
				}
				
				if (bestPoint == firstPoint) {
					break;
				}
				
				path.Add(remainingPoints.Pop(bestPoint));
			}
			
			// Going left-down
			bestPoint = -firstPoint;
			
			while (true) {
				float bestScore = float.MaxValue;
				bestPoint = firstPoint;
				
				for (int i = remainingPoints.Count - 1; i >= 0; i--) {
					Vector2 point = remainingPoints[i];
					float distanceFromPreviousPoint = Vector2.Distance(point, path.Last());
					
					if (distanceFromPreviousPoint < minDistance) {
						remainingPoints.Remove(point);
					}
					else if (point.x < path.Last().x) {
						float score = distanceFromPreviousPoint * previousPointDistanceMultiplier;
						score -= Vector2.Distance(point, centerPoint) * centerDistanceMultiplier;
						score += point.y - path.Last().y * wrongDirectionMultiplier;
						
						if (score < bestScore) {
							bestScore = score;
							bestPoint = point;
						}
					}
				}
				
				if (bestPoint == firstPoint) {
					break;
				}
				
				path.Add(remainingPoints.Pop(bestPoint));
			}
			
			if (path.Count > 2) {
				PolygonCollider2D polygon2D = boneCollider.GetOrAddComponent<PolygonCollider2D>();
				polygon2D.SetPath(0, path.ToArray());
			}
		}

		void DestroyBoneColliders() {
			foreach (Collider collider in model.GetComponentsInChildren<Collider>()) {
				collider.Remove();
			}
			
			foreach (Collider2D collider in model.GetComponentsInChildren<Collider2D>()) {
				collider.Remove();
			}
		}

		void SetLayer() {
			model.gameObject.layer = 27;
			
			foreach (GameObject child in model.GetChildrenRecursive()) {
				child.layer = 27;
			}
		}
	}
}
