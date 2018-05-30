using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {
	/// <summary>
	/// author: silvio
	/// </summary>

	public Transform player;

	void LateUpdate () {
		//Nuova posizione del player
		Vector3 newPosition = player.position;

		//Mantenendo la stessa altezza di camera
		newPosition.y = transform.position.y;

		//Set della posizione
		transform.position = newPosition;

		//La minimappa ruota col player
		transform.rotation = Quaternion.Euler(90.0f, player.eulerAngles.y, 0.0f);
	}
}
