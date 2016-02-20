using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float GameSpeed;
	public float RobotLevel;


	private GameObject ball;
	private GameObject human_player;
	private GameObject robot_player;
	private GameObject wall;

	//score
	private int human_score;
	private int robot_score;

	private bool human_touched;
	private bool scored;

	private Vector3 ball_velocity;
	private float robot_position;
	private Vector3 ball_reset_position;

	private float last_human_paddle_x, last_robot_paddle_x;
	private float human_paddle_velocity;
	private float robot_paddle_velocity;

	private float ball_direction_factor;

	private GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {

		GameSpeed = 8;
		RobotLevel = 8;
		
		ball = GameObject.Find("ball");
		human_player = GameObject.Find ("human_player");
		robot_player = GameObject.Find ("robot_player");
		wall = GameObject.Find("wall");


		ball_reset_position = new Vector3 (0, 1, 0);

		ball_velocity.x = 0.1f;
		ball_velocity.y = 0;
		ball_velocity.z = -0.2f;

		human_touched = false;
		scored = false;


		human_paddle_velocity = 0;
		robot_paddle_velocity = 0;
		ball_direction_factor = 1.0f;

		human_score = 0;
		robot_score = 0;

		guiStyle.fontSize = 30;
		guiStyle.normal.textColor = Color.white;

	}
		
	// Update is called once per frame
	void Update () {

		// check collisions with Raycast System
		RaycastHit hit = new RaycastHit();
		Ray ball_forward = new Ray (ball.transform.position, -ball.transform.forward);
		Ray ball_backward = new Ray (ball.transform.position, ball.transform.forward);
		Ray ball_left = new Ray (ball.transform.position, -ball.transform.right);
		Ray ball_right = new Ray (ball.transform.position, ball.transform.right);
		if (Physics.Raycast (ball_forward, out hit, 0.6f)) {
			ball_velocity.z  = -ball_velocity.z;
			human_touched = true;
			ball_direction_factor = (Mathf.Sign (human_paddle_velocity) == 1)? Random.Range (0.8f, 1.0f):Random.Range (1.0f, 1.2f);

		}
		if (Physics.Raycast (ball_backward, out hit, 0.6f)) {
			ball_velocity.z  = -ball_velocity.z;
			human_touched = false;
			ball_direction_factor = (Mathf.Sign (robot_paddle_velocity) == -1)? Random.Range (0.8f, 1.0f):Random.Range (1.0f, 1.2f);
		}


		// update ball direction
		if (Physics.Raycast (ball_left, out hit, 0.8f))
			ball_velocity.x = -ball_velocity.x * ball_direction_factor;
			
		if (Physics.Raycast (ball_right, out hit, 0.8f))
			ball_velocity.x  = -ball_velocity.x * ball_direction_factor;




		//update ball
		if(scored == false)
			ball.transform.Translate(ball_velocity);
			

		Ray robot_left_ray = new Ray(robot_player.transform.position, -robot_player.transform.right);
		Ray robot_right_ray = new Ray(robot_player.transform.position, robot_player.transform.right);

		float d = ball.transform.position.x - robot_player.transform.position.x;

		if(d > 0){
			robot_position = RobotLevel * Mathf.Min(d, 0.5f);    
		}
		if(d < 0){
			robot_position = -(RobotLevel * Mathf.Min(-d, 0.5f));
		}

		Debug.Log ("robot position is " + robot_position);
				

		Vector3 updated_robot_position = new Vector3(robot_player.transform.position.x + robot_position * Time.deltaTime,robot_player.transform.position.y,robot_player.transform.position.z);

//		robot_player.transform.Translate (robot_position, 0, 0);
		robot_player.transform.position = updated_robot_position;


		human_paddle_velocity = (human_player.transform.position.x - last_human_paddle_x);
		robot_paddle_velocity = (robot_player.transform.position.x - last_robot_paddle_x);
		last_robot_paddle_x = robot_player.transform.position.x;
		last_human_paddle_x = human_player.transform.position.x;


			

		// Move Human Paddle left/right
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



		// GOAL!!!
		if (ball.transform.position.z > robot_player.transform.position.z) {
			human_score++;
			ResetBall ();
			Invoke ("UpdateScore", 2);
		}

		if (ball.transform.position.z < human_player.transform.position.z) {
			robot_score++;
			ResetBall ();
			Invoke ("UpdateScore", 2);
		}
			
			
	}

	void ResetBall()
	{
		scored = true;
		ball.transform.position = ball_reset_position;
	}
		
	void UpdateScore(){
		scored = false;
	}

	void OnGUI()
	{
		string score_text = "The score is " + "HUMAN - ROBOT " + human_score + " - " + robot_score;
		if (scored == true) {
			guiStyle.fontSize = 30;
			GUI.Label (new Rect (Screen.width/2, 50, 100, 50), score_text, guiStyle);
		}
		else {
			guiStyle.fontSize = 10;
			GUI.Label (new Rect (5, 5, 100, 50), score_text, guiStyle);
		}
			
			
	}
}
