using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHUD : MonoBehaviour {

	private const int startHealth = 100;
	public int currentHealth;
	public Slider healtSlider;
	public Image fillHealthSlider;

	private Color fullHealthColor = Color.green;
	private Color halfHealthColor = Color.yellow;
	private Color noHealthColor = Color.red;

	public Image damageImage;
	public float flashSpeed = 2.5f;
	public Color flashColor = new Color (1.0f, 0.0f, 0.0f, 0.0f);

	private PlayerController moveScript;
	private bool damaged;
	private bool isDead;

	// Use this for initialization
	void Start () {
		moveScript = GetComponent<PlayerController> ();

		currentHealth = startHealth;
		SliderUpdate (startHealth);

		damaged = false;
		if (currentHealth > 0) {
			isDead = false;
		} else {
			isDead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
			TakeDamage (10);
		} 

		if ((currentHealth < 100) && (Input.GetKeyDown(KeyCode.Y))) {
			currentHealth += 10;
			SliderUpdate (currentHealth);
		}


		if (damaged) {
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		damaged = false;
	}

	public void TakeDamage (int amount) {
		damaged = true;

		currentHealth -= amount;

		SliderUpdate (currentHealth);

		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	void SliderUpdate (int health) {
		healtSlider.value = health;

		if (health == 100) {
			fillHealthSlider.color = fullHealthColor;
		} else if ((health < 100) && (health > 50)) {
			fillHealthSlider.color = Color.Lerp (halfHealthColor, fullHealthColor, (float)(health - 50) / 50);
		} else if (health == 50) {
			fillHealthSlider.color = halfHealthColor;
		} else if ((health < 50) && (health > 0)) {
			fillHealthSlider.color = Color.Lerp (noHealthColor, halfHealthColor, (float)health / 50);
		} else if (health <= 0) {
			fillHealthSlider.enabled = false;
		}
	}

	void Death () {
		isDead = true;

		moveScript.enabled = false;
	}
}
