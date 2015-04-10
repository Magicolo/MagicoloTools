using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.EditorTools {
	public static class CustomMenus {

		[MenuItem("Magicolo's Tools/Create/Sprite")]
		static void CreateSprite() {
			if (System.Array.TrueForAll(Selection.objects, selected => !(selected is Texture))) {
				Logger.LogError("No sprites were selected.");
				return;
			}
			
			for (int i = 0; i < Selection.objects.Length; i++) {
				Texture texture = Selection.objects[i] as Texture;
			
				if (texture == null) {
					continue;
				}
			
				string textureName = texture.name.EndsWith("Texture") ? texture.name.Substring(0, texture.name.Length - "Texture".Length) : texture.name;
				string texturePath = AssetDatabase.GetAssetPath(texture);
				string materialPath = Path.GetDirectoryName(texturePath) + "/" + textureName + ".mat";
		
				Sprite sprite = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Sprite)) as Sprite;
			
				if (sprite == null) {
					Logger.LogError(string.Format("Texture {0} must be imported as a sprite.", texture.name));
					continue;
				}
			
				AssetDatabase.CopyAsset(HelperFunctions.GetAssetPath("GraphicsTools/SpriteMaterial.mat"), materialPath);
				AssetDatabase.Refresh();
		
				Material material = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material)) as Material;
		
				GameObject gameObject = new GameObject(textureName);
				GameObject child = gameObject.AddChild("Sprite");
				SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();
		
				spriteRenderer.sprite = sprite;
				spriteRenderer.material = material;
		
				PrefabUtility.CreatePrefab(Path.GetDirectoryName(texturePath) + "/" + textureName + ".prefab", gameObject);
				AssetDatabase.Refresh();
		
				gameObject.Remove();
			}
		}
	
		[MenuItem("Magicolo's Tools/Create/Particle")]
		static void CreateParticle() {
			if (System.Array.TrueForAll(Selection.objects, selected => !(selected is Texture))) {
				Logger.LogError("No textures were selected.");
				return;
			}
			
			for (int i = 0; i < Selection.objects.Length; i++) {
				Texture texture = Selection.objects[i] as Texture;
			
				if (texture == null) {
					continue;
				}
				
				string textureName = texture.name.EndsWith("Texture") ? texture.name.Substring(0, texture.name.Length - "Texture".Length) : texture.name;
				string texturePath = AssetDatabase.GetAssetPath(texture);
				string materialPath = Path.GetDirectoryName(texturePath) + "/" + textureName + ".mat";
				
				AssetDatabase.CopyAsset(HelperFunctions.GetAssetPath("GraphicsTools/ParticleMaterial.mat"), materialPath);
				AssetDatabase.Refresh();
				
				Material material = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material)) as Material;
				material.mainTexture = texture;
			}
		}
	}
}
