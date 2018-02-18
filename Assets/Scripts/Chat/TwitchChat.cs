using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;
using TwitchLib.Models.API.v5.Users;
using UnityEngine;

public enum DebugLogType { None, Short, Full};

/// <summary>
/// System component responsible for twitch chat interaction.
/// </summary>
public class TwitchChat : MonoSingleton<TwitchChat> {
    [SerializeField]
    bool chatEnabled = true;
    [SerializeField]
    DebugLogType logType = DebugLogType.Full;
    [SerializeField]
    char commandIdentifier = '!';

    static TwitchAPI twitchAPI;
    TwitchClient client;
	ConnectionCredentials credentials;
    ConcurrentBag<string> usersJoinedBag;
    ConcurrentBag<string> usersLeftBag;
    ConcurrentQueue<Tuple<string, string>> messageQueue;
    ConcurrentQueue<Tuple<string, string>> commandQueue;

    private void Awake(){
		credentials = new ConnectionCredentials (Credentials.userName, Credentials.accessToken);
    }

    private void Start () {
#if !UNITY_EDITOR
        chatEnabled = true;
        logType = DebugLogType.Full;
#endif
        CheckIsSingleInScene();
        ServicePointManager.ServerCertificateValidationCallback = CertificateValidationMonoFix;
        usersJoinedBag = new ConcurrentBag<string>();
        usersLeftBag = new ConcurrentBag<string>();
        messageQueue = new ConcurrentQueue<Tuple<string, string>>();
        commandQueue = new ConcurrentQueue<Tuple<string, string>>();
        if (chatEnabled) {
            InitTwitchClient();
            client.Connect();
        }
	}

    private void Update() {
        // Handle userJoined and userLeft events in main thread.
        string userName;
        while(usersJoinedBag.IsEmpty == false) {
            if (usersJoinedBag.TryTake(out userName)) {
                if (logType == DebugLogType.Full)
                    Logger.LogMessage("Twitch chat: " + userName + " has joined");
                AvatarListController.Instance.AddViewer(userName);
            }
            else
                break;
        }
        while (usersLeftBag.IsEmpty == false) {
            if (usersLeftBag.TryTake(out userName)) {
                if (logType == DebugLogType.Full)
                    Logger.LogMessage("Twitch chat: " + userName + " has left");
                AvatarListController.Instance.RemoveViewer(userName);
            }
            else
                break;
        }
        // Handle messages and commands in main thread.
        Tuple<string, string> messagePair;
        while(messageQueue.IsEmpty == false) {
            if (messageQueue.TryDequeue(out messagePair)) {
                if (logType == DebugLogType.Full)
                    Logger.LogMessage("Message from " + messagePair.Item1 + ": " + messagePair.Item2);
                AvatarListController.Instance.AddMessage(messagePair);
            }
            else
                break;
        }
        while (commandQueue.IsEmpty == false) {
            if (commandQueue.TryDequeue(out messagePair)) {
                if (logType == DebugLogType.Full)
                    Logger.LogMessage("Command from " + messagePair.Item1 + ": " + messagePair.Item2);
                AvatarListController.Instance.AddCommand(messagePair);
            }
            else
                break;
        }
    }

    private void OnApplicationQuit() {
        if (client != null && client.IsConnected)
            client.Disconnect();
    }

    private void InitTwitchClient() {
        client = new TwitchClient(credentials, Credentials.channelToJoin);
        client.OnConnected += OnConnected;
        client.OnDisconnected += OnDisconnected;
        client.OnUserJoined += OnUserJoined;
        client.OnUserLeft += OnUserLeft;
        client.OnConnectionError += OnConnectionError;
        client.OnWhisperReceived += OnWisperReceived;
        client.OnMessageReceived += OnMessageReceived;
        client.OnChatCommandReceived += OnCommandReceived;
        client.AddChatCommandIdentifier(commandIdentifier);
        client.AddWhisperCommandIdentifier(commandIdentifier);
    }

    private void OnCommandReceived(object sender, OnChatCommandReceivedArgs e) {
        string userName = e.Command.ChatMessage.DisplayName;
        string command = e.Command.CommandText;
        commandQueue.Enqueue(new Tuple<string, string>(userName, command));
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e) {
        string userName = e.ChatMessage.DisplayName;
        string message = e.ChatMessage.Message.Trim();
        if(message[0] != commandIdentifier) 
            messageQueue.Enqueue(new Tuple<string, string>(userName, message));
    }

    private void OnWisperReceived(object sender, OnWhisperReceivedArgs e) {
        // TODO: I think I should handle wisper commands as normal commands and ignore simple messages.
        string userName = e.WhisperMessage.DisplayName;
        string message = e.WhisperMessage.Message;
        if (logType == DebugLogType.Full)
            Logger.LogMessage("Got a wisper from " + userName + ": " + message);
        client.SendWhisper(e.WhisperMessage.Username, "Hey, " + userName + " no reason going stealth-mode," +
            " I can't handle wispers yet.");
    }

    private void OnConnectionError(object sender, OnConnectionErrorArgs e) {
        Logger.LogMessage("TwitchChat connection: " + e.Error.Message, LogType.Error);
    }

    private void OnUserLeft(object sender, OnUserLeftArgs e) {
        string viewerName = GetUserDisplayedName(e.Username);
        usersLeftBag.Add(viewerName);
    }

    private void OnUserJoined(object sender, OnUserJoinedArgs e) {
        string viewerName = GetUserDisplayedName(e.Username);
        usersJoinedBag.Add(viewerName);
    }

    private void OnDisconnected(object sender, OnDisconnectedArgs e) {
        if (logType != DebugLogType.None)
            Logger.LogMessage("Twitch chat disconnected");
    }

    private void OnConnected(object sender, OnConnectedArgs e) {
        if (logType != DebugLogType.None)
            Logger.LogMessage("Twitch chat connected.");
        twitchAPI = new TwitchAPI(Credentials.apiClientId, Credentials.accessToken);
        client.SendMessage("ChatParty successfully connected to the channel.");
    }

    private string GetUserDisplayedName (string userName){
        User[] userList = twitchAPI.Users.v5.GetUserByNameAsync(userName).Result.Matches;
        if (userList == null || userList.Length == 0)
            return null;
        return userList[0].DisplayName;
    }

    /// <summary>
    /// Fix for Mono TLS Exception.
    /// </summary>
    public bool CertificateValidationMonoFix(System.Object sender, X509Certificate certificate, 
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;

        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        foreach (X509ChainStatus status in chain.ChainStatus)
        {
            if (status.Status == X509ChainStatusFlags.RevocationStatusUnknown)
            {
                continue;
            }

            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;

            bool chainIsValid = chain.Build((X509Certificate2)certificate);

            if (!chainIsValid)
            {
                isOk = false;
            }
        }

        return isOk;
    }
}
