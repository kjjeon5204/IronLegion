using UnityEngine;
using System.Collections;

public class ClickSpriteCAMERA : MonoBehaviour {
	
	public struct GetFrontmostRaycastHitResult
	{
		public RaycastHit2D rayCastHit2D;
		public bool nothingClicked;
	}
	
	private GameObject leftClickedObject;
	private GetFrontmostRaycastHitResult frontmostRaycastHit;
	private bool showDebug = true;
	private SpriteRenderer spriteRenderer;
	
	void Update () 
	{
		// If the left mouse button is clicked anywhere...
		if (Input.GetMouseButtonDown(0))
		{
			// frontmostRaycastHit stores information about the RaycastHit2D that is returned by GetFrontmostRaycastHit()
			frontmostRaycastHit = GetFrontmostRaycastHit();
			
			// If frontmostRaycastHit is true, i,e the user hasn't clicked on nothing, i.e GetFrontmostRaycastHit() didn't return nothing...
			if (frontmostRaycastHit.nothingClicked == false)
			{
				// Assigns the game object that the collider that has been clicked on to leftClickedObject.
				leftClickedObject = frontmostRaycastHit.rayCastHit2D.collider.gameObject;
				
				// Sends the frontmostRaycast to a function called OnLeftClick in a script attached to whatever the leftClickedObject is.
				leftClickedObject.SendMessage("OnClick", frontmostRaycastHit, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	GetFrontmostRaycastHitResult GetFrontmostRaycastHit()
	{
		
		GetFrontmostRaycastHitResult result = new GetFrontmostRaycastHitResult();
		
		// Store the point where the user has clicked as a Vector3.
		Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Retrieve all raycast hits from the click position and store them in an array called "hits".
		
		RaycastHit2D[] hits = Physics2D.LinecastAll (clickPosition, clickPosition);
		
		// If the raycast hits something...
		if (hits.Length != 0)
		{
			// A variable that will store the frontmost sorting layer that contains an object that has been clicked on as an int.
			int topSortingLayer = 0;
			// A variable that will store the index of the top sorting layer as an int.
			int indexOfTopSortingLayer;
			// An array that stores the IDs of all the sorting layers that contain a sprite in the path of the linecast.
			int[] sortingLayerIDArray = new int[hits.Length];
			// An array that stores the sorting orders of each sprite that has been hit by the linecast
			int[] sortingOrderArray = new int[hits.Length];
			// An array that stores the sorting order number of the frontmost sprite that has been clicked.
			int topSortingOrder = 0;
			// A variable that will store the index in the sortingOrderArray where topSortingOrder is. This index used with the hits array will give us our frontmost clicked sprite.
			int indexOfTopSortingOrder = 0;
			
			// Loop through the array of raycast hits...
			for (var i = 0; i < hits.Length; i++)
			{
				// Get the SpriteRenderer from each game object under the click.
				spriteRenderer = hits[i].collider.gameObject.GetComponent<SpriteRenderer>();
				
				// Access the sortingLayerID through the SpriteRenderer and store it in the sortingLayerIDArray.
				sortingLayerIDArray[i] = spriteRenderer.sortingLayerID;
				
				// Access the sortingOrder through the SpriteRenderer and store it in the sortingOrderArray.
				sortingOrderArray[i] = spriteRenderer.sortingOrder;
			}
			
			// Loop through the array of sprite sorting layer IDs...
			for (int j = 0; j < sortingLayerIDArray.Length; j++)
			{
				// If the sortingLayerID is higher that the topSortingLayer...
				if (sortingLayerIDArray[j] >= topSortingLayer)
				{
					topSortingLayer = sortingLayerIDArray[j];
					indexOfTopSortingLayer = j;
				}
			}
			
			// Loop through the array of sprite sorting orders...
			for (int k = 0; k < sortingOrderArray.Length; k++)
			{
				// If the sorting order of the sprite is higher than topSortingOrder AND the sprite is on the top sorting layer...
				if (sortingOrderArray[k] >= topSortingOrder && sortingLayerIDArray[k] == topSortingLayer)
				{
					topSortingOrder = sortingOrderArray[k];
					indexOfTopSortingOrder = k; //Set index of sprite that is on top layer
				}
				else
				{
					// Do nothing and continue loop.
				}
			}
			
			// How many sprites with colliders attached are underneath the click?
			if (showDebug) Debug.Log("How many sprites have been clicked on: " + hits.Length);
			// Which is the sorting layer of the frontmost clicked sprite?
			if (showDebug)Debug.Log("Frontmost sorting layer ID: "+ topSortingLayer);
			// Which is the order in that sorting layer of the frontmost clicked sprite?
			if (showDebug)Debug.Log("Frontmost order in layer: "+ topSortingOrder);
			
			// The indexOfTopSortingOrder will also be the index of the frontmost raycast hit in the array "hits". 
			result.rayCastHit2D = hits[indexOfTopSortingOrder];
			result.nothingClicked = false;
			return result;
		}
		else // If the hits array has a length of 0 then nothing has been clicked...
		{
			if (showDebug)Debug.Log("Nothing clicked.");
			result.nothingClicked = true;
			return result;
		}
	}
}