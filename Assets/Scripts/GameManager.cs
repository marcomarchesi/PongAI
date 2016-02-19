using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float GameSpeed = 0.1f;
	public float RobotLevel = 10.0f;


	private GameObject ball;
	private GameObject human_player;
	private GameObject robot_player;
	private GameObject wall;


	private bool human_touched;

	private Vector3 ball_direction;
	private float robot_position;

	private float last_human_paddle_x, last_robot_paddle_x;
	private float human_paddle_velocity;
	private float robot_paddle_velocity;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find("ball");
		human_player = GameObject.Find ("human_player");
		robot_player = GameObject.Find ("robot_player");
		wall = GameObject.Find("wall");


		ball_direction.x = GameSpeed;
		ball_direction.y = 0;
		ball_direction.z = -0.2f;

		human_touched = false;
		human_paddle_velocity = 0;
		robot_paddle_velocity = 0;

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
			ball_direction.z  = -ball_direction.z;
			human_touched = true;
		}
		if (Physics.Raycast (ball_backward, out hit, 0.6f)) {
			ball_direction.z  = -ball_direction.z;
			human_touched = false;
		}

		// update ball direction
		if (Physics.Raycast (ball_left, out hit, 0.8f))
			ball_direction.x  = -ball_direction.x;
			
		if (Physics.Raycast (ball_right, out hit, 0.8f))
			ball_direction.x  = -ball_direction.x;


		//update ball
		ball.transform.Translate(ball_direction);
			

		Ray robot_left_ray = new Ray(robot_player.transform.position, -robot_player.transform.right);
		Ray robot_right_ray = new Ray(robot_player.transform.position, robot_player.transform.right);

		float d = ball.transform.position.x - robot_player.transform.position.x;

		if(d > 0){
			robot_position = RobotLevel * Mathf.Min(d, 0.5f);    
		}
		if(d < 0){
			robot_position = -(RobotLevel * Mathf.Min(-d, 0.5f));
		}

		Debug.Log ("d is " + RobotLevel);	

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
			if (!Physics.Raycast(ray,out hit,0.8f))
				human_player.transform.Translate (-GameSpeed, 0, 0);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Ray ray = new Ray(human_player.transform.position, human_player.transform.right);
			if (!Physics.Raycast(ray,out hit,0.8f))
				human_player.transform.Translate (GameSpeed, 0, 0);
		}

//		//Change AI difficulty
//		//increase
//		if (Input.GetKeyDown (KeyCode.A)) {
//			ball_direction.x *= 2;
//			ball_direction.z *= 2;
//			RobotLevel *= 2;
//			GameSpeed *= 2;
//		}
//		// decrease
//		if (Input.GetKeyDown (KeyCode.S)) {
//			ball_direction.x /= 2;
//			ball_direction.z /= 2;
//			RobotLevel /= 2;
//			GameSpeed /= 2;
//		}
			
	}
}
