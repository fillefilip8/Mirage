using Mirage.KCP;
using Mirage.Logging;
using UnityEditor;
using UnityEngine;

namespace Mirage
{

    public static class NetworkMenu
    {
        // Start is called before the first frame update
        [MenuItem("GameObject/Network/NetworkManager", priority = 7)]
        public static GameObject CreateNetworkManager()
        {
            var go = new GameObject("NetworkManager", typeof(NetworkManager), typeof(NetworkServer), typeof(NetworkClient), typeof(NetworkSceneManager), typeof(ServerObjectManager), typeof(ClientObjectManager), typeof(CharacterSpawner), typeof(KcpTransport), typeof(LogSettings));

            KcpTransport transport = go.GetComponent<KcpTransport>();
            NetworkSceneManager nsm = go.GetComponent<NetworkSceneManager>();

            NetworkClient networkClient = go.GetComponent<NetworkClient>();
            networkClient.Transport = transport;

            NetworkServer networkServer = go.GetComponent<NetworkServer>();
            networkServer.Transport = transport;

            ServerObjectManager serverObjectManager = go.GetComponent<ServerObjectManager>();
            serverObjectManager.Server = networkServer;
            serverObjectManager.NetworkSceneManager = nsm;

            ClientObjectManager clientObjectManager = go.GetComponent<ClientObjectManager>();
            clientObjectManager.Client = networkClient;
            clientObjectManager.NetworkSceneManager = nsm;

            NetworkManager networkManager = go.GetComponent<NetworkManager>();
            networkManager.Client = networkClient;
            networkManager.Server = networkServer;
            networkManager.ServerObjectManager = serverObjectManager;
            networkManager.ClientObjectManager = clientObjectManager;
            networkManager.NetworkSceneManager = nsm;

            CharacterSpawner playerSpawner = go.GetComponent<CharacterSpawner>();
            playerSpawner.Client = networkClient;
            playerSpawner.Server = networkServer;
            playerSpawner.SceneManager = nsm;
            playerSpawner.ServerObjectManager = serverObjectManager;
            playerSpawner.ClientObjectManager = clientObjectManager;

            nsm.Client = networkClient;
            nsm.Server = networkServer;
            return go;
        }
    }
}
