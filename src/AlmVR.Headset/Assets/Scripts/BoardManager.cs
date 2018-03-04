using AlmVR.Client;
using AlmVR.Client.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public GameObject CardPrefab;

    public string HostName;
    public int Port;
    
	// Use this for initialization
	async void Start () {
        var boardClient = ClientFactory.GetInstance<IBoardClient>();
        await boardClient.ConnectAsync(HostName, Port);
        var board = await boardClient.GetBoardAsync();

        var cards = (from c in board.SwimLanes.SelectMany(s => s.Cards)
                     select GameObject.Instantiate(CardPrefab)).ToArray();

        foreach (var card in cards)
        {
            card.transform.position = new Vector3(Random.value * 10.0f - 5, Random.value * 500.0f, Random.value * 10.0f - 5);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
