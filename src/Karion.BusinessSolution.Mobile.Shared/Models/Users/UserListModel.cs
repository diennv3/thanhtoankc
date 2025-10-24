using System.ComponentModel;
using Abp.AutoMapper;
using Karion.BusinessSolution.Authorization.Users.Dto;
using Xamarin.Forms;

namespace Karion.BusinessSolution.Models.Users
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto, INotifyPropertyChanged
    {
        private ImageSource _photo;

        public string FullName => Name + " " + Surname;

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                RaisePropertyChanged(nameof(Photo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}