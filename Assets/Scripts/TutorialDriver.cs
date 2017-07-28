/**
 * Description:
 *
 * Authors: Kornel
 *
 * Copyright: © 2017 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 *
 * TODO:
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class TutorialDriver : MonoBehaviour
{
	[SerializeField] GameObject[] tips;
	[SerializeField] GameObject scripts;
	[SerializeField] GameObject tutorialScreen;
	[SerializeField] GameObject tutorialDriver;

	private int currentTip = 0;

	void Start ()
	{
		Assert.IsNotNull( tips );
		Assert.IsNotNull( scripts );
		Assert.IsNotNull( tutorialScreen );
		Assert.IsNotNull( tutorialDriver );

		foreach ( var tip in tips )
		{
			tip.SetActive( false );
		}

		tips[currentTip].SetActive( true );
		scripts.SetActive( false );
	}

	public void OnClick( )
	{
		tips[currentTip].SetActive( false );
		currentTip++;

		if (currentTip >= tips.Length)
		{
			scripts.SetActive( true );
			tutorialScreen.SetActive( false );
			tutorialDriver.SetActive( false );
		}
		else
		{
			tips[currentTip].SetActive( true );
		}
	}
}
