using Mirage.Logging;
using UnityEngine;

namespace Mirage.Examples.Additive
{
    // This script is attached to a scene object called Zone that is on the Player layer and has:
    // - Sphere Collider with isTrigger = true
    // - Network Identity with Server Only checked
    // These OnTrigger events only run on the server and will only send a message to the player
    // that entered the Zone to load the subscene assigned to the subscene property.
    public class ZoneHandler : NetworkBehaviour
    {
        static readonly ILogger logger = LogFactory.GetLogger(typeof(ZoneHandler));

        [Scene]
        [Tooltip("Assign the sub-scene to load for this zone")]
        public string subScene;

        [Server]
        void OnTriggerEnter(Collider other)
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "Loading {0}", subScene);

            NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
            networkIdentity.ConnectionToClient.Send(new SceneMessage { scenePath = subScene, sceneOperation = SceneOperation.LoadAdditive });
        }

        [Server]
        void OnTriggerExit(Collider other)
        {
            if (logger.LogEnabled()) logger.LogFormat(LogType.Log, "Unloading {0}", subScene);

            NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
            networkIdentity.ConnectionToClient.Send(new SceneMessage { scenePath = subScene, sceneOperation = SceneOperation.UnloadAdditive });
        }
    }
}
