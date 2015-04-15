namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="CharacterMapShow")]
    public class CharacterMapShow : IExtensible
    {
        private uint _coat;
        private uint _face;
        private uint _occupation;
        private uint _weapon;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(3, IsRequired=false, Name="coat", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint coat
        {
            get
            {
                return this._coat;
            }
            set
            {
                this._coat = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="face", DataFormat=DataFormat.TwosComplement)]
        public uint face
        {
            get
            {
                return this._face;
            }
            set
            {
                this._face = value;
            }
        }

        [ProtoMember(4, IsRequired=false, Name="occupation", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint occupation
        {
            get
            {
                return this._occupation;
            }
            set
            {
                this._occupation = value;
            }
        }

        [ProtoMember(2, IsRequired=false, Name="weapon", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint weapon
        {
            get
            {
                return this._weapon;
            }
            set
            {
                this._weapon = value;
            }
        }
    }
}

