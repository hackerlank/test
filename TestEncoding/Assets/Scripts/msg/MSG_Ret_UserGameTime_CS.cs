namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_UserGameTime_CS")]
    public class MSG_Ret_UserGameTime_CS : IExtensible
    {
        private ulong _gametime;
        private uint _usertempid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(2, IsRequired=false, Name="gametime", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [ProtoMember(1, IsRequired=false, Name="usertempid", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint usertempid
        {
            get
            {
                return this._usertempid;
            }
            set
            {
                this._usertempid = value;
            }
        }
    }
}

