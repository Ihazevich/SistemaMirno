// <copyright file="ViewModelBase.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// Base view model class.
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase, INotifyPropertyChanged
    {
        protected IEventAggregator _eventAggregator;
        private Visibility _viewVisibility;

        public ViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ExitView = new DelegateCommand(OnExitViewExecute);

            NotifyStatusBar("Generando vista", true);

            ViewVisibility = Visibility.Hidden;
        }

        public Visibility ViewVisibility
        {
            get => _viewVisibility;

            set
            {
                _viewVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExitView { get; }

        public abstract Task LoadAsync(int? id);

        private void OnExitViewExecute()
        {
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (PropertyChanged != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(propertyName))));
                }
            }
        }

        protected async void NotifyStatusBar(string message, bool processing)
        {
            await Task.Run( () =>
            {
                _eventAggregator.GetEvent<NotifyStatusBarEvent>().Publish(new NotifyStatusBarEventArgs { Message = message, Processing = processing });
            });
        }

        protected async void ClearStatusBar()
        {
            await Task.Run(() =>
            {
                _eventAggregator.GetEvent<NotifyStatusBarEvent>().Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
            });
        }
    }
}