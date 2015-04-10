using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class RendererExtensions {

		public static Color GetColor(this Renderer renderer, bool shared) {
			SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
			Color color;
			
			if (spriteRenderer != null && spriteRenderer.sharedMaterial == null) {
				color = ((SpriteRenderer)renderer).color;
			}
			else if (shared) {
				color = renderer.sharedMaterial.color;
			}
			else {
				color = renderer.material.color;
			}
			
			return color;
		}

		public static Color GetColor(this Renderer renderer) {
			return renderer.GetColor(false);
		}
		
		public static void SetColor(this Renderer renderer, Color color, bool shared, Channels channels = Channels.RGBA) {
			SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
			
			if (spriteRenderer != null && spriteRenderer.sharedMaterial == null) {
				spriteRenderer.color = spriteRenderer.color.SetValues(color, channels);
			}
			else if (shared) {
				renderer.sharedMaterial.SetColor(color, channels);
			}
			else {
				renderer.material.SetColor(color, channels);
			}
		}

		public static void SetColor(this Renderer renderer, Color color, Channels channels = Channels.RGBA) {
			renderer.SetColor(color, false, channels);
		}
		
		public static void SetColor(this Renderer renderer, float color, bool shared, Channels channels = Channels.RGBA) {
			renderer.SetColor(new Color(color, color, color, color), shared, channels);
		}

		public static void SetColor(this Renderer renderer, float color, Channels channels = Channels.RGBA) {
			renderer.SetColor(new Color(color, color, color, color), false, channels);
		}
		
		public static void Fade(this Renderer renderer, Color fade, bool shared, Channels channels = Channels.RGBA) {
			renderer.SetColor(renderer.GetColor(shared) + fade * Time.deltaTime, channels);
		}

		public static void Fade(this Renderer renderer, Color fade, Channels channels = Channels.RGBA) {
			renderer.Fade(fade, false, channels);
		}
		
		public static void Fade(this Renderer renderer, float fade, bool shared, Channels channels = Channels.RGBA) {
			renderer.Fade(new Color(fade, fade, fade, fade), shared, channels);
		}

		public static void Fade(this Renderer renderer, float fade, Channels channels = Channels.RGBA) {
			renderer.Fade(new Color(fade, fade, fade, fade), false, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, Color targetColor, float speed, InterpolationModes interpolation, bool shared, Channels channels = Channels.RGBA) {
			switch (interpolation) {
				case InterpolationModes.Quadratic:
					renderer.SetColor(renderer.GetColor().Lerp(targetColor, Time.deltaTime * speed, channels), shared, channels);
					break;
				case InterpolationModes.Linear:
					renderer.SetColor(renderer.GetColor().LerpLinear(targetColor, Time.deltaTime * speed, channels), shared, channels);
					break;
			}
		}
		
		public static void FadeTowards(this Renderer renderer, Color targetColor, float speed, InterpolationModes interpolation, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(targetColor, speed, interpolation, false, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, Color targetColor, float speed, bool shared, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(targetColor, speed, InterpolationModes.Quadratic, shared, channels);
		}

		public static void FadeTowards(this Renderer renderer, Color targetColor, float speed, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(targetColor, speed, false, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, float targetColor, float speed, InterpolationModes interpolation, bool shared, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(new Color(targetColor, targetColor, targetColor, targetColor), speed, interpolation, shared, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, float targetColor, float speed, InterpolationModes interpolation, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(new Color(targetColor, targetColor, targetColor, targetColor), speed, interpolation, false, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, float targetColor, float speed, bool shared, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(new Color(targetColor, targetColor, targetColor, targetColor), speed, InterpolationModes.Quadratic, shared, channels);
		}
		
		public static void FadeTowards(this Renderer renderer, float targetColor, float speed, Channels channels = Channels.RGBA) {
			renderer.FadeTowards(new Color(targetColor, targetColor, targetColor, targetColor), speed, InterpolationModes.Quadratic, false, channels);
		}
		
		public static void OscillateColor(this Renderer renderer, Color frequency, Color amplitude, Color center, bool shared, Channels channels = Channels.RGBA) {
			renderer.SetColor(renderer.GetColor().Oscillate(frequency, amplitude, center, renderer.GetInstanceID() / 1000), shared, channels);
		}

		public static void OscillateColor(this Renderer renderer, Color frequency, Color amplitude, Color center, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(frequency, amplitude, center, false, channels);
		}
		
		public static void OscillateColor(this Renderer renderer, Color frequency, Color amplitude, bool shared, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(frequency, amplitude, Color.white * 0.5F, shared, channels);
		}

		public static void OscillateColor(this Renderer renderer, Color frequency, Color amplitude, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(frequency, amplitude, Color.white * 0.5F, false, channels);
		}
		
		public static void OscillateColor(this Renderer renderer, float frequency, float amplitude, float center, bool shared, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), new Color(center, center, center, center), shared, channels);
		}

		public static void OscillateColor(this Renderer renderer, float frequency, float amplitude, float center, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), new Color(center, center, center, center), false, channels);
		}
		
		public static void OscillateColor(this Renderer renderer, float frequency, float amplitude, bool shared, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), Color.white * 0.5F, shared, channels);
		}

		public static void OscillateColor(this Renderer renderer, float frequency, float amplitude, Channels channels = Channels.RGBA) {
			renderer.OscillateColor(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), Color.white * 0.5F, false, channels);
		}
		
		public static bool IsVisibleFrom(this Renderer renderer, Camera camera) {
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}
}
