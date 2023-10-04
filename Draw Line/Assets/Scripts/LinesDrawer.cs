using UnityEngine;

public class LinesDrawer : MonoBehaviour 
{

	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;

	[Space ( 30f )]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

	Line currentLine;

	Camera cam;


	void Start ( ) 
	{
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer ( "CantDrawOver" );
	}

	void Update ( ) 
	{
		if ( Input.GetMouseButtonDown ( 0 ) )
			BeginDraw ( );

		if ( currentLine != null )
			Draw ( );

		if ( Input.GetMouseButtonUp ( 0 ) )
			EndDraw ( );
	}

	void BeginDraw ( ) 
	{
		currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );

		currentLine.UsePhysics ( false );
		currentLine.SetLineColor ( lineColor );
		currentLine.SetPointsMinDistance ( linePointsMinDistance );
		currentLine.SetLineWidth ( lineWidth );

	}

	void Draw ( ) 
	{
		Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );

		RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

		if ( hit )
			EndDraw ( );
		else
			currentLine.AddPoint ( mousePosition );
	}

	void EndDraw ( ) 
	{
		if ( currentLine != null ) 
		{
			if ( currentLine.pointsCount < 2 ) 
			{
				Destroy ( currentLine.gameObject );
			}
			else 
			{
				currentLine.gameObject.layer = cantDrawOverLayerIndex;

				currentLine.UsePhysics ( true );

				currentLine = null;
			}
		}
	}
}
