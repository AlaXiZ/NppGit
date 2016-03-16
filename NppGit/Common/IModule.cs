namespace NppGit.Common
{
    public interface IModule
    {
        void Init(IModuleManager manager);
        void Final();
        bool IsNeedRun { get; }
    }
}
