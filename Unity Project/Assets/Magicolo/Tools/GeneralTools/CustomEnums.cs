using System;

namespace Magicolo {

	[Flags]
	public enum Axis {
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		XY = 3,
		XZ = 5,
		YZ = 6,
		XYZ = 7,
		W = 8,
		XW = 9,
		YW = 10,
		XYW = 11,
		ZW = 12,
		XZW = 13,
		YZW = 14,
		XYZW = 15
	}
	
	[Flags]
	public enum Channels {
		None = 0,
		R = 1,
		G = 2,
		B = 4,
		RG = 3,
		RB = 5,
		GB = 6,
		RGB = 7,
		A = 8,
		RA = 9,
		GA = 10,
		RGA = 11,
		BA = 12,
		RBA = 13,
		GBA = 14,
		RGBA = 15
	}
		
	public enum InterpolationModes {
		Quadratic,
		Linear
	}
}

