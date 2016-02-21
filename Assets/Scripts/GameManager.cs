using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float GameSpeed;
	public float RobotLevel;


	private GameObject ball;
	private GameObject human_player;
	private GameObject robot_paddle_one;
	private GameObject robot_paddle_two;

	//score
	private int human_score;
	private int robot_score;

	private bool scored;

	private Vector3 ball_velocity;
	private float robot_paddle_one_move;
	private float robot_paddle_two_move;
	private Vector3 ball_reset_position;
	private bool human_touched;


	private float last_human_paddle_x, last_robot_paddle_one,last_robot_paddle_two;
	private float human_paddle_velocity;
	private float robot_paddle_velocity;

	private float ball_direction_factor;

	private GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {

		// players parameters
		GameSpeed = 8;
		RobotLevel = 12;
		scored = false;
		human_score = 0;
		robot_score = 0;

		// get objects
		ball = GameObject.FindWithTag("Ball");
		human_player = GameObject.Find ("human_player");
		// two paddles
		robot_paddle_one = GameObject.Find ("robot_paddle_one");
		robot_paddle_two = GameObject.Find ("robot_paddle_two");

		// physical parameters
		human_paddle_velocity = 0;
		robot_paddle_velocity = 0;
		human_touched = false;


		// graphics
		guiStyle.fontSize = 30;
		guiStyle.normal.textColor = Color.white;

		ball_reset_position = new Vector3 (0, 1.5f, 0);

		ResetBall ();

		ball.GetComponent<Rigidbody> ().AddForce(new Vector3(50,0,500));

	}
		
	// Update is called once per frame
	void Update () {

		// check collisions with Raycast System
		RaycastHit hit = new RaycastHit();

		float d_one = ball.transform.position.x - robot_paddle_one.transform.position.x;
		float d_two = ball.transform.position.x - robot_paddle_two.transform.position.x;

		if(d_one > 0){
			robot_paddle_one_move = RobotLevel * Mathf.Min(d_one, 0.5f);    
		}
		if(d_one < 0){
			robot_paddle_one_move = -(RobotLevel * Mathf.Min(-d_one, 0.5f));
		}

		if(d_two > 0){
			robot_paddle_two_move = RobotLevel * Mathf.Min(d_two, 0.5f);    
		}
		if(d_two < 0){
			robot_paddle_two_move = -(RobotLevel * Mathf.Min(-d_two, 0.5f));
		}
			
		Vector3 updated_robot_paddle_one_move = new Vector3(robot_paddle_one.transform.position.x + robot_paddle_one_move * Time.deltaTime,robot_paddle_one.transform.position.y,robot_paddle_one.transform.position.z);
		Vector3 updated_robot_paddle_two_move = new Vector3(robot_paddle_two.transform.position.x + robot_paddle_two_move * Time.deltaTime,robot_paddle_two.transform.position.y,robot_paddle_two.transform.position.z);

//		if (human_touched) {
			if(ball.transform.position.x < 0)
				robot_paddle_one.transform.position = updated_robot_paddle_one_move;
			else
				robot_paddle_two.transform.position = updated_robot_paddle_two_move;
//		}
//
//
		human_paddle_velocity = (human_player.transform.position.x - last_human_paddle_x);
		robot_paddle_velocity = Mathf.Max(robot_paddle_one.transform.position.x - last_robot_paddle_one,robot_paddle_two.transform.position.x - last_robot_paddle_two);
		last_robot_paddle_one = robot_paddle_one.transform.position.x;
		last_robot_paddle_two = robot_paddle_two.transform.position.x;
		last_human_paddle_x = human_player.transform.position.x;
//
//		// Move Human Paddle left/right
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Ray ray = new Ray(human_player.transform.position, -human_player.transform.right);
			if (!Physics.Raycast (ray, out hit, 0.9f)) {
				Vector3 updated_human_position = new Vector3(human_player.transform.position.x - GameSpeed * Time.deltaTime,human_player.transform.position.y,human_player.transform.position.z);
				human_player.transform.position = updated_human_position;
			}	
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Ray ray = new Ray(human_player.transform.position, human_player.transform.right);
			if (!Physics.Raycast (ray, out hit, 0.9f)) {
				Vector3 updated_human_position = new Vector3(human_player.transform.position.x + GameSpeed * Time.deltaTime,human_player.transform.position.y,human_player.transform.position.z);
				human_player.transform.position = updated_human_position;
			}
		}
//		// Move Human Paddle up/down
//		if (Input.GetKey (KeyCode.DownArrow)) {
//			Ray ray = new Ray(human_player.transform.position, -human_player.transform.up);
//			if (!Physics.Raycast (ray, out hit, 0.9f)) {
//				Vector3 updated_human_position = new Vector3(human_player.transform.position.x,human_player.transform.position.y - GameSpeed * Time.deltaTime,human_player.transform.position.z);
//				human_player.transform.position = updated_human_position;
//			}	
//		}
//		if (Input.GetKey (KeyCode.UpArrow)) {
//			Ray ray = new Ray(human_player.transform.position, human_player.transform.up);
//			if (!Physics.Raycast (ray, out hit, 0.9f)) {
//				Vector3 updated_human_position = new Vector3(human_player.transform.position.x,human_player.transform.position.y + GameSpeed * Time.deltaTime,human_player.transform.position.z);
//				human_player.transform.position = updated_human_position;
//			}
//		}
//			
//		// GOAL!!!
//		if (ball.transform.position.z > robot_paddle_one.transform.position.z) {
//			human_score++;
//			ResetBall ();
//			Invoke ("UpdateScore", 4);
//		}
//
//		if (ball.transform.position.z < human_player.transform.position.z) {
//			robot_score++;
//			ResetBall ();
//			Invoke ("UpdateScore", 4);
//		}
			
	}

	void ResetBall()
	{
		ball.transform.position = ball_reset_position;
		scored = true;
	}
		
	void UpdateScore(){

		scored = false;
		ball.GetComponent<Rigidbody> ().AddForce(new Vector3(200,0,400));
	}

	void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.tag == "Ball")
		{
			if(human_paddle_velocity >= 0)
				other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(200,0,400));
			else if (human_paddle_velocity < 0)
				other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-200,0,400));
		}
	}

	void OnGUI()
	{
		string score_text = "The score is " + "HUMAN - ROBOT " + human_score + " - " + robot_score;
		if (scored == true) {
			guiStyle.fontSize = 30;
			GUI.Label (new Rect (Screen.width / 2, 50, 100, 50), score_text, guiStyle);
		} else {
			guiStyle.fontSize = 10;
			GUI.Label (new Rect (5, 5, 100, 50), score_text, guiStyle);
		}
			
	}
}
