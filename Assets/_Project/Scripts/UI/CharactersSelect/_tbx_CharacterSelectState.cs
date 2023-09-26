using System;
using Unity.Netcode;

public struct _tbx_CharacterSelectState : INetworkSerializable, IEquatable<_tbx_CharacterSelectState>
{
    public ulong ClientId;
    public int CharacterId;

    public _tbx_CharacterSelectState(ulong clientId, int characterId = -1)
    {
        ClientId = clientId;
        CharacterId = characterId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref CharacterId);
    }

    public bool Equals(_tbx_CharacterSelectState other)
    {
        return ClientId == other.ClientId && CharacterId == other.CharacterId;
    }
}
