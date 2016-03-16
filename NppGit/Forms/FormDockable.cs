using System.Drawing;

namespace NppGit.Forms
{
    public interface FormDockable
    {
        void ChangeContext();
        string Title { get; }
        Bitmap TabIcon { get; }
    }
}
