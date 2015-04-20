using System;
using System.Collections.Generic;
using System.Text;

namespace map
{
    public partial class NewTile
    {
        public void UpdateTileInfo(System.Object typeVaule)
        {
            this.flags = (UInt32)(Int32)typeVaule;
        }

        public string FlagToString() 
        {
            string result = string.Empty;

            string[] names = System.Enum.GetNames(typeof(map.TileFlag));

            StringBuilder sb = new StringBuilder();

            int length = names.Length;

            bool nothing = true;

            for (int i = 0; i < length; i++)
            {
                map.TileFlag tempFlag = (map.TileFlag)System.Enum.Parse(typeof(map.TileFlag), names[i], true);
                int v = (int)tempFlag;

                if ((v & this.flags) != 0)
                {
                    sb.Append(tempFlag.ToString()).Append("\n");
                    nothing = false;
                }
            }

            if (nothing)
            {
                result = "Nothing";
            }
            else 
            {
                result = sb.ToString();
            }

            return result;
        }

    }
}
