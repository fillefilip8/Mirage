// DO NOT EDIT: GENERATED BY ZigZagTestGenerator.cs

using System;
using System.Collections;
using Mirage.Serialization;
using Mirage.Tests.Runtime.ClientServer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mirage.Tests.Runtime.Generated.ZigZagAttributeTests.MyEnum2_4_negative
{
    [System.Serializable]
    public enum MyEnum2
    {
        Negative = -1,
        Zero = 0,
        Positive = 1,
    }
    public class BitPackBehaviour : NetworkBehaviour
    {
        [BitCount(4), ZigZagEncode]
        [SyncVar] public MyEnum2 myValue;

        public event Action<MyEnum2> onRpc;

        [ClientRpc]
        public void RpcSomeFunction([BitCount(4), ZigZagEncode] MyEnum2 myParam)
        {
            onRpc?.Invoke(myParam);
        }
        
        // Use BitPackStruct in rpc so it has writer generated
        [ClientRpc]
        public void RpcOtherFunction(BitPackStruct myParam)
        {
            // nothing
        }
    }
    
    [NetworkMessage]
    public struct BitPackMessage 
    {
        [BitCount(4), ZigZagEncode] 
        public MyEnum2 myValue;
    }

    [Serializable]
    public struct BitPackStruct
    {
        [BitCount(4), ZigZagEncode] 
        public MyEnum2 myValue;
    }

    public class BitPackTest : ClientServerSetup<BitPackBehaviour>
    {
        private const MyEnum2 value = (MyEnum2)(-1);

        [Test]
        public void SyncVarIsBitPacked()
        {
            serverComponent.myValue = value;

            using (PooledNetworkWriter writer = NetworkWriterPool.GetWriter())
            {
                serverComponent.SerializeSyncVars(writer, true);

                Assert.That(writer.BitPosition, Is.EqualTo(4));

                using (PooledNetworkReader reader = NetworkReaderPool.GetReader(writer.ToArraySegment(), null))
                {
                    clientComponent.DeserializeSyncVars(reader, true);
                    Assert.That(reader.BitPosition, Is.EqualTo(4));

                    Assert.That(clientComponent.myValue, Is.EqualTo(value));
                }
            }
        }

        [UnityTest]
        public IEnumerator RpcIsBitPacked()
        {
            int called = 0;
            clientComponent.onRpc += (v) => 
            { 
                called++;
                Assert.That(v, Is.EqualTo(value)); 
            };

            client.MessageHandler.UnregisterHandler<RpcMessage>();
            int payloadSize = 0;
            client.MessageHandler.RegisterHandler<RpcMessage>((player, msg) =>
            {
                // store value in variable because assert will throw and be catch by message wrapper
                payloadSize = msg.payload.Count;
                clientObjectManager.OnRpcMessage(msg);
            });

            serverComponent.RpcSomeFunction(value);
            yield return null;
            yield return null;
            Assert.That(called, Is.EqualTo(1));
            
            // this will round up to nearest 8
            int expectedPayLoadSize = (4 + 7) / 8;
            Assert.That(payloadSize, Is.EqualTo(expectedPayLoadSize), $"4 bits is 1 bytes in payload");
        }

        [UnityTest]
        public IEnumerator StructIsBitPacked() 
        {
            var inMessage = new BitPackMessage 
            {
                myValue = value,
            };

            int payloadSize = 0;
            int called = 0;
            BitPackMessage outMessage = default;
            server.MessageHandler.RegisterHandler<BitPackMessage>((player, msg) =>
            {
                // store value in variable because assert will throw and be catch by message wrapper
                called++;
                outMessage = msg;
            });
            Action<NetworkDiagnostics.MessageInfo> diagAction = (info) =>
            {
                if (info.message is BitPackMessage)
                {
                    payloadSize = info.bytes;
                }
            };

            NetworkDiagnostics.OutMessageEvent += diagAction;
            client.Player.Send(inMessage);
            NetworkDiagnostics.OutMessageEvent -= diagAction;
            yield return null;
            yield return null;
            Assert.That(called, Is.EqualTo(1));
            // this will round up to nearest 8
            // +2 for message header
            int expectedPayLoadSize = ((4 + 7) / 8) + 2;
            Assert.That(payloadSize, Is.EqualTo(expectedPayLoadSize), $"4 bits is {expectedPayLoadSize - 2} bytes in payload");
            Assert.That(outMessage, Is.EqualTo(inMessage));
        }

        [Test]
        public void MessageIsBitPacked() 
        {
            var inStruct = new BitPackStruct 
            {
                myValue = value,
            };

            using (PooledNetworkWriter writer = NetworkWriterPool.GetWriter())
            {
                // generic write, uses generated function that should include bitPacking
                writer.Write(inStruct);

                Assert.That(writer.BitPosition, Is.EqualTo(4));

                using (PooledNetworkReader reader = NetworkReaderPool.GetReader(writer.ToArraySegment(), null))
                {
                    var outStruct = reader.Read<BitPackStruct>();
                    Assert.That(reader.BitPosition, Is.EqualTo(4));

                    Assert.That(outStruct, Is.EqualTo(inStruct));
                }
            }
        }
    }
}
