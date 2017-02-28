using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class chessBehavior : MonoBehaviour {


	Meteor.Collection<DocumentType> game;
	string move;
	string userId;
	string color;
	bool listening = false;
	String newMove = "";

	// Use this for initialization
	void Start () {
		GvrViewer.Create();
		GvrViewer.Instance.Recenter ();


		StartCoroutine (MeteorInit ());

	}

	IEnumerator MeteorInit(){
		yield return Meteor.Connection.Connect ("ws://localhost:3000/websocket");

		//Debug.Log (xyz);
		move = "";

		var methodCall = Meteor.Method<string>.Call ("readyToPlay");
		yield return (Coroutine)methodCall;
		userId = methodCall.Response;

		var subscription = Meteor.Subscription.Subscribe ("game", userId);
		yield return (Coroutine)subscription;
		game = new Meteor.Collection<DocumentType> ("games");

		bool gameOver = game.FindOne().gameOver == true;

		color = turnToColor(game.FindOne ().turn);




	}

	// Update is called once per frame
	void Update () {


		if (!move.Equals(game.FindOne().latestMove)){
			makeMove (game.FindOne().latestMove);
			move = game.FindOne ().latestMove;
		}


		if (game.FindOne () == null) {
			//exit game
		} else {

			if (turnToColor (game.FindOne ().turn).Equals (color)) {
				string move = "";					
				if (GvrViewer.Instance.Triggered) {
					//call get user command, get move in string move
					makeMove (color + "" + newMove);
				}

			}
		}

	}

	public string turnToColor(int turn){
		if (turn % 2 == 0) {
			return "w";
		} else {
			return "b";
		}
	}

	public IEnumerator makeMove(string s){
		move = s;
		var moveMethod = Meteor.Method<string>.Call ("movePiece", s.Substring(1), userId);
		yield return (Coroutine)moveMethod;
		GameObject gm = findObj (s.Substring (1, 3));

		Debug.Log (gm);

		GameObject target = findObj (s.Substring (3));



		if (target != null) {				
			target.transform.Translate (new Vector3 (0, -1, 0));
		}


		Vector3 v = findCoord (s.Substring (3));



		Vector3 travel = new Vector3 (v.x-gm.transform.position.x, (float)0, v.z-gm.transform.position.z);
		gm.transform.Rotate(new Vector3((float)90, (float)0, (float)0));
		gm.transform.Translate (travel);
		gm.transform.Rotate(new Vector3((float)-90, (float)0, (float)0));
	}



	public class DocumentType: Meteor.MongoDocument {
		public object chessObj;
		public string ascii;
		public string latestMove;
		public int player1;
		public int player2;
		public int turn;
		public string[] validMoves;
		public bool gameOver;
	}


	GameObject findObj(string loc){

		Vector3 v = findCoord (loc);

		float xPos = v.x;
		float zPos = v.z;

		foreach (Transform child in transform) {
			if (child.gameObject.name.Equals("Chess_Pawn_B1")){
				Debug.Log(child.position.x + " " + child.position.z);
			}
		}

		foreach (Transform child in transform){
			if (Math.Abs(child.position.x - xPos) <= 0.001 && Math.Abs(child.position.z - zPos) <= 0.001 &&
				!child.gameObject.name.Equals("Chess_Checker") && !child.gameObject.name.Equals("Chess_Frame")){

				return child.gameObject;
			}
		}
		return null;

	}

	Vector3 findCoord(string loc){
		int x = (int)loc.Substring (0, 1).ToCharArray()[0] - 97;
		int z = Int32.Parse(loc.Substring (1)) - 1;
		var xPos = 245 - x * 70;
		var zPos = 245 - z * 70;
		//Debug.Log ("x: " + xPos + " y: " + zPos);
		return new Vector3 ((float)xPos, (float)0, (float)zPos);
	}

	void javaMessage(String x){
		newMove = x;
	}

}
