namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_GameTime_SC")]
    public class MSG_Ret_GameTime_SC : IExtensible
    {
        private ulong _gametime;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="gametime", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong gametime
        {
            get
            {
                return this._gametime;
            }
            set
            {
                this._gametime = value;
            }
        }
    }
}

