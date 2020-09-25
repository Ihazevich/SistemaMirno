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
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _name;
        private int _dataGridIndex;
        private Visibility _progressVisibility;
        private SessionInfo _sessionInfo;
        private Visibility _viewVisibility;

        protected ViewModelBase(IEventAggregator eventAggregator, string name, IDialogCoordinator dialogCoordinator)
        {
            _eventAggregator = eventAggregator;
            _name = name;

            _dialogCoordinator = dialogCoordinator;
            ExitView = new DelegateCommand(OnExitViewExecute);

            ProgressVisibility = Visibility.Visible;
            ViewVisibility = Visibility.Collapsed;
            DataGridIndex = -1;

            EventAggregator.GetEvent<BroadcastSessionInfoEvent>()
                .Subscribe(GetSessionInfo);
            EventAggregator.GetEvent<AskSessionInfoEvent>().Publish();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public int DataGridIndex
        {
            get => _dataGridIndex;

            set
            {
                _dataGridIndex = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExitView { get; }

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;

            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

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

        protected IDialogCoordinator DialogCoordinator => _dialogCoordinator;

        protected IEventAggregator EventAggregator => _eventAggregator;

        public abstract Task LoadAsync(int? id = null);

        protected void ClearStatusBar()
        {
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
        }

        protected void NotifyStatusBar(string message, bool processing)
        {
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Publish(new NotifyStatusBarEventArgs { Message = message, Processing = processing });
        }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
                }
            }
        }

        private void GetSessionInfo(SessionInfo obj)
        {
            SessionInfo = obj;
        }

        private void OnExitViewExecute()
        {
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }
    }
}