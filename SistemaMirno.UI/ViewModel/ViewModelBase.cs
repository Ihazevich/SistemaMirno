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
    /// Represents the base class for all view models.
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase, INotifyPropertyChanged
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _name;
        private Visibility _progressVisibility;
        private SessionInfo _sessionInfo;
        private Visibility _viewVisibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggreagator.</param>
        /// <param name="name">The name of the view model.</param>
        /// <param name="dialogCoordinator">The dialog coordinator.</param>
        protected ViewModelBase(IEventAggregator eventAggregator, string name, IDialogCoordinator dialogCoordinator)
        {
            _eventAggregator = eventAggregator;
            _name = name;

            _dialogCoordinator = dialogCoordinator;
            ExitView = new DelegateCommand(OnExitViewExecute);

            ProgressVisibility = Visibility.Visible;
            ViewVisibility = Visibility.Collapsed;

            EventAggregator.GetEvent<BroadcastSessionInfoEvent>()
                .Subscribe(GetSessionInfo);
            EventAggregator.GetEvent<AskSessionInfoEvent>().Publish();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the command used to close the view.
        /// </summary>
        public ICommand ExitView { get; }

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> parameter used for
        /// all progress-related UI elements.
        /// </summary>
        public Visibility ProgressVisibility
        {
            get => _progressVisibility;

            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current session information.
        /// </summary>
        public SessionInfo SessionInfo
        {
            get => _sessionInfo;

            set
            {
                _sessionInfo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> parameter used for
        /// all non-progress-related UI elements.
        /// </summary>
        public Visibility ViewVisibility
        {
            get => _viewVisibility;

            set
            {
                _viewVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current dialog coordinator.
        /// </summary>
        protected IDialogCoordinator DialogCoordinator => _dialogCoordinator;

        /// <summary>
        /// Gets the current event aggregator.
        /// </summary>
        protected IEventAggregator EventAggregator => _eventAggregator;

        /// <inheritdoc/>
        public abstract Task LoadAsync(int? id = null);

        /// <summary>
        /// Clears the status bar message and hides the progress bar.
        /// </summary>
        protected void ClearStatusBar()
        {
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
        }

        /// <summary>
        /// Sends a message to the status bar and sets the progress bar status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="processing">A value indicating whether the progress bar is active or not.</param>
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

        /// <summary>
        /// Sets the session information when a <see cref="BroadcastSessionInfoEvent"/> is received.
        /// </summary>
        /// <param name="obj">The session information.</param>
        private void GetSessionInfo(SessionInfo obj)
        {
            SessionInfo = obj;
        }

        /// <summary>
        /// Raises the <see cref="ChangeNavigationStatusEvent"/> event when the view is closed.
        /// </summary>
        private void OnExitViewExecute()
        {
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }
    }
}