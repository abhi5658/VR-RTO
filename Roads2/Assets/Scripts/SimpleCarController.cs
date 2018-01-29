using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;
using SimpleJSON;

public class SimpleCarController : MonoBehaviour
{
    public Transform t_CenterOfMass;
    public float maxTorque = 350f;
    public float maxSteerAngle = 5f;
    public Transform[] wheelMesh = new Transform[4];
    
    public WheelCollider[] wheelCollider = new WheelCollider[4];
    private Rigidbody r_Ridgedbody;
    private int countWrong;
    public Text countWrongText;
    private int countRight;
    public Text countRightText;
	private float turn = 0.0f, acc = 0.0f;
	string gamedata="";
	bool socketdata = false;

    public void Start()
    {
		
		GameObject go = GameObject.Find("SocketIO");
		SocketIOComponent socket = go.GetComponent<SocketIOComponent> ();
		socket.On("datarec",(SocketIOEvent obj)=> {
			//socketdata = true;
			var N = JSON.Parse(obj.data.ToString());
			//Debug.Log("JSON data : "+obj.data.ToString());
			var Hval = N["H"].AsFloat;
			var Vval = N["V"].AsFloat;
			//Debug.Log("------jsoning---{ H : "+Hval+" V : "+ Vval + " }");
			acc = Vval;
			turn = Hval;
			//Debug.Log(acc);
		});

		Debug.Log ("print");
        r_Ridgedbody = GetComponent<Rigidbody>();
        r_Ridgedbody.centerOfMass = t_CenterOfMass.localPosition;
        countWrong = 0;
        countRight = 0;
        countWrongText.text = "Wrong Count: " + countWrong.ToString();
        countRightText.text = "Right Count: " + countRight.ToString();
    }

    public void Update()
    {
        UpdateMeshPosition();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stop"))
        {
            other.gameObject.SetActive(false);
            countWrong += 1;
            countWrongText.text = "Wrong Count: " + countWrong.ToString();
        }
        else if (other.gameObject.CompareTag("left"))
        {
            other.gameObject.SetActive(false);
            countRight += 1;
            countRightText.text = "Right Count: " + countRight.ToString();
        } 
        else if (other.gameObject.CompareTag("not-ok"))
        {
            other.gameObject.SetActive(false);
            countWrong += 1;
            countWrongText.text = "Wrong Count: " + countWrong.ToString();
        }
    }

    public void FixedUpdate()
    {
		float steer, torque;
		/*
		if (socketdata == true) 
		{*/
			steer = turn * maxSteerAngle;
			torque = acc * maxTorque;
			Debug.Log("Input H : " + turn+ "  Input V : " + acc);
			//socketdata = false;

		/*} 
		else 
		{
			steer = Input.GetAxis("Horizontal") * maxSteerAngle;
			torque = Input.GetAxis("Vertical") * maxTorque;
			Debug.Log("Input H : " + Input.GetAxis ("Horizontal")+ "  Input V : " + Input.GetAxis ("Vertical"));
		/*}
        
		
		/*
		if (steer > 0 || torque > 0) {
			Debug.Log ("Input H : " + Input.GetAxis ("Horizontal"));
			Debug.Log ("Input V : " + Input.GetAxis ("Vertical"));
			Debug.Log ("steer : " + steer);
			Debug.Log ("torque : " + torque+"\n------------------------------------------------");
		}
*/


        wheelCollider[0].steerAngle = steer;
        wheelCollider[1].steerAngle = steer;
        
        for (int i = 2; i < 4; i++) {
            wheelCollider[i].motorTorque = torque;
        }
    }

    public void UpdateMeshPosition()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelCollider[i].GetWorldPose(out pos, out quat);
            wheelMesh[i].position = pos;
            wheelMesh[i].rotation = quat;
        }
    }
}