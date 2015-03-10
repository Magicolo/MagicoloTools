using System;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public static class PureDataPluginManager {

		public static string LibpdcsharpPath {
			get {
				return Application.dataPath + HelperFunctions.GetAssetPath("Plugins/libpdcsharp.dll").Substring("Assets".Length);
			}
		}
		
		public static string PthreadGC2Path {
			get {
				return Application.dataPath + HelperFunctions.GetAssetPath("Plugins/pthreadGC2.dll").Substring("Assets".Length);
			}
		}
		
		public static string UnityDirectory {
			get {
				string directory = "";
				#if UNITY_EDITOR
				directory = Path.GetDirectoryName(UnityEditor.EditorApplication.applicationPath);
				#endif
				return directory;
			}
		}
		
		public static void ResolvePath() {
			#if !UNITY_WEBPLAYER
			try {
				string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
				string dllPath = (Application.dataPath + "/" + "Plugins").Replace('/', Path.AltDirectorySeparatorChar);
		
				if (!currentPath.Contains(dllPath)) {
					Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
				}
			}
			catch {
			}
			#endif
		}
		
		public static void CheckPlugins() {
			string libpdcsharpTargetPath = UnityDirectory + Path.AltDirectorySeparatorChar + "libpdcsharp.dll";
			string pthreadGC2TargetPath = UnityDirectory + Path.AltDirectorySeparatorChar + "pthreadGC2.dll";

			try {
				if (!File.Exists(libpdcsharpTargetPath)) {
					File.Copy(LibpdcsharpPath, libpdcsharpTargetPath);
					Logger.Log(string.Format("libpdcsharp.dll has been added to {0}.", UnityDirectory));
				}
			
				if (!File.Exists(pthreadGC2TargetPath)) {
					File.Copy(PthreadGC2Path, pthreadGC2TargetPath);
					Logger.Log(string.Format("pthreadGC2.dll has been added to {0}.", UnityDirectory));
				}
			}
			catch {
			}
		}
		
		#if UNITY_EDITOR
		[UnityEditor.Callbacks.PostProcessBuild]
		public static void OnPostProcessBuild(UnityEditor.BuildTarget buildTarget, string buildPath) {
			CheckPlugins();
		}
		
		[UnityEditor.Callbacks.DidReloadScripts]
		public static void OnReloadScripts() {
			CheckPlugins();
		}
		#endif
	}
}