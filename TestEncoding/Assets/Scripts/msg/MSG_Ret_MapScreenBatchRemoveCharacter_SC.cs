namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenBatchRemoveCharacter_SC")]
    public class MSG_Ret_MapScreenBatchRemoveCharacter_SC : IExtensible
    {
        private readonly List<ulong> _charids = new List<ulong>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name="charids", DataFormat=DataFormat.TwosComplement)]
        public List<ulong> charids
        {
            get
            {
                return this._charids;
            }
        }
    }
}

