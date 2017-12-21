using Realms;
using System;

namespace ElectroneumSpace.Utilities
{
    public static class RealmUtils
    {

        public static Realm LocalRealm
        {
            get
            {
                if (!ThreadUtils.IsMainThreadBound)
                    throw new Exception("Could not get local realm as not on the current thread.");
                return Realm.GetInstance();
            }
        }

    }
}
