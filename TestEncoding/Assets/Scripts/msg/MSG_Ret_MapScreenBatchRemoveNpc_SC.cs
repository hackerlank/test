namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenBatchRemoveNpc_SC")]
    public class MSG_Ret_MapScreenBatchRemoveNpc_SC : IExtensible
    {
        private readonly List<ulong> _tempids = new List<ulong>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name="tempids", DataFormat=DataFormat.TwosComplement)]
        public List<ulong> tempids
        {
            get
            {
                return this._tempids;
            }
        }
    }
}

