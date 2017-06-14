using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CanDeconstruct : MonoBehaviour
{
	[SerializeField] private Button button = null;

	void Start( )
	{
		Assert.IsNotNull( button );
	}

	public void TryCanDeconstruct( )
	{
		if ( GameController.instance.CanDeconstructTower( ) == false )
			button.interactable = false;
		else
			button.interactable = true;
	}
}
