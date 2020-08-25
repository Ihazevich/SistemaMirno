// <copyright file="ViewModelBase.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
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
        private readonly IEventAggregator _eventAggregator;
        private Visibility _viewVisibility;
        private Visibility _progressVisibility;
        private string _name;
        private readonly IDialogCoordinator _dialogCoordinator;
        private int _dataGridIndex;
        private SessionInfo _sessionInfo;

        public ViewModelBase(IEventAggregator eventAggregator, string name, IDialogCoordinator dialogCoordinator)
        {
            _eventAggregator = eventAggregator;
            _name = name;

            _dialogCoordinator = dialogCoordinator;
            ExitView = new DelegateCommand(OnExitViewExecute);
            
            ProgressVisibility = Visibility.Visible;
            ViewVisibility = Visibility.Collapsed;
            DataGridIndex = -1;
        }

        protected IDialogCoordinator DialogCoordinator => _dialogCoordinator;

        protected IEventAggregator EventAggregator => _eventAggregator;

        public SessionInfo SessionInfo
        {
            get => _sessionInfo;

            set
            {
                _sessionInfo = value;
                OnPropertyChanged();
            }
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

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;

            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }
        public int DataGridIndex
        {
            get => _dataGridIndex;

            set
            {
                _dataGridIndex = value;
                OnPropertyChanged();
            }
        }
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExitView { get; }


        public abstract Task LoadAsync();

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

        protected void NotifyStatusBar(string message, bool processing)
        {
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Publish(new NotifyStatusBarEventArgs { Message = message, Processing = processing });
        }

        protected void ClearStatusBar()
        {
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
        }

        protected virtual void CloseDetailView()
        {
            DataGridIndex = -1;
        }
    }
}