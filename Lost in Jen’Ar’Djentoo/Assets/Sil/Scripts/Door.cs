using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
	private HUDSystem hud;
	private bool inRange;
	private bool isOpened;
	private bool moveDoor;

	private float currentY;
	private float absoluteY;
	public float closeDoorY = 0.0f;
	public float openDoorY = 45.0f;

	//private Vector3 portaAperta = new Vector3 (0.0f, 90.0f, 0.0f);
	//private Vector3 portaChiusa = new Vector3 (0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem> ();
		isOpened = false;
		hud.centralBoxEnabler (false);
		inRange = false;
	}

	// Update is called once per frame
	void Update () {
		//Premi E, sei nel raggio della porta ed è ferma
		if ((Input.GetKeyDown(KeyCode.E)) && (inRange) && !(moveDoor)) {
			moveDoor = true;
		}

		//La porta è chiusa e richiede di aprirsi
		if (!(isOpened) && (moveDoor)) {
			hud.centralBoxEnabler (false);
			currentY = transform.eulerAngles.y;

			currentY = Mathf.LerpAngle(currentY, openDoorY, Time.deltaTime);

			if (Mathf.Abs (openDoorY - currentY) > 360.0f) {
				absoluteY = Mathf.Abs (openDoorY - currentY) - 360.0f;
			} else {
				absoluteY = Mathf.Abs (openDoorY - currentY);
			}

			if (absoluteY < 1.0f) {
				currentY = openDoorY;
				moveDoor = false;
				isOpened = true;
			}

			transform.eulerAngles = new Vector3 (0.0f, currentY, 0.0f);
		}

		//La porta è aperta e richiede di chiudersi
		if ((isOpened) && (moveDoor)) {
			hud.centralBoxEnabler (false);
			currentY = transform.eulerAngles.y;

			currentY = Mathf.LerpAngle(currentY, closeDoorY, Time.deltaTime);

			if (Mathf.Abs (closeDoorY - currentY) > 360.0f) {
				absoluteY = Mathf.Abs (closeDoorY - currentY) - 360.0f;
			} else {
				absoluteY = Mathf.Abs (closeDoorY - currentY);
			}

			if (absoluteY < 1.0f) {
				currentY = closeDoorY;
				moveDoor = false;
				isOpened = false;
			}

			transform.eulerAngles = new Vector3 (0.0f, currentY, 0.0f);
		}
	}

	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			inRange = true;
		}
	}

	public void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			
			//isOpened rappresenta la porta aperta
			//True se è aperta, false se è chiusa
			if (!(isOpened)) {
				//Se la porta non è in movimento
				if (!(moveDoor)) {
					hud.centralBoxText ("Premi E per aprire la porta");
					hud.centralBoxEnabler (true);
				}
			} else {
				//Se la porta non è in movimento
				if (!(moveDoor)) {
					hud.centralBoxText ("Premi E per chiudere la porta");
					hud.centralBoxEnabler (true);
				}
			}
		}
	}

	public void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			inRange = false;
			hud.centralBoxEnabler (false);
		}
	}
}
