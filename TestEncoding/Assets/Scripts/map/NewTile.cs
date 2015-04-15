namespace map
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;
    using System.Text;

    [Serializable, ProtoContract(Name="NewTile")]
    public class NewTile : IExtensible
    {
        private uint _flags;
        private uint _type;
        private IExtension extensionObject;

        public string FlagToString()
        {
            string[] names = Enum.GetNames(typeof(TileFlag));
            StringBuilder builder = new StringBuilder();
            int length = names.Length;
            bool flag = true;
            for (int i = 0; i < length; i++)
            {
                TileFlag flag2 = (TileFlag) ((int) Enum.Parse(typeof(TileFlag), names[i], true));
                int num3 = (int) flag2;
                if ((num3 & this.flags) != 0)
                {
                    builder.Append(flag2.ToString()).Append("\n");
                    flag = false;
                }
            }
            if (flag)
            {
                return "Nothing";
            }
            return builder.ToString();
        }

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        public void UpdateTileInfo(object typeVaule)
        {
            this.flags = (uint) ((int) typeVaule);
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="flags", DataFormat=DataFormat.TwosComplement)]
        public uint flags
        {
            get
            {
                return this._flags;
            }
            set
            {
                this._flags = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="type", DataFormat=DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

