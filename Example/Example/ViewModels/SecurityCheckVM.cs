using AeroGear.Mobile.Core;
using AeroGear.Mobile.Security;
using Example.Models;
using Example.ViewModels.Base;
using ListDiff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Example.ViewModels
{
    class SecurityCheckVM : BaseVM
    {
        public ObservableCollection<CheckModel> Checks { get; private set; }
        public bool Loading = false;
        List<ISecurityCheck> securityCheckList;

        private ISecurityService securityService = MobileCore.Instance.GetService<ISecurityService>();

        public SecurityCheckVM(List<ISecurityCheck> securityCheckList)
        {
            Checks = new ObservableCollection<CheckModel>();
            this.securityCheckList = securityCheckList;
        }

        public async Task Fetch()
        {
            Loading = true;
            await Task.Run(()=> {
                List<CheckModel> newModels = new List<CheckModel>();
                foreach (ISecurityCheck check in securityCheckList)
                {
                    var result = securityService.Check(check);
                    var model = new CheckModel();
                    model.beenRun = true;
                    model.passed = result.Passed;
                    model.name = result.Name;
                    newModels.Add(model);
                }

                Checks.MergeInto(newModels, (model1, model2) =>  model1.name == model2.name);

                Loading = false;
            });
           
        }

    }
}
