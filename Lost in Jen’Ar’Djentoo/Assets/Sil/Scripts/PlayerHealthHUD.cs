using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHUD : MonoBehaviour {

	private const int startHealth = 30;
	public int currentHealth;
	public Image healthRadial;
	public Text healthText;

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
		RadialUpdate (startHealth);

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

		if (Input.GetKeyDown(KeyCode.G)) {
			StartCoroutine (DamageOverTime (3, 10));
		} 

		if (Input.GetKeyDown(KeyCode.Y)) {
			HealDamage (10);
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			StartCoroutine (HealOverTime (3, 10));
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

		RadialUpdate (currentHealth);

		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	public IEnumerator DamageOverTime (int amount, int times) {
		for (int i = 0; i < times; i++) {
			TakeDamage (amount);
			yield return new WaitForSeconds (1);
		}
	}

	public void HealDamage (int amount) {
		currentHealth += amount;

		if (currentHealth > startHealth) {
			currentHealth = startHealth;
		}

		RadialUpdate (currentHealth);
	}

	public IEnumerator HealOverTime (int amount, int times) {
		for (int i = 0; i < times; i++) {
			HealDamage (amount);
			yield return new WaitForSeconds (1);
		}
	}

	void RadialUpdate (int health) {
		healthText.text = health + "%";

		healthRadial.fillAmount = (float) health / startHealth;

		if (health == 100) {
			healthRadial.color = fullHealthColor;
		} else if ((health < 100) && (health > 50)) {
			healthRadial.color = Color.Lerp (halfHealthColor, fullHealthColor, (float)(health - 50) / 50);
		} else if (health == 50) {
			healthRadial.color = halfHealthColor;
		} else if ((health < 50) && (health > 0)) {
			healthRadial.color = Color.Lerp (noHealthColor, halfHealthColor, (float)health / 50);
		} else if (health <= 0) {
			healthRadial.enabled = false;
		}
	}

	void Death () {
		isDead = true;
		moveScript.enabled = false;
	}
}
