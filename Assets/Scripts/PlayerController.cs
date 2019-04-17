using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody myrb;
	public AudioSource bg;
	public Camera myCamera;
	public float lerpSpeed;
	public Rigidbody cameraRb;
	public float ballSpeed;
	public float cameraSpeed;
	public float bonusBallSpeed;

	public float minBallDistance;
	public float maxBallDistance;
	public float currentBallDistance;

	/*piece*/
	public float cubeSize = 0.2f;
	public int cubesInRow = 5;

	float cubesPivotDistance;
	Vector3 cubesPivot;

	public float explosionForce = 50f;
	public float explosionRadius = 5f;
	public float explosionUpward = 0.6f;
//piece

	// Use this for initialization
	void Start () {
		myrb = GetComponent<Rigidbody> ();
		bg.Play ();
		//calculate pivot distance
		cubesPivotDistance = cubeSize * cubesInRow / 2;
		//use this value to create pivot vector)
		cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

	}
	
	// Update is called once per frame
	void Update () {
		if (!LevelManager.LM.isStarted)
			return;
		if(Input.GetButton("Fire1")){
			HorizontalMovement ();
		}
		BallCameraDistance ();
		ForwardMovement ();
	}

	private void FixedUpdate(){
		if (!LevelManager.LM.isStarted)
			return;
		CameraMovement ();
	}

	void HorizontalMovement(){
		Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			transform.position = Vector3.Lerp (transform.position, new Vector3(hit.point.x, transform.position.y, transform.position.z), lerpSpeed * Time.deltaTime);
		}

	}

	void ForwardMovement(){
		if (currentBallDistance < maxBallDistance && currentBallDistance > minBallDistance) {
			myrb.velocity = Vector3.forward * ballSpeed;
			if(Input.GetButton("Fire1")){
				float upDownSpeed = Input.GetAxis ("Mouse Y") * bonusBallSpeed;
				myrb.velocity = Vector3.forward * (ballSpeed + upDownSpeed);
			}
		} else {
			myrb.velocity = Vector3.forward * cameraSpeed;
			if(Input.GetButton("Fire1")){
				float upDownSpeed = Input.GetAxis ("Mouse Y") * bonusBallSpeed;
				if (currentBallDistance >= maxBallDistance) {
					if (upDownSpeed < 0) {
						myrb.velocity = Vector3.forward * (ballSpeed + upDownSpeed);
					}
				}
				if (currentBallDistance <= maxBallDistance) {
					if (upDownSpeed > 0) {
						myrb.velocity = Vector3.forward * (ballSpeed + upDownSpeed);
					}
				}
			}
		}

	}

	void CameraMovement(){
		cameraRb.velocity = Vector3.forward * cameraSpeed;
	}

	void BallCameraDistance(){
		currentBallDistance = Vector3.Distance (new Vector3(0,0,myCamera.transform.position.z) , new Vector3(0,0,transform.position.z));
	}

	private void OnCollisionEnter(Collision collision){
		if(collision.rigidbody.tag == "Enemy"){
			Destroy (this.gameObject);
			LevelManager.LM.gameOverPanel.SetActive (true);
			cameraRb.velocity = Vector3.zero;
			LevelManager.LM.isStarted = false;
			explode();
			bg.Play ();
		}
	}

	private void OnTriggerEnter(Collider other){
		if(other.tag == "EnemyLine"){
			LevelManager.LM.winPanel.SetActive (true);
			cameraRb.velocity = Vector3.zero;
			LevelManager.LM.isStarted = false;
		}
	}

	public void explode() {
		//make object disappear
		gameObject.SetActive(false);

		//loop 3 times to create 5x5x5 pieces in x,y,z coordinates
		for (int x = 0; x < cubesInRow; x++) {
			for (int y = 0; y < cubesInRow; y++) {
				for (int z = 0; z < cubesInRow; z++) {
					createPiece(x, y, z);
				}
			}
		}

		//get explosion position
		Vector3 explosionPos = transform.position;
		//get colliders in that position and radius
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		//add explosion force to all colliders in that overlap sphere
		foreach (Collider hit in colliders) {
			//get rigidbody from collider object
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null) {
				//add explosion force to this body with given parameters
				rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
			}
		}

	}

	void createPiece(int x, int y, int z) {

		//create piece
		GameObject piece;
		piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

		//set piece position and scale
		piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
		piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

		//add rigidbody and set mass
		piece.AddComponent<Rigidbody>();
		piece.GetComponent<Rigidbody>().mass = cubeSize;
	}

}
