namespace TaskingSystem.Global
{
    public class ThemeGlobal : IDisposable
    {
        public static string Name = "bootstrap.min";

        private bool disposed = false;

        public ThemeGlobal(string themeName)
        {
            Name = themeName;
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose of managed resources here (if any)
                }

                // Dispose of unmanaged resources here (if any)

                disposed = true;
            }
        }

        // Destructor (finalizer)
        ~ThemeGlobal()
        {
            Dispose(false);
        }
    }

}









