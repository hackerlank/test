using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PackParse_AE
{

    private static AEPackerData aePacker = null;
    private static TexturePackerSprite.Sprite sprite = null;

    public static void PackAE_Atlas(AfterEffectAnimation animation)
    {
        List<TextAsset> atlasFileList = animation.atlasFileList;
        List<Texture> atlasTextureList = animation.atlasTextureList;
        List<AESprite> _sprites = animation.sprites;

        List<AEPackerData> aePackerData = new List<AEPackerData>();
        for (int i = 0; i < atlasFileList.Count; i++)
        {
            TexturePackerSprite sp = PackParse.GetTexturePackerSprite(atlasFileList[i]);
            AEPackerData item = new AEPackerData();
            item.spriteData = sp;
            item.spriteTexture = atlasTextureList[i];
            aePackerData.Add(item);
        }

        for (int i = 0; i < _sprites.Count; i++)
        {
            AESprite sp = _sprites[i];

            if (!IsContain(aePackerData, sp._layer.sourceNoExt))
                continue;
            sp.uvs = GetMeshUV(sprite);
            sp.strAtlasName = sprite.strAltas;
            sp.w = sprite.fSpSourW;
            sp.h = sprite.fSpSourH;

            sp.anchorOffset = new Vector3(sprite.fSourXOffset, -sprite.fSourYOffset, 0);

            if (sp.plane != null)
            {
                Material ma = sp.plane.GetComponent<MeshRenderer>().sharedMaterial;
                ma.mainTexture = aePacker.spriteTexture;

                Mesh mesh = sp.plane.GetComponent<MeshFilter>().sharedMesh;
                mesh.uv = sp.uvs;
            }
        }
    }


    public static bool IsContain(List<AEPackerData> list, string strName)
    {
        for (int i = 0; i < list.Count; i++)
        {
            aePacker = list[i];

            for (int j = 0; j < aePacker.spriteData.Spritelist.Count; j++)
            {
                sprite = aePacker.spriteData.Spritelist[j];
                if (sprite.name != strName)
                    continue;

                return true;
            }
        }

        return false;
    }

    public static Vector2[] GetMeshUV(TexturePackerSprite.Sprite sprite)
    {
        Vector2[] uvs = new Vector2[4];

        Vector2 v0_0, v0_1, v1_1, v1_0;

        //Debug.Log(sprite.outer.xMin + "   " + sprite.outer.xMax + "   " + sprite.outer.yMin + "    " + sprite.outer.yMax);

        v0_0 = new Vector2(sprite.outer.xMin, sprite.outer.yMin);//texturePacker 的坐标系 左上角00 右下角11
        v1_1 = new Vector2(sprite.outer.xMax, sprite.outer.yMax);

        v0_1 = new Vector2(sprite.outer.xMax, sprite.outer.yMin);
        v1_0 = new Vector2(sprite.outer.xMin, sprite.outer.yMax);

        uvs[0] = v1_0; uvs[1] = v0_1;
        uvs[2] = v0_0; uvs[3] = v1_1;

        return uvs;
    }

    public class AEPackerData
    {
        public TexturePackerSprite spriteData;
        public Texture spriteTexture;
    }
}



