using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppKate.Modules.SnippetFeature
{
    public class ParamFactory : IParamFactory
    {
        private static readonly IParam Empty = new NotProcessed();

        private readonly Dictionary<string, WeakReference> _paramCache;
        private readonly ISnippetManager _snippetManager;

        public ParamFactory(ISnippetManager snippetManager)
        {
            _paramCache = new Dictionary<string, WeakReference>();
            _snippetManager = snippetManager;
        }

        public IParam GetAutoParamByName(string name)
        {
            IParam outParam = null;
            if (_paramCache.ContainsKey(name))
            {
                if (_paramCache[name].IsAlive)
                    outParam = _paramCache[name].Target as IParam;
            }
            if (outParam == null)
            {
                outParam = Creator(name);
                _paramCache.Add(name, new WeakReference(outParam));
            }
            return outParam;
        }

        private IParam Creator(string name)
        {
            var iparam = typeof(IParam);
            var type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => iparam.IsAssignableFrom(p))
                        .Where(p => p.IsClass)                    // Нужны только классы
                        .Where(p => p.Name.Contains(name)         // Ищем по имени
                        ).FirstOrDefault();

            if (type == null) return Empty;
            return (IParam)Activator.CreateInstance(type);
        }
    }

    internal class NotProcessed : IParam
    {
        public void Process(ref StringBuilder buffer) { }
    }

    internal class UserNameParam : IParam
    {
        public void Process(ref StringBuilder buffer)
        {
            throw new NotImplementedException();
        }
    }

    internal class DateParam : IParam
    {
        public void Process(ref StringBuilder buffer)
        {
            var date = DateTime.Now.ToString("dd.MM.yyyy");
            buffer.Replace("$(DATE)", date);
        }
    }
}
