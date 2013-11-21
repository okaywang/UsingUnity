
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new  UnityContainer();

            container.RegisterType<ILogger, DatabaseLogger>();
            var logger = container.Resolve<ILogger>();
            logger.Write("install IIS service");


            var bllPowerPolicy = container.Resolve<PowerPolicyBussinesslogic>();
            bllPowerPolicy.Save();

            var bllPowerPolicy2 = container.Resolve<PowerPolicyBussinesslogic2>();
            bllPowerPolicy2.Save();
        }
    }

    class PowerPolicyBussinesslogic
    {
        private ILogger _logger;

        [Dependency]
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        } 
        public void Save()
        {
            _logger.Write("power policy saved");
        }
    }

    class PowerPolicyBussinesslogic2
    {
        private ILogger _logger;

        public PowerPolicyBussinesslogic2(ILogger logger)
        {
            _logger = logger;
        }

        public void Save()
        {
            _logger.Write("power policy saved");
        }
    }
}
