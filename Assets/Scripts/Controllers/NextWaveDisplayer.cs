using UnityEngine;
using UnityEngine.Assertions;

public class NextWaveDisplayer : MonoBehaviour
{
	[SerializeField] private RectTransform originPoint = null;
	[Header("Icons")]
	[SerializeField] private GameObject normalEnemyIcon = null;
	[SerializeField] private GameObject fastEnemyIcon = null;
	[SerializeField] private GameObject armoredEnemyIcon = null;

	private GameObject parentGO;

	private const float IncrementX = 25, IncrementY = 25;
	
	void Start ()
	{
		Assert.IsNotNull( originPoint );
		Assert.IsNotNull( normalEnemyIcon );
		Assert.IsNotNull( fastEnemyIcon );
		Assert.IsNotNull( armoredEnemyIcon );
	}
	
	public void DisplayNextWave ()
	{
		string s = "[ ";
		float offsetX = 0, offsetY = 0;

		// Container
		if ( parentGO != null ) Destroy( parentGO );
		parentGO = new GameObject( "Icons" );
		parentGO.transform.SetParent( originPoint );
		parentGO.transform.localPosition = Vector3.zero;

		// Adding icons for every spawn point and every wave
		foreach ( var spawner in UIController.instance.nextWaves )
		{
			foreach ( var waveComponenet in spawner )
			{
				for ( int i = 0; i < waveComponenet.num; i++ )
				{
					s += waveComponenet.enemyPrefab.name + ", ";

					// Not preaty, but gets the job done ;-)
					GameObject icon = normalEnemyIcon;
					if ( waveComponenet.enemyPrefab.name == "Fast Enemy" )
						icon = fastEnemyIcon;
					if ( waveComponenet.enemyPrefab.name == "Armored Enemy" )
						icon = armoredEnemyIcon;

					var go = Instantiate( icon, Vector3.zero, Quaternion.identity, parentGO.transform );
					go.transform.localPosition = new Vector2( offsetX, offsetY );
					offsetX += IncrementX;
				}
			}

			offsetY -= IncrementY;
			offsetX = 0;
		}

		s += " ]";
		Debug.Log( s );
	}
}
