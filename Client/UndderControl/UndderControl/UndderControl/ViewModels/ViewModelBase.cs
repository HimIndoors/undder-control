using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        private bool _isBusy;
        private string _title;
        /// <summary>
        /// If <code>true</code>, indicates the view is busy.
        /// </summary>
        /// <remarks>
        /// This is a useful property to bind an activity indicator visibility to.
        /// </remarks>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RaisePropertyChanged(nameof(IsNotBusy));
                }
            }
        }
        /// <summary>
        /// If <code>true</code>, signals the activity is not busy.
        /// </summary>
        /// <remarks>
        /// This is a useful property to bind a button enabled property to.
        /// </remarks>
        public bool IsNotBusy => !_isBusy;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
