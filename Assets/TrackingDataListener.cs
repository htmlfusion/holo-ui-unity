using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;

public class TrackingDataListener : MonoBehaviour {

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[8142];

	// Use this for initialization
	void Start () {
		SetupServer ();
	}
	
	// Update is called once per frame
	void Update () {

		string positionString = System.Text.Encoding.Default.GetString(_recieveBuffer);
		string[] position = positionString.Split(null);

		if(position.Length == 7 ){
			transform.position = new Vector3(float.Parse(position[3]), 
			                                 float.Parse(position[4])+449.25f, 
			                                 -float.Parse(position[5]));
		}

		Array.Clear(_recieveBuffer, 0, _recieveBuffer.Length);

	}


	private void SetupServer()
	{
		try
		{
			_clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 5000));
		}
		catch(SocketException ex)
		{
			Debug.Log(ex.Message);
		}
		
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
		
	}
	
	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);
		
		if(recieved <= 0)
			return;
		
		//Copy the recieved data into new buffer , to avoid null bytes
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);
		
		//Process data here the way you want , all your bytes will be stored in recData

		//Start receiving again
		_clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
	}
	
	private void SendData(byte[] data)
	{
		SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
		socketAsyncData.SetBuffer(data,0,data.Length);
		_clientSocket.SendAsync(socketAsyncData);
	}

}
