using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour
{
	private static Node parentNode;

	[MenuItem( "GameObject/Connect Nodes #c" )]
	public static void ConnectNodes( )
	{
		parentNode = null;
		parentNode = Selection.activeGameObject.GetComponent<Node>( );

		if (parentNode == null)
		{
			Debug.LogWarning( "Can't start a connection. Selected GameObject needs a Node class attached to it." );
			return;
		}

		Selection.selectionChanged += OnSelectionChanged;
		Debug.Log( "Starting connection mode. Parent: " + Selection.activeGameObject.name + ". Please select child node." );
	}

	private static void OnSelectionChanged( )
	{
		Selection.selectionChanged -= OnSelectionChanged;

		Node childNode = null;
		childNode = Selection.activeGameObject.GetComponent<Node>( );

		if ( childNode == null )
		{
			Debug.LogWarning( "Can't make a connection. Selected GameObject needs a Node class attached to it." );
			return;
		}

		List<GameObject> oldNextNodes = new List<GameObject>(parentNode.nextNodes);
		oldNextNodes.Add( Selection.activeGameObject );
		parentNode.nextNodes = oldNextNodes.ToArray( );

		Debug.Log( "New connection between Nodes made: " + parentNode.name + "->" + Selection.activeGameObject.name + "." );
	}
}
