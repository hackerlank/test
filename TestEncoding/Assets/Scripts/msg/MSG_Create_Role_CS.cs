namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Create_Role_CS")]
    public class MSG_Create_Role_CS : IExtensible
    {
        private string _name = string.Empty;
        private uint _occupation;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(""), ProtoMember(1, IsRequired=false, Name="name", DataFormat=DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="occupation", DataFormat=DataFormat.TwosComplement)]
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
    }
}

