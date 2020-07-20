using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerController : Agent
{
	[SerializeField] float speed = 2f;
	[SerializeField] Tilemap winSpotTileMap = null;
	[SerializeField] Tile winTile = null;
	[SerializeField] float resetTimer = 10f;

	Vector2 winPos;
	float startTime;

	Vector2 positiveVelocity, negativeVelocity, startpos = new Vector2(3, 1);
	Vector3Int leftSpot, rightSpot;

	private void Update()
	{
		if (Time.time > startTime + resetTimer)
		{
			AddReward(-1f);
			Reset();
		}
	}

	private void Reset()
	{
		Debug.Log("Total Reward <color=green>" + GetCumulativeReward()+ "</color>");
		EndEpisode();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Win")
		{
			AddReward(1f);
			Reset();
		}
		else if (collision.gameObject.tag == "Lose")
		{
			AddReward(-0.9f);
			Reset();
		}
	}

	public override void Initialize()
	{
		positiveVelocity = new Vector2(speed, 0);
		negativeVelocity = new Vector2(-speed, 0);
		leftSpot = new Vector3Int(1, 1, 0);
		rightSpot = new Vector3Int(5, 1, 0);
	}

	public override void OnActionReceived(float[] vectorAction)
	{
		if (Mathf.FloorToInt(vectorAction[0]) == 1)
		{
			GetComponent<Rigidbody2D>().velocity = positiveVelocity;
		}
		else if (Mathf.FloorToInt(vectorAction[0]) == 2)
		{
			GetComponent<Rigidbody2D>().velocity = negativeVelocity;
		}
		else
		{
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}

	public override void OnEpisodeBegin()
	{
		winSpotTileMap.ClearAllTiles();
		int coinFlip = Random.Range(0, 2);

		if (coinFlip == 0)
		{
			winSpotTileMap.SetTile(leftSpot, winTile);
			winPos = new Vector2(1, 1);
		}
		else
		{
			winSpotTileMap.SetTile(rightSpot, winTile);
			winPos = new Vector2(5, 1);
		}

		gameObject.transform.localPosition = startpos;
		startTime = Time.time;
	}

	public override void Heuristic(float[] actionsOut)
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			actionsOut[0] = 1;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			actionsOut[0] = 2;
		}
		else
		{
			actionsOut[0] = 0;
		}
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.localPosition.x - winPos.x);
		sensor.AddObservation(winPos);
	}
}
