/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Custom inspector for the Client.
///
/// </summary>

using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor(typeof(Client))]
	public class ClientEditor : Editor
	{
		#region GUI:
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("qualityOfService"));
			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}