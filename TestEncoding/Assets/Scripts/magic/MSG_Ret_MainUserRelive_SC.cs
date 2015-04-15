namespace magic
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MainUserRelive_SC")]
    public class MSG_Ret_MainUserRelive_SC : IExtensible
    {
        private ulong _userid;
        private uint _x;
        private uint _y;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="userid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="x", DataFormat=DataFormat.TwosComplement)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="y", DataFormat=DataFormat.TwosComplement)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }
    }
}

