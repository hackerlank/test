using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PackParse
{
    public static TexturePackerSprite GetTexturePackerSprite(TextAsset asset)
    {

        string jsonString = asset.text;
        Hashtable decodedHash = NGUIJson.jsonDecode(jsonString) as Hashtable;
        if (decodedHash == null)
        {
            Debug.LogWarning("Unable to parse Json file: " + asset.name);
            return null;
        }

        TexturePackerSprite packerSprite = new TexturePackerSprite();

        Hashtable meta = (Hashtable)decodedHash["meta"];
        foreach (System.Collections.DictionaryEntry item in meta)
        {
            string strKey = item.Key.ToString();

            if (strKey == "image")//判断是否是pvr
            {
                packerSprite.spriteType = GetImageType(item.Value.ToString());
                packerSprite.strAltas = item.Value.ToString();
                packerSprite.strAltas = packerSprite.strAltas.Replace(".png", "");
                packerSprite.strAltas = packerSprite.strAltas.Replace(".jpg", "");
                packerSprite.strAltas = packerSprite.strAltas.Replace(".tga", "");
            }

            if (strKey != "size")//解析图片大小
                continue;

            Hashtable table = (Hashtable)item.Value;
            if (table != null)
            {
                packerSprite.fSizeW = float.Parse(table["w"].ToString());
                packerSprite.fSizeH = float.Parse(table["h"].ToString());
            }
        }

        Hashtable frames = (Hashtable)decodedHash["frames"];
        List<TexturePackerSprite.Sprite> spriteList = new List<TexturePackerSprite.Sprite>();

        foreach (System.Collections.DictionaryEntry item in frames)
        {
            TexturePackerSprite.Sprite uData = new TexturePackerSprite.Sprite();

            uData.strAltas = packerSprite.strAltas;
            uData.name = item.Key.ToString();

            uData.name = uData.name.Replace(".png", "");
            uData.name = uData.name.Replace(".jpg", "");
            uData.name = uData.name.Replace(".tga", "");

            Hashtable table = (Hashtable)item.Value;
            Hashtable frame = (Hashtable)table["frame"];

            float frameX = float.Parse(frame["x"].ToString());
            float frameY = float.Parse(frame["y"].ToString());
            float frameW = float.Parse(frame["w"].ToString());
            float frameH = float.Parse(frame["h"].ToString());

            uData.rotated = (bool)table["rotated"];
            uData.fSpSourW = frameW;
            uData.fSpSourH = frameH;


            Hashtable sourceSize1 = (Hashtable)table["sourceSize"];

            uData.fSourW = float.Parse(sourceSize1["w"].ToString());
            uData.fSourH = float.Parse(sourceSize1["h"].ToString());


            if (uData.rotated)
            {
                uData.outer = new Rect(frameX, frameY, frameH, frameW);
                uData.inner = new Rect(frameX, frameY, frameH, frameW);
            }
            else
            {
                uData.outer = new Rect(frameX, frameY, frameW, frameH);
                uData.inner = new Rect(frameX, frameY, frameW, frameH);
            }


            Hashtable sourceSize = (Hashtable)table["sourceSize"];
            Hashtable spriteSize = (Hashtable)table["spriteSourceSize"];

            uData.fSourXOffset = float.Parse(spriteSize["x"].ToString());
            uData.fSourYOffset = float.Parse(spriteSize["y"].ToString());

            if (spriteSize != null && sourceSize != null)
            {
                if (frameW > 0)
                {
                    float spriteX = int.Parse(spriteSize["x"].ToString());
                    float spriteW = int.Parse(spriteSize["w"].ToString());
                    float sourceW = int.Parse(sourceSize["w"].ToString());

                    uData.paddingLeft = spriteX / frameW;
                    uData.paddingRight = (sourceW - (spriteX + spriteW)) / frameW;
                }

                if (frameH > 0)
                {
                    float spriteY = int.Parse(spriteSize["y"].ToString());
                    float spriteH = int.Parse(spriteSize["h"].ToString());
                    float sourceH = int.Parse(sourceSize["h"].ToString());

                    uData.paddingTop = spriteY / frameH;
                    uData.paddingBottom = (sourceH - (spriteY + spriteH)) / frameH;
                }
            }


            uData.outer = NGUIMath.ConvertToTexCoords(uData.outer, (int)packerSprite.fSizeW, (int)packerSprite.fSizeH);
            uData.inner = NGUIMath.ConvertToTexCoords(uData.inner, (int)packerSprite.fSizeW, (int)packerSprite.fSizeH);

            spriteList.Add(uData);
        }

        packerSprite.Spritelist = spriteList;

        return packerSprite;
    }

    public static TexturePackerSprite.SpriteType GetImageType(string strValue)
    {
        if (strValue.Contains("pvr"))
            return TexturePackerSprite.SpriteType.PVR;
        else if (strValue.Contains("png"))
            return TexturePackerSprite.SpriteType.PNG;
        else
            return TexturePackerSprite.SpriteType.UNDEFINE;
    }

}


[System.Serializable]
public class TexturePackerSprite
{

    public float fSizeW = 0;
    public float fSizeH = 0;

    public class Sprite
    {
        public string strAltas;
        public string name = "sprite";


        public float fSourW = 0;
        public float fSourH = 0;

        public float fSourXOffset = 0;
        public float fSourYOffset = 0;
        public float fSpSourW = 0;
        public float fSpSourH = 0;

        public Rect outer = new Rect(0f, 0f, 1f, 1f);
        public Rect inner = new Rect(0f, 0f, 1f, 1f);
        public bool rotated = false;


        public float paddingLeft = 0f;
        public float paddingRight = 0f;
        public float paddingTop = 0f;
        public float paddingBottom = 0f;

        public bool hasPadding { get { return paddingLeft != 0f || paddingRight != 0f || paddingTop != 0f || paddingBottom != 0f; } }
    }

    public enum SpriteType
    {
        UNDEFINE,
        PVR,
        PNG,
    }
    public SpriteType spriteType = SpriteType.UNDEFINE;
    public string strAltas;
    private List<Sprite> spriteList = new List<Sprite>();
    public List<Sprite> Spritelist
    {
        get { return spriteList; }
        set { spriteList = value; }
    }

}



