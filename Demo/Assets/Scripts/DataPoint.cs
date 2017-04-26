using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour {
	public int id;
	public string coordinates = null;
	public int birdTypeQuantity;
	public string[] birdCodes;
	public int[] birdCounts;

	public void sizeArrays(int arraySize) {
		birdTypeQuantity = arraySize;
		birdCodes = new string[arraySize];
		birdCounts = new int[arraySize];
	}

	public int Id {
		get { 
			return id;
		}
		set { 
			id = value;
		}
	}

	public string Coordinates {
		get { 
			return coordinates;
		}
		set { 
			coordinates = value;
		}
	}

	public int BirdTypeQuantity {
		get {
			return birdTypeQuantity;
		}
		set { 
			birdTypeQuantity = value;
		}
	}

	public void SetBirdCode(int index, string code) {
		birdCodes[index] = code;
	}

	public void SetBirdCount(int index, int count) {
		birdCounts [index] = count;
	}

	public string GetBirdCode(int index) {
		return birdCodes [index];
	}

	public int GetBirdCount(int index) {
		return birdCounts [index];
	}
}
	
