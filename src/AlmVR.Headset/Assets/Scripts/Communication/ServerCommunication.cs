using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlmVR.Client;
using AlmVR.Client.Core;
using UnityEngine;
using UnityEngine.UI;

public class ServerCommunication : MonoBehaviour {

    public GameObject StdOut;
    public GameObject PortInputField;

    private Text stdoutText;
    private InputField portInputFieldText;
    private StringBuilder stdoutBuilder = new StringBuilder();
    private SynchronizationContext syncContext;

    // Use this for initialization
    void Start () {
        syncContext = SynchronizationContext.Current;

        stdoutText = StdOut.GetComponent<Text>();
        portInputFieldText = PortInputField.GetComponent<InputField>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public async void Connect()
    {
        AppendLine("Connecting...");
        string ipaddress = "localhost";

        string portString = portInputFieldText.text;
        int port = int.Parse(portString);

        AppendLine($"Connecting to http://{ipaddress}:{port}...");

        var boardClient = ClientFactory.GetInstance<IBoardClient>();
        await boardClient.ConnectAsync(ipaddress, port);
        AppendLine((await boardClient.GetBoardAsync()).ID);

        boardClient.ThingHappenedToMe += BoardClient_ThingHappenedToMe;
    }

    private void AppendLine(string line)
    {
        stdoutBuilder.AppendLine(line);

        stdoutText.text = stdoutBuilder.ToString();
    }

    private void BoardClient_ThingHappenedToMe(object sender, EventArgs e)
    {
        syncContext.Send(x => AppendLine("stop"), null);
    }
}
