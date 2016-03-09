namespace NppGit.Modules.PSSEFeatures
{
    public sealed class POSHObject
    {
        private static object syncRoot = new object();
        private static volatile POSHObject _instance;
        private POSHObject() { }

        public static POSHObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(syncRoot)
                    {
                        if (_instance == null)
                            _instance = new POSHObject();
                    }
                }
                return _instance;
            }
        }
    }
}
