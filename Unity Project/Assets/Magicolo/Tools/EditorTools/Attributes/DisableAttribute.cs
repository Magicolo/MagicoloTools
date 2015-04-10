using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DisableAttribute : CustomAttributeBase {
	
	public DisableAttribute() {
		DisableOnPlay = true;
		DisableOnStop = true;
	}
}
