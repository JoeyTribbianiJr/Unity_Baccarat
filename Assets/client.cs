using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class client : MonoBehaviour {

	public NetworkClient svr_client;
	public GameObject playerPre;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnGUI()
	{
		
		//if (GUI.Button(new Rect(10, 10, 100, 30), "client"))
		//{
		//	ClientConnectServerAction();
		//}
		//if (GUI.Button(new Rect(10, 50, 100, 30), "add player"))
		//{
		//	ClientScene.AddPlayer(31);
		//}

	}
	public void ClientConnectServerAction()
	{
		svr_client = new NetworkClient();
		//连接服务器
		//注册客户端连接之后的回调方法
		svr_client.RegisterHandler(MsgType.Connect, ClientConnectServerCallback);
		print("已注册回调");
		svr_client.Connect("127.0.0.1", 54322);
	}
	void ClientConnectServerCallback(NetworkMessage msg)
	{
		print("已连接");
		//告诉服务器准备完毕
		ClientScene.Ready(msg.conn);
		//向服务器注册预设体(预设体必须是网络对象)
		ClientScene.RegisterPrefab(playerPre);
		//向服务器发送添加游戏对象的请求
		//服务器在接收这个请求之后会自动调用 添加游戏玩家的回调方法
		//ClientScene.AddPlayer(31);
	}
}
