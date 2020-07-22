using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	[System.Serializable]
	public class Cell
	{
		public bool visited = false;
		public GameObject north;
		public GameObject east;
		public GameObject west;
		public GameObject south;
	}

	[SerializeField] GameObject wall = null;
	[SerializeField] float wallLength = 1f;
	[SerializeField] int xSize = 5;
	[SerializeField] int ySize = 5;
	[SerializeField] Cell[] cells;
	private Vector3 startingPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
		CreateWalls();
    }

	void CreateWalls()
	{
		startingPos = new Vector3((-xSize / 2) + wallLength / 2, 0, (-ySize / 2) + wallLength / 2);

		Vector3 myPos = startingPos;

		for (int y = 0; y < ySize; y++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				myPos.x = startingPos.x + (x * wallLength) - wallLength / 2;
				myPos.z = startingPos.z + (y * wallLength) - wallLength / 2;

				GameObject newWall = Instantiate(wall, transform);
				newWall.transform.localPosition = myPos;


			}
		}

		for (int y = 0; y <= ySize; y++)
		{
			for (int x = 0; x < xSize; x++)
			{
				myPos.x = startingPos.x + (x * wallLength);
				myPos.z = startingPos.z + (y * wallLength) - wallLength;

				GameObject newWall = Instantiate(wall, transform);
				newWall.transform.localPosition = myPos;
				newWall.transform.rotation = Quaternion.Euler(0, 90, 0);
			}
		}

		CreateCells();
	}

	void CreateCells()
	{
		GameObject[] allWalls = new GameObject[transform.childCount];
		cells = new Cell[xSize*ySize];

		for (int i = 0; i < transform.childCount; i++)
		{
			allWalls[i] = transform.GetChild(i).gameObject;
		}


		// Assign walls to cells
		int rowNumber = 0;
		int southWall = (xSize + 1) * ySize;
		int northWall = ((xSize + 1) * ySize) + xSize;


		for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
		{
			if (cellProcess % xSize == 0 && cellProcess != 0) rowNumber++;

			cells[cellProcess] = new Cell();
			cells[cellProcess].east = allWalls[cellProcess + 1 + rowNumber];
			cells[cellProcess].west = allWalls[cellProcess + rowNumber];
			cells[cellProcess].north = allWalls[cellProcess + northWall];
			cells[cellProcess].south = allWalls[cellProcess + southWall];
		}

		Debug.Log(allWalls.Length);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
