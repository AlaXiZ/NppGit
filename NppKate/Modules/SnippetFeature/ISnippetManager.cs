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

using System.Collections.Generic;

namespace NppKate.Modules.SnippetFeature
{
    public interface ISnippetManager
    {
        /// <summary>
        /// Метод добавляет/обновляет сниппет
        /// </summary>
        /// <param name="snippet">Сниппет</param>
        /// <param name="oldName">Старое имя снипптеа</param>
        void AddOrUpdate(Snippet snippet, string oldName = "");
        /// <summary>
        /// Метод удаляет сниппет
        /// </summary>
        /// <param name="snippet">Сниппет</param>
        void Remove(Snippet snippet);
        /// <summary>
        /// Метод удаляет сниппет
        /// </summary>
        /// <param name="snippetName">Имя сниппета</param>
        void Remove(string snippetName);
        /// <summary>
        /// Метод ищет сниппет по ПОЛНОМУ имени
        /// </summary>
        /// <param name="snippetName">Полное имя сниппета</param>
        /// <returns>Возвращается сниппет или Snippet.Null</returns>
        Snippet FindByName(string snippetName);
        /// <summary>
        /// Метод ищет сниппет по СОКРАЩЕННОМУ имени
        /// </summary>
        /// <param name="snippetShortName">Сокращенное имя сниппета</param>
        /// <returns>Возвращается сниппет или Snippet.Null</returns>
        Snippet FindByShortName(string snippetShortName);
        /// <summary>
        /// Метод ищет сниппет по ПОЛНОМУ и СОКРАЩЕННОМУ именам
        /// </summary>
        /// <param name="name">Полное или сокращенное имя сниппета</param>
        /// <returns>Возвращается сниппет или Snippet.Null</returns>
        Snippet FindByBothName(string name);
        /// <summary>
        /// Полный список сниппетов
        /// </summary>
        /// <returns>Список сниппетов</returns>
        List<Snippet> GetAllSnippets();
        /// <summary>
        /// Метод проверяет существует ли сниппет с таким ПОЛНЫМ или СОКРАЩЕННЫМ именем
        /// </summary>
        /// <param name="name">Полное или сокращенное имя сниппета</param>
        /// <returns></returns>
        bool Contains(string name);
    }
}
