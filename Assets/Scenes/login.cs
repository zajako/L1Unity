using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class login : MonoBehaviour {
	// Use this for initialization
	public NetCon con;
	public ChatBox chatInterface;
	private BackgroundWorker _backgroundWorker;
	bool worker_running;
	
	void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		con.read_packet();
		worker_running = false;
	}
	
	void Start () {
		Debug.Log ("Login screen, attempting to login");
		_backgroundWorker = new BackgroundWorker();
		_backgroundWorker.DoWork += backgroundWorker_DoWork;
		worker_running = true;
		_backgroundWorker.RunWorkerAsync();
		con.connect();
		chatInterface = con.getChatInterface();
	}
	
	// Update is called once per frame
	void Update () {
		if (!worker_running && !_backgroundWorker.IsBusy)
		{
			con.process_packet_contents();
			worker_running = true;
			_backgroundWorker.RunWorkerAsync();
		}
	}
	
	void OnApplicationQuit() {
		con.disconnect();
    }
}
