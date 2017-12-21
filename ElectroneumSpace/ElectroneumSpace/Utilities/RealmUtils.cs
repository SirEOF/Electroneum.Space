using Realms;

namespace ElectroneumSpace.Utilities
{
    public static class RealmUtils
    {

        public static Realm LocalRealm
        {
            get
            {
                if (!ThreadUtils.IsMainThreadBound)
                    throw new System.Exception("Cannot get local realm as caller is not on the main thread.");
                return Realm.GetInstance();
            }
        }

    }
}
