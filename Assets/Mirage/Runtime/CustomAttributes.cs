using System;
using UnityEngine;

namespace Mirage
{
    /// <summary>
    /// SyncVars are used to synchronize a variable from the server to all clients automatically.
    /// <para>Value must be changed on server, not directly by clients.  Hook parameter allows you to define a client-side method to be invoked when the client gets an update from the server.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SyncVarAttribute : PropertyAttribute
    {
        ///<summary>A function that should be called on the client when the value changes.</summary>
        public string hook;
    }

    /// <summary>
    /// Call this from a client to run this function on the server.
    /// <para>Make sure to validate input etc. It's not possible to call this from a server.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerRpcAttribute : Attribute
    {
        public int channel = Channel.Reliable;
        public bool requireAuthority = true;
    }

    public enum Client { Owner, Observers, Player }

    /// <summary>
    /// The server uses a Remote Procedure Call (RPC) to run this function on specific clients.
    /// <para>Note that if you set the target as Connection, you need to pass a specific connection as a parameter of your method</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ClientRpcAttribute : Attribute
    {
        public int channel = Channel.Reliable;
        public Client target = Client.Observers;
        public bool excludeOwner;
    }

    /// <summary>
    /// Prevents a method from running if server is not active.
    /// <para>Can only be used inside a NetworkBehaviour</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerAttribute : Attribute
    {
        /// <summary>
        /// If true,  when the method is called from a client, it throws an error
        /// If false, no error is thrown, but the method won't execute
        /// useful for unity built in methods such as Await, Update, Start, etc.
        /// </summary>
        public bool error = true;
    }

    /// <summary>
    /// Tell the weaver to generate  reader and writer for a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class NetworkMessageAttribute : Attribute
    {
    }

    /// <summary>
    /// Prevents this method from running if client is not active.
    /// <para>Can only be used inside a NetworkBehaviour</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ClientAttribute : Attribute
    {
        /// <summary>
        /// If true,  when the method is called from a client, it throws an error
        /// If false, no error is thrown, but the method won't execute
        /// useful for unity built in methods such as Await, Update, Start, etc.
        /// </summary>
        public bool error = true;
    }

    /// <summary>
    /// Prevents players without authority from running this method.
    /// <para>Can only be used inside a NetworkBehaviour</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HasAuthorityAttribute : Attribute
    {
        /// <summary>
        /// If true,  when the method is called from a client, it throws an error
        /// If false, no error is thrown, but the method won't execute
        /// useful for unity built in methods such as Await, Update, Start, etc.
        /// </summary>
        public bool error = true;
    }

    /// <summary>
    /// Prevents nonlocal players from running this method.
    /// <para>Can only be used inside a NetworkBehaviour</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LocalPlayerAttribute : Attribute
    {
        /// <summary>
        /// If true,  when the method is called from a client, it throws an error
        /// If false, no error is thrown, but the method won't execute
        /// useful for unity built in methods such as Await, Update, Start, etc.
        /// </summary>
        public bool error = true;
    }

    /// <summary>
    /// Converts a string property into a Scene property in the inspector
    /// </summary>
    public class SceneAttribute : PropertyAttribute { }

    /// <summary>
    /// Used to show private SyncList in the inspector,
    /// <para> Use instead of SerializeField for non Serializable types </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowInInspectorAttribute : Attribute { }
}
