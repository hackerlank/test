using UnityEngine;
using System.Collections;

public class ConfigManager : LSingleton<ConfigManager>
{
    public MapConfig mapConfig;
    public GiftBagConfig giftbagConfig;
    public MainUIConfig mainUIConfig;
    private bool init = false;
    public void Initialize()
    {
        if (!init)
        {
            mapConfig = new MapConfig();
            mapConfig.Initialize();

            giftbagConfig = new GiftBagConfig();
            giftbagConfig.Initialize();

            mainUIConfig = new MainUIConfig();
            mainUIConfig.Initialize();

            init = true; 
        }
    }

    public void UnInitialize()
    {
 
    }
}
