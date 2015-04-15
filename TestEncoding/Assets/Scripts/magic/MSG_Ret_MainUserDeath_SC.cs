namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MainUserDeath_SC")]
    public class MSG_Ret_MainUserDeath_SC : IExtensible
    {
        private ulong _tempid;
        private uint _waittime;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="tempid", DataFormat=DataFormat.TwosComplement)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="waittime", DataFormat=DataFormat.TwosComplement)]
        public uint waittime
        {
            get
            {
                return this._waittime;
            }
            set
            {
                this._waittime = value;
            }
        }
    }
}

