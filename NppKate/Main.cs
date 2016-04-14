/*
Copyright (c) 2015-2016, Schadin Alexey (schadin@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
and the following disclaimer in the documentation and/or other materials provided with 
the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse 
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using NppKate.Common;
using NppKate.Modules;
using NppKate.Modules.GitCore;
using NppKate.Modules.IdeFeatures;
using NppKate.Modules.SnippetFeature;
using NppKate.Modules.TortoiseGitFeatures;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace NppKate
{
    class Main
    {
        #region "Fields"
        private static ModuleManager mm = new ModuleManager();
        private static readonly IList<Type> _excludedTypes = new ReadOnlyCollection<Type>(
            new List<Type> {
                typeof(GitCore)
            });
        #endregion

        #region " StartUp/CleanUp "
        public static void Init()
        {
            LoadModules();
            mm.AddModule(GitCore.Module); // TODO: Переделать на автоматическую загрузку

            mm.Init();

            NppInfo.Instance.AddCommand("Restart Notepad++", NppUtils.Restart);
            NppInfo.Instance.AddCommand("Settings", DoSettings);
            NppInfo.Instance.AddCommand("About", DoAbout);
        }

        public static void ToolBarInit()
        {
            mm.ToolBarInit();
        }

        public static void PluginCleanUp()
        {
            mm.Final();
        }

        public static void MessageProc (SCNotification sn)
        {
            mm.MessageProc(sn);
        }

        private static void LoadModules()
        {
            var imodule = typeof(IModule);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => imodule.IsAssignableFrom(p))
                        .Where(p => p.IsClass)                    // Нужны только классы
                        .Where(p => !_excludedTypes.Contains(p))  // Исключим классы, которые по какой-то причине создаются вручную
                        .OrderBy(p => p.Name);                    // Отсортируем по имени
            // Создаем модули, добавляем в менеджер
            foreach (var t in types)
            {
                var module = Activator.CreateInstance(t) as IModule;
                mm.AddModule(module);
            }
        }

        #endregion

        #region " Menu functions "
        private static void DoSettings()
        {
            var dlg = new Forms.SettingsDialog();
            dlg.ShowDialog();
        }

        private static void DoAbout()
        {
            var dlg = new Forms.About();
            dlg.ShowDialog();
            mm.ManualTitleUpdate();
        }

        /*
        static void GitStatusDialog()
        {
            var isVisible = 0;
            if (gitStatusDlg == null)
            {
                gitStatusDlg = new GitStatus();

                tbIcon = NppUtils.NppBitmapToIcon(Properties.Resources.Git);

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = gitStatusDlg.Handle;
                _nppTbData.pszName = "Git status";
                _nppTbData.dlgID = gitStatusId;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = Properties.Resources.PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                isVisible = 1;
            }
            else
            {
                if (gitStatusDlg.Visible)
                {
                    Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_DMMHIDE, 0, gitStatusDlg.Handle);
                }
                else
                {
                    Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_DMMSHOW, 0, gitStatusDlg.Handle);
                    isVisible = 1;
                }
            }
            if (gitStatusDlg != null && gitStatusDlg.Visible)
            {
                (gitStatusDlg as FormDockable).ChangeContext();
            }

           Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, NppInfo.Instance.SearchCmdIdByIndex(gitStatusId), isVisible);
        }
        */
        /*
        public static void ChangeTabItem()
        {
            if (gitStatusDlg != null)
                (gitStatusDlg as FormDockable).ChangeContext();
        }
        */
        #endregion
    }
}