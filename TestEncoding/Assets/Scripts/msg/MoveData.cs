namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MoveData")]
    public class MoveData : IExtensible
    {
        private uint _dir;
        private uint _x;
        private uint _y;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(4, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="x", DataFormat=DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired=false, Name="y", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

