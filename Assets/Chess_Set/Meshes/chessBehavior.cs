using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class chessBehavior : MonoBehaviour {


	Meteor.Collection<DocumentType> col;
	Meteor.Collection<DocumentType2> col2;
	Meteor.Collection<DocumentType3> col3;
	string move;

	// Use this for initialization
	void Start () {
		StartCoroutine (MeteorExample ());

	}

	IEnumerator MeteorExample(){
		yield return Meteor.Connection.Connect ("ws://vrchess.herokuapp.com/websocket");

		var subscription = Meteor.Subscription.Subscribe ("board", 1);
		yield return (Coroutine)subscription;



		col = new Meteor.Collection<DocumentType> ("ascii");
		col2 = new Meteor.Collection<DocumentType2> ("status");
		col3 = new Meteor.Collection<DocumentType3> ("moves");

		//Debug.Log (col.FindOne ().ascii);

		//Debug.Log (xyz);
		move = "";


		bool gameOver = (col2.FindOne().status == "true");

		//update piece positions

		var constStart = -.245;
		var constInc = .07;

		var i = 0;
		var j = 0;


		//var methodCall = Meteor.Method.Call ("movePiece", "a2a3");
		//yield return (Coroutine)methodCall;

		// Execute the method. This will yield until all the database side effects have synced.



	}

	// Update is called once per frame
	void Update () {


		bool requestMove = false;
	
		if (col2.FindOne().status.Equals("true")) {
			

		} else {

			//updateServer
			if (requestMove) {
				
				//Meteor.Method.Call ("movePiece", "a2a3");

			} 

			//updateBoard

			String s = col.FindOne().ascii;

			if (s != null && !move.Equals (s)) {

				//Debug.Log (s.Substring (0, 2));


				GameObject gm = findObj (s.Substring (0, 2));



				GameObject target = findObj (s.Substring (2));



				if (target != null) {				
					target.transform.Translate (new Vector3 (0, -1, 0));
				}
					

				Vector3 v = findCoord (s.Substring (2));
				//Vector3 v2 = gm.transform.position;



				Vector3 travel = new Vector3 (v.x-gm.transform.position.x, (float)0, v.z-gm.transform.position.z);
				//Debug.Log (travel.x+" "+travel.z);
				gm.transform.Rotate(new Vector3((float)90, (float)0, (float)0));
				gm.transform.Translate (travel);
				gm.transform.Rotate(new Vector3((float)-90, (float)0, (float)0));

				move = s;
			}
		}

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

		Vector3 v = findCoord (loc);

		float xPos = v.x;
		float zPos = v.z;



		foreach (Transform child in transform){
			//Debug.Log (child.position.x + " " + child.position.z);
			//Debug.Log (child.position.x + " " + child.position.z);
			///if (child.position.x == xPos && child.position.z == zPos && !child.gameObject.name.Equals("Chess_Checker") && !child.gameObject.name.Equals("Chess_Frame")){
			if (Math.Abs(child.position.x - xPos) <= 0.001 && Math.Abs(child.position.z - zPos) <= 0.001 &&
				!child.gameObject.name.Equals("Chess_Checker") && !child.gameObject.name.Equals("Chess_Frame")){

				return child.gameObject;
			}
		}
		return null;

	}

	Vector3 findCoord(string loc){
		int x = (int)loc.Substring (0, 1).ToCharArray()[0] - 97;
		Debug.Log (loc.Substring (1));
		int z = Int32.Parse(loc.Substring (1)) - 1;
		var xPos = -.245 + x * .07;
		var zPos = -.245 + z * .07;
		//Debug.Log ("x: " + xPos + " y: " + zPos);
		return new Vector3 ((float)xPos, (float)0, (float)zPos);
	}
	/*
	IEnumerator movePieces(string board){

		//GameObject piece = Instantiate(Resources.Load("Chess_Queen_A", typeof(GameObject))) as GameObject;
		//Debug.Log (piece);
		//Component[] hingeJoints = piece.GetComponentInChildren<Component> ();
		//hingeJoints[0].transform.Translate(new Vector3((float)0.5, (float)0.0, (float)0.5));
		//piece.BroadcastMessage ("checkMove");

		//transform.Translate (0,2,0);

		var constStart = -.245;
		var constInc = .07;

		GameObject xyz = findObj("a1");




		foreach (Transform child in transform){
			//Debug.Log (child.gameObject.name);
			if (child.gameObject.name.Equals("Chess_Checker") || child.gameObject.name.Equals("Chess_Frame")){
				continue;
			}
		
		}

	}*/

}
