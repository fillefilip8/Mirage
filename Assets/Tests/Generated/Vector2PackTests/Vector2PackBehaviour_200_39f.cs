// DO NOT EDIT: GENERATED BY Vector2PackTestGenerator.cs

using System;
using System.Collections;
using Mirage.Serialization;
using Mirage.Tests.Runtime.ClientServer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mirage.Tests.Runtime.Generated.Vector2PackAttributeTests
{
    public class Vector2PackBehaviour_200_39f : NetworkBehaviour
    {
        [Vector2Pack(200f, 200f, 0.05f)]
        [SyncVar] public Vector2 myValue;
    }
    public class Vector2PackTest_200_39f : ClientServerSetup<Vector2PackBehaviour_200_39f>
    {
        static readonly Vector2 value = new Vector2(-10.3f, 0.2f);
        const float within = 0.1f;

        [Test]
        public void SyncVarIsBitPacked()
        {
            serverComponent.myValue = value;

            using (PooledNetworkWriter writer = NetworkWriterPool.GetWriter())
            {
                serverComponent.SerializeSyncVars(writer, true);

                Assert.That(writer.BitPosition, Is.EqualTo(26));

                using (PooledNetworkReader reader = NetworkReaderPool.GetReader(writer.ToArraySegment()))
                {
                    clientComponent.DeserializeSyncVars(reader, true);
                    Assert.That(reader.BitPosition, Is.EqualTo(26));

                    Assert.That(clientComponent.myValue.x, Is.EqualTo(value.x).Within(within));
                    Assert.That(clientComponent.myValue.y, Is.EqualTo(value.y).Within(within));
                }
            }
        }
    }
}
