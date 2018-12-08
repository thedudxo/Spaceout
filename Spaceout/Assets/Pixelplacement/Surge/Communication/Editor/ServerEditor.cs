/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Custom inspector for the Server.
///
/// </summary>

using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor(typeof(Server))]
	public class ServerEditor : Editor
	{
		#region GUI:
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("port"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("qualityOfService"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("initialBandwidth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("customDeviceId"));

			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}