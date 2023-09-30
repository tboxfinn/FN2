using System;
using Unity.Netcode;

public struct _tbx_CharacterSelectState : INetworkSerializable, IEquatable<_tbx_CharacterSelectState>
{
    public ulong ClientId;
    public int CharacterId;
    public bool IsLockedIn;

    public _tbx_CharacterSelectState(ulong clientId, int characterId = -1, bool isLockedIn = false)
    {
        ClientId = clientId;
        CharacterId = characterId;
        IsLockedIn = isLockedIn;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref CharacterId);
        serializer.SerializeValue(ref IsLockedIn);
    }

    public bool Equals(_tbx_CharacterSelectState other)
    {
        return ClientId == other.ClientId && CharacterId == other.CharacterId && IsLockedIn == other.IsLockedIn;
    }
}
