using System.Drawing;

namespace NppGit
{
    public interface FormDockable
    {
        void ChangeContext();
        string Title { get; }
        Bitmap TabIcon { get; }
    }
}
