using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : MonoBehaviour {

    public BlockControl blockControl;

    private void OnTriggerEnter2D(Collider2D collision)
	{
			Bomb();
			
			StartCoroutine(blockControl.BlockDeath()); // if doesn't work, put block death in this script
	}
	
    public void Bomb()
    {
		/*
			Bomb hit and it then randomly deducts points from blocksToHit(20%) of blocks 
		*/
		
		// min and max points to be deducted from blocks
		int minPoints = 10;
		int maxPoints = 75;
		int blocksToHit = 5; //20% using modulus
		int counter = 0;
		
		GameObject[] block = GameObject.FindGameObjectsWithTag("block"); // Not including super blocks...should I?
        foreach (GameObject b in block)
        {
            if ((b != null)&&(counter % blocksToHit == 0)) 
            {
				b.GetComponentInParent<Block>().hitsRemaining -= (Random.Range(minPoints, maxPoints)); //Reduce block hitsRemaining
			
				//Some kind of visual thing to show it was hit...maybe death, without the destroy?
				//Maybe draw a line between this block and the affected blocks??
				StartCoroutine(BombLinesAndDeath(b));
				StartCoroutine(Flash(b)); 
				
				//Adjust hitsRemainingText
				if(b.GetComponentInParent<Block>().hitsRemaining >=0)
					hitsRemainingText.text = b.GetComponentInParent<Block>().hitsRemaining.ToString();
				else
					hitsRemainingText.text = "0";
				
			}
			counter++;
		}
	}
	
	IEnumerator BombLinesAndDeath(GameObject blockHit)
	{
		Vector3[] linePoints;
		LinneRenderer lineRenderer;
		public Material lineMaterial;
				
		//Setup Line Stuff
		lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        linePoints = new Vector3[2];
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = lineMaterial;
        lineRenderer.material = new Material(Shader.Find("Unlit/Texture"));
		
		//Set line positions, colour and draw
		linePoints[0] = transform.localPosition;
		linePoints[1] = blockHit.transform.localPosition;
		lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
		lineRenderer.SetPositions(linePoints);
		//flash line?
		yield return new WaitForSeconds(0.025f);
		lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.white;
		lineRenderer.SetPositions(linePoints);
		yield return new WaitForSeconds(0.025f);
		lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
		lineRenderer.SetPositions(linePoints);
		yield return new WaitForSeconds(0.025f);
		lineRenderer.positionCount = 0; // destroy line
		
		//Check and update if block destroyed
		if(blockHit.GetComponentInParent<Block>().hitsRemaining >= 0)
		{
			StartCoroutine(blockControl.BlockDeath()); // if doesn't work, put block death in this script
		}
		
		yield return null;
	}
	
	//flash colour on hit
    IEnumerator Flash(GameObject blockHit)
    {
        SpriteRenderer block = blockHit.GetComponent<SpriteRenderer>();
        block.color = Color.white;
        yield return new WaitForSeconds(0.025f);
        // Have to use this as if you store the colour of the block, if might already be white from being hit previously
        block.color = blockHit.GetComponentInParent<Block>().colour;
    }
}
