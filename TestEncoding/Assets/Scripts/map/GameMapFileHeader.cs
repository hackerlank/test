namespace map
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="GameMapFileHeader")]
    public class GameMapFileHeader : IExtensible
    {
        private uint _height;
        private uint _magic;
        private uint _real_height;
        private uint _real_width;
        private uint _ver;
        private uint _width;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(4, IsRequired=false, Name="height", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="magic", DataFormat=DataFormat.TwosComplement)]
        public uint magic
        {
            get
            {
                return this._magic;
            }
            set
            {
                this._magic = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(6, IsRequired=false, Name="real_height", DataFormat=DataFormat.TwosComplement)]
        public uint real_height
        {
            get
            {
                return this._real_height;
            }
            set
            {
                this._real_height = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="real_width", DataFormat=DataFormat.TwosComplement)]
        public uint real_width
        {
            get
            {
                return this._real_width;
            }
            set
            {
                this._real_width = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="ver", DataFormat=DataFormat.TwosComplement)]
        public uint ver
        {
            get
            {
                return this._ver;
            }
            set
            {
                this._ver = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="width", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }
    }
}

