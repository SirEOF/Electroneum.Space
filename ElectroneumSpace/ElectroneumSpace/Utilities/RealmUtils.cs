using Realms;
namespace ElectroneumSpace.Utilities
{
    public class RealmUtils
    {

        public Realm GetLocalRealm()
        {
            if (!ThreadUtils.IsMainThreadBound)
                throw new System.Exception("Cannot get local realm as caller is not on the main thread.");
            return Realm.GetInstance();
        }

    }
}
