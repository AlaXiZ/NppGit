using System.Runtime.InteropServices;
using System.Windows.Forms;
using NppKate.Common;
using NppKate.Interop;
using NppKate.Modules.TortoiseGitFeatures;

namespace NppKate.Modules.GitCore
{
    public partial class TortoiseLogSearch : FormDialog
    {
        private readonly int[] filters = new int[] { 0x001, 0x002, 0x004, 0x008, 0x020, 0x040, 0x080, 0x100, 0x200 };
        private LocalWindowsHook hook;

        public string RepositoryPath { get; set; }

        public string SearchText
        {
            get { return tbFindString.Text; }
            set { tbFindString.Text = value; }
        }

        public TortoiseLogSearch()
        {
            InitializeComponent();
            hook = new LocalWindowsHook(HookType.WH_GETMESSAGE);
            hook.HookInvoked += Hook_HookInvoked;
            hook.Install();
        }

        private void Hook_HookInvoked(object sender, HookEventArgs e)
        {
            MSG msg = (Interop.MSG)Marshal.PtrToStructure(e.lParam, typeof(Interop.MSG));
            if (msg.message == (uint)WinMsg.WM_KEYDOWN)
            {
                if ((Keys)msg.wParam.ToInt32() == Keys.Escape)
                {
                    Close();
                }
                else if ((Keys)msg.wParam.ToInt32() == Keys.Enter && tbFindString.Text != "")
                {
                    Search();
                    Close();
                }
            }
        }

        private void SaveFindType()
        {
            int flags = 0;
            for (int i = 0; i < chklstFindType.Items.Count; i++)
            {
                flags |= (chklstFindType.GetItemChecked(i) ? 1 : 0) << i;
            }
            Settings.GitCore.SearchFlags = flags;
        }

        private void LoadFindType()
        {
            int flags = Settings.GitCore.SearchFlags;
            for (int i = 0; i < chklstFindType.Items.Count; i++)
            {
                chklstFindType.SetItemChecked(i, (flags & (1 << i)) > 0);
            }
        }

        private void bSearch_Click(object sender, System.EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var tgSearch = _manager.ModuleManager.GetService(typeof(ITortoiseGitSearch)) as ITortoiseGitSearch;
            var findType = GetFindTypes();
            var findString = tbFindString.Text;

            tgSearch.RunSearch(findType, RepositoryPath, findString);
        }

        private TortoiseGitFindType GetFindTypes()
        {
            int result = 0;
            for (int i = 0; i < filters.Length; i++)
            {
                result |= chklstFindType.GetItemChecked(i) ? filters[i] : 0;
            }

            return result == 0 ? TortoiseGitFindType.ByEverything : (TortoiseGitFindType)result;
        }

        private void TortoiseLogSearch_Shown(object sender, System.EventArgs e)
        {
            LoadFindType();
            tbFindString.SelectionLength = 0;
            tbFindString.SelectionStart = tbFindString.Text.Length;
        }

        private void TortoiseLogSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            hook.Uninstall();
            SaveFindType();
        }

        private void TortoiseLogSearch_Deactivate(object sender, System.EventArgs e)
        {
            Close();
        }

        private void bSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }
    }
}
