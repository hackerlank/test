using System;

public class LSingleton<T> where T: class, new()
{
    private static T instance_ = default(T);

    public void DeleteInstance()
    {
        LSingleton<T>.instance_ = default(T);;
    }

    public static T Instance
    {
        get
        {
            if (LSingleton<T>.instance_ == null)
            {
                LSingleton<T>.instance_ = Activator.CreateInstance<T>();
            }
            return LSingleton<T>.instance_;
        }
    }
}

