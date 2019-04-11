using System;
using MvvmCross.Core.ViewModels;
using Glimpse.Core.Utility;

namespace Glimpse.Core.Model.App
{
    public class MenuItem : MvxViewModel
    {
        private bool _isSelected;

        public string Title { get; set; }
        public Type ViewModelType { get; set; }
        public MenuOption Option { get; set; }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}