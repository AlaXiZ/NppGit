// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

using System;

namespace NppKate.Common
{
    public interface IFormManager
    {
        /// <summary>
        /// Переключение состояния прикрепляемой формы
        /// </summary>
        /// <param name="hwnd">Дескриптор окна</param>
        /// <returns>Возвращает текущее состояние</returns>
        bool ToogleVisibleDockableForm(IntPtr hwnd);
        /// <summary>
        /// Конструктор форм
        /// </summary>
        /// <typeparam name="T">Класс формы, наследующий DockDialog</typeparam>
        /// <param name="commandIndex">Номер команды</param>
        /// <param name="dockParam">Параметры для менеджера окон</param>
        /// <param name="iconHandle">Ссылка на иконку в закладке</param>
        /// <param name="dockableManager">Ссылка на IDockableManager</param>
        /// <returns>Экзепляр формы</returns>
        T BuildForm<T>(int commandIndex, NppTbMsg dockParam, IntPtr iconHandle, IDockableManager dockableManager = null) where T : DockDialog, new();
    }
}
