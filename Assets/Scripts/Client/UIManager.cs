using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using RiptideNetworking.Utils;
using Client.Net;

namespace Client
{
    public class UIManager : MonoBehaviour
    {
        public Camera cam;
        
        private static UIManager _singleton;

        public static UIManager Singleton
        {
            get => _singleton;
            set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(UIManager)} Instance already exists, destroying duplicate!");
                    Destroy(value);
                }

            }
        }

        [Header("Connect")]
        [SerializeField] private GameObject connectUI;
        [SerializeField] private InputField usernameField;

        private void Awake()
        {
            Singleton = this;
        }

        public void ConnectClicked()
        {
            usernameField.interactable = false;
            connectUI.SetActive(false);
            cam.enabled = false;

            NetworkManager.Singleton.Connect();
        }

        public void BackToMain()
        {
            usernameField.interactable = true;
            connectUI.SetActive(true);
        }

        public void SendName()
        {
            Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
            message.AddString(usernameField.text);
            NetworkManager.Singleton.Client.Send(message);
        }

        public void StartServer()
        {
            Server.Net.NetworkManager.Singleton.StartServer();
        }
    }
}


