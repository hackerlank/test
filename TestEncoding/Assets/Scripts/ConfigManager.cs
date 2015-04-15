using System;

public class ConfigManager : LSingleton<ConfigManager>
{
    public MapConfig mapConfig;

    public void Initialize()
    {
        this.mapConfig = new MapConfig();
        this.mapConfig.Initialize();
    }

    public void UnInitialize()
    {
    }
}

