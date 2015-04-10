using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class SliderAttribute : CustomAttributeBase {
	
	public float min;
	public float max = 1;
	
	public SliderAttribute() {
	}
	
	public SliderAttribute(float min, float max) {
		this.min = min;
		this.max = max;
	}
}
