using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Common;

namespace NppKateTest.CommandManager
{
    [TestClass]
    public class CmdManagerTest
    {
        const string Module = "Test";
        const string Module2 = "Test2";
        [TestMethod]
        public void CheckDoubleSeparator()
        {
            var cmdManager = new NppKate.Common.CommandManager();
            cmdManager.RegisterCommand(Module, "1");
            cmdManager.RegisterSeparator(Module);
            cmdManager.RegisterSeparator(Module);

            Assert.AreEqual<int>(cmdManager.GetCommandsByModule(Module).Count, 2);
        }

        [TestMethod]
        public void CheckFirstSeparator()
        {
            var cmdManager = new NppKate.Common.CommandManager();
            cmdManager.RegisterSeparator(Module);

            Assert.AreEqual<int>(cmdManager.GetCommandsByModule(Module).Count, 0);
        }
        
        private void AddCommands(ICommandManager cmd, int count, string module)
        {
            for (int i = 0; i < count; i++)
            {
                cmd.RegisterCommand(module, $"command_{i}");
            }
        }

        [TestMethod]
        public void AddOneCommand()
        {
            var cmdManager = new NppKate.Common.CommandManager();
            AddCommands(cmdManager, 1, Module);
            Assert.AreEqual<int>(cmdManager.GetCommandsByModule(Module).Count, 1);
        }
        [TestMethod]
        public void AddManyCommand()
        {
            var cmdManager = new NppKate.Common.CommandManager();
            var count = 10;
            AddCommands(cmdManager, count, Module);
            Assert.AreEqual<int>(cmdManager.GetCommandsByModule(Module).Count, count);
        }
    }
}
