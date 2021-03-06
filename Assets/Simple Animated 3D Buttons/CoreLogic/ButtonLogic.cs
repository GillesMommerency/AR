﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;
using UnityEngine.XR.ARFoundation;

public class ButtonLogic : MonoBehaviour
{

	[Header("Target Method")]
	[SerializeField]
	private UnityEvent MethodsToCallMouseDown;
	[SerializeField]
	private UnityEvent MethodsToCallMouseStay;
	[SerializeField]
	private UnityEvent MethodsToCallMouseUp;



	[Header("Button Animation Properties")]
	[SerializeField]
	private GameObject buttonMeshObject;
	[SerializeField]
	private GameObject buttonSwitchObject;
	[SerializeField]
	private float downTravelSpeed = 1f;
	[SerializeField]
	private float downTravelSpeedSwitch = 0.01f;
	[SerializeField]
	private float upTravelSpeed = 0.01f;
	[SerializeField]
	private float buttonTravelDistance = 50f;
	private float switchTravelDistance = 0.045f;
	private Vector3 startPos;
	private Vector3 startPosSwitch;
	static HttpClient client = new HttpClient();
	[SerializeField]
	public static bool aanUit { get; set; }
	public AudioSource clickButton;
	

    // Use this for initialization
    void Start()
	{
	
		if (buttonMeshObject != null)
		{
			startPos = buttonMeshObject.transform.localPosition;
		}
		if (buttonSwitchObject != null)
		{
			startPosSwitch = buttonSwitchObject.transform.localPosition;
		}


	}




	public void OnClick()
	{

		MethodsToCallMouseDown.Invoke();
		clickButton.Play();
		if (gameObject.name == "Switcher Variant")
		{
			
			Debug.Log("Switchieeeeeeeeeeeeeeee");
			if (aanUit == true)
			{
				StartCoroutine("AnimateSwitchDown");
				aanUit = false;
				var newcolor = "000000";
				sendAPICall(newcolor);
			}
			else if (aanUit == false)
			{
				StartCoroutine("AnimateSwitchUp");
				aanUit = true;
				var newcolor = "FFFFFF";
				sendAPICall(newcolor);
			}
		}
		else
		{
			if (aanUit == true)
			{
				var mycolor = gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color;
				Debug.Log("Hello:" + ColorUtility.ToHtmlStringRGB(mycolor));
				var newcolor = ColorUtility.ToHtmlStringRGB(mycolor);
				//var stringContent = JsonSerializer.Serialize(newcolor);
				sendAPICall(newcolor);
			}
		}

		if (buttonMeshObject != null)
		{
			StartCoroutine("AnimateButtonDown");
		}
	}
	public void OnClickStay()
	{
		MethodsToCallMouseStay.Invoke();
	}
	public void OnClickLeave()
	{
		MethodsToCallMouseUp.Invoke();

	}
	public async void sendAPICall(string mycolor)
	{
		HttpResponseMessage response = await client.GetAsync(
				"http://192.168.0.135:5000/rgb/" + mycolor);
		response.EnsureSuccessStatusCode();

		// return URI of the created resource.

		Debug.Log("Response:" + response.Headers.Location);
	}

	public async void sendBrightnessCall(int brightness)
	{
		HttpResponseMessage response = await client.GetAsync(
				"http://192.168.0.135:5000/brightness/" + brightness);
		response.EnsureSuccessStatusCode();

		// return URI of the created resource.

		Debug.Log("Response:" + response.Headers.Location);
	}
	public void ButtonNoLongerPressed()
	{
		Debug.Log("Check");
		if (buttonMeshObject != null)
		{
			StartCoroutine("AnimateButtonUp");
		}
	}
	public void onValueChange(Slider slider)
	{
		if (aanUit == true)
        {
			//Debug.Log("Jis aan:" + slider.value);
			var brightness = Convert.ToInt32(slider.value);
			sendBrightnessCall(brightness);

		}
	}

	IEnumerator AnimateButtonDown()
	{
		StopCoroutine("AnimateButtonUp");
		float travelled = 0;
		buttonMeshObject.transform.localPosition = startPos;
		while (travelled < buttonTravelDistance)
		{
			travelled += downTravelSpeed;
			buttonMeshObject.transform.localPosition -= new Vector3(0, 1, 0) * downTravelSpeed;
			yield return null;
		}
	}
	IEnumerator AnimateButtonUp()
	{
		float travelled = 0;
		while (travelled < buttonTravelDistance)
		{
			travelled += upTravelSpeed;
			buttonMeshObject.transform.localPosition += new Vector3(0, 1, 0) * upTravelSpeed;
			yield return null;
		}
		buttonMeshObject.transform.localPosition = startPos;
	}

	IEnumerator AnimateSwitchDown()
	{
		float travelled = 0;
		while (travelled < switchTravelDistance)
		{
			travelled += downTravelSpeedSwitch;
			buttonSwitchObject.transform.localPosition -= new Vector3(0, 0, 1) * downTravelSpeedSwitch;
			yield return null;
		}
	}
	IEnumerator AnimateSwitchUp()
	{
		float travelled = 0;
		while (travelled < switchTravelDistance)
		{
			travelled += upTravelSpeed;
			buttonSwitchObject.transform.localPosition += new Vector3(0, 0, 1) * upTravelSpeed;
			yield return null;
		}
	}


}
