using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UndderControl.ViewModels
{
    public class CowStatusComparisonPageViewModel : ViewModelBase
    {
        public CowStatusComparisonPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "";
        }
    }
}
