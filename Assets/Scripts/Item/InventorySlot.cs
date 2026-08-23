using System;
using Unity.Netcode;

public struct InventorySlot : INetworkSerializable, IEquatable<InventorySlot>
{
    public int itemId;
    public int count;

    public InventorySlot(int itemId, int count)
    {
        this.itemId = itemId;
        this.count = count;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        serializer.SerializeValue(ref itemId);
        serializer.SerializeValue(ref count);
    }

    public bool Equals(InventorySlot other)
    {
        return itemId == other.itemId &&
               count == other.count;
    }
}