using UnityEngine;
using UnityEngine.Assertions;

public class MaterialPulse : MonoBehaviour
{
	[SerializeField] private float duration = 1f;
	[SerializeField] private float minValue = 0.8f;
	[SerializeField] private float maxValue = 1f;

	private Renderer rend;
	private Material matModel;
	private Color matColor;

	void Awake ()
	{
		GetMaterial( );
		matColor = matModel.GetColor( "_EmissionColor" );
	}

	private void GetMaterial( )
	{
		rend = GetComponent<Renderer>( );
		Assert.IsNotNull( rend );

		matModel = rend.material;
		Assert.IsNotNull( matModel );
	}

	void Update( )
	{
		float lerp = Mathf.PingPong( Time.time, duration ) / duration;
		matColor.g = minValue + ( ( maxValue - minValue ) * lerp );

		matModel.SetColor( "_EmissionColor", matColor );
	}
}
