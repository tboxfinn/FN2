using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class _ply_NetworkTransformCUSTOM : NetworkBehaviour
{
    public NetworkVariable<Vector3> playerPos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> playerRot = new NetworkVariable<Quaternion>(writePerm: NetworkVariableWritePermission.Owner);

    [SerializeField] NetworkVariable<PlayerData> pD = new(writePerm: NetworkVariableWritePermission.Owner);

    struct PlayerData : INetworkSerializable
    {
        public float x, z;
        public float playerRot;

        internal Vector3 position
        {
            get => new Vector3(x, 0, z);
            set { x = value.x; z = value.z; }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref z);
            serializer.SerializeValue(ref playerRot);
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            var playerDatas = new PlayerData();

            playerDatas.position = transform.position;
            playerDatas.playerRot = transform.eulerAngles.y;
            pD.Value = playerDatas;
        }
        else
        {
            transform.position = pD.Value.position;
            transform.rotation = Quaternion.Euler(0, pD.Value.playerRot, 0);
        }
    }
}
