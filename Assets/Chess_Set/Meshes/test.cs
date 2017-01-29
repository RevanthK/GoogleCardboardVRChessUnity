using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class test : MonoBehaviour {


	Meteor.Collection<DocumentType> col;
	Meteor.Collection<DocumentType2> col2;
	Meteor.Collection<DocumentType3> col3;

	// Use this for initialization
	void Start () {
		StartCoroutine (MeteorExample ());

	}

	IEnumerator MeteorExample(){
		yield return Meteor.Connection.Connect ("ws://localhost:3000/websocket");

		var subscription = Meteor.Subscription.Subscribe ("board", 1);
	    yield return (Coroutine)subscription;



		col = new Meteor.Collection<DocumentType> ("ascii");
		col2 = new Meteor.Collection<DocumentType2> ("status");
		col3 = new Meteor.Collection<DocumentType3> ("moves");

		Debug.Log (col.FindOne ().ascii);
		movePieces (col.FindOne().ascii);
		GameObject xyz = findObj ("a1");
		Debug.Log (xyz);

	
		bool gameOver = (col2.FindOne().status == "true");

		//update piece positions

		var constStart = -.245;
		var constInc = .07;

		var i = 0;
		var j = 0;

		/*foreach (string[] sArray in col.FindOne().ascii) {
			j = 0;
			foreach (string s in sArray) {
				var xPos = constStart + i * constInc;
				var yPos = constStart + j * constInc;
				//Debug.Log ("x: " + xPos + " y: " + yPos);
				j++;
			}
			i++;
		}*/

		if (!gameOver) {
			//Debug.Log (col2.FindOne ().status);
			//Ask user to move


			//check move with possible moves
			string[] possMoves = col3.FindOne().moves;

		}
		else{
			Debug.Log ("game over !");
		}
	
		var methodCall = Meteor.Method.Call ("movePiece", "a2a3");

		// Execute the method. This will yield until all the database side effects have synced.
		yield return (Coroutine)methodCall;


		//foreach(string s in col3.FindOne().moves)
		//	Debug.Log (s);

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (col.FindOne ().ascii);
		//string[][] board = col.FindOne().ascii;

		//movePieces (board);

		//Meteor.Method.Call("movePiece", "a2a3");
	
	}



	public class DocumentType: Meteor.MongoDocument {
		public string ascii;
		public int _id;
		public string element;
	}
	public class DocumentType2: Meteor.MongoDocument {
		public string status;
		public int _id;
		public string element;
	}
	public class DocumentType3: Meteor.MongoDocument {
		public string[] moves;
		public int _id;
		public string element;
	}

    GameObject findObj(string loc){
		int x = (int)loc.Substring (0, 1).ToCharArray()[0] - 97;
		int z = Int32.Parse(loc.Substring (1)) - 1;
		var xPos = -.245 + x * .07;
		var zPos = -.245 + z * .07;
		Debug.Log ("x: " + xPos + " y: " + zPos);
		foreach (Transform child in transform){
			//Debug.Log (child.gameObject.name);
			//Debug.Log(child.position.x + " " + child.position.y);
			if (child.gameObject.name.Equals("Chess_Rock_B1")){
				//Debug.Log(child.position.x);
				//Debug.Log ("x: " + child.position.x + " y: " + child.position.z);

				//Debug.Log(child.position);
			}
			if (child.gameObject.name.Equals("Chess_Rock_A1")){
				//Debug.Log(child.position.x);
				//Debug.Log ("x: " + child.position.x + " y: " + child.position.z);


				//Debug.Log(child.position);
			}
			///if (child.position.x == xPos && child.position.z == zPos && !child.gameObject.name.Equals("Chess_Checker") && !child.gameObject.name.Equals("Chess_Frame")){
			if (child.position.x - xPos <= 0.01 && child.position.z - zPos <= 0.01 && !child.gameObject.name.Equals("Chess_Checker") && !child.gameObject.name.Equals("Chess_Frame")){
				Debug.Log (child.position.x);
				Debug.Log(child.gameObject);
			}
		}
		return null;

	}

	void movePieces(string board){

		//GameObject piece = Instantiate(Resources.Load("Chess_Queen_A", typeof(GameObject))) as GameObject;
		//Debug.Log (piece);
		//Component[] hingeJoints = piece.GetComponentInChildren<Component> ();
		//hingeJoints[0].transform.Translate(new Vector3((float)0.5, (float)0.0, (float)0.5));
		//piece.BroadcastMessage ("checkMove");

		//transform.Translate (0,2,0);

		foreach (Transform child in transform){
			//Debug.Log (child.gameObject.name);
			if (child.gameObject.name.Equals("Chess_Checker") || child.gameObject.name.Equals("Chess_Frame")){
				continue;
			}
			//child.Translate (0, -1, 0);
		}
			
	}

}
