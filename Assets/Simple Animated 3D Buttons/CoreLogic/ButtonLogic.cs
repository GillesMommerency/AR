using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Http;
using System.Threading.Tasks;

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
	private float downTravelSpeed = 1f;
	[SerializeField]
	private float upTravelSpeed = 0.01f;
	[SerializeField]
	private float buttonTravelDistance = 50f;
	private float currentDestination = 0;
	private Vector3 startPos;
	static HttpClient client = new HttpClient();
	[SerializeField]
	public static bool aanUit { get; set; }




	// Use this for initialization
	void Start()
	{
		
		if (buttonMeshObject != null)
		{
			startPos = buttonMeshObject.transform.localPosition;
		}

		//gameObject.tag = "button3D"; //This is required if the tag based solution is selected. Can be disabled once it has been set, it's simply just to ensure that the user (you), is aware of this tag's importance.

	}




	public void OnClick()
	{

		MethodsToCallMouseDown.Invoke();
		if (gameObject.name == "Switcher Variant") {
			Debug.Log("Switchieeeeeeeeeeeeeeee");
			if (aanUit == true)
			{
				aanUit = false;
				var newcolor = "000000";
				sendAPICall(newcolor);
			}
			else if (aanUit == false)
			{
				aanUit = true;
				var newcolor = "FFFFFF";
				sendAPICall(newcolor);
			}
		}
		else
		{
			if (aanUit == true) { 
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
				"http://192.168.168.168:5000/rgb/" + mycolor);
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
	
}
