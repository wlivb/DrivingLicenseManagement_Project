using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace EmailSendingService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "EmailSendingService";
            serviceInstaller.DisplayName = "DVLD Email Sending Service";
            serviceInstaller.Description = "Emails are automatically sent to drivers when their license expires.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);

            InitializeComponent();
        }
    }
}