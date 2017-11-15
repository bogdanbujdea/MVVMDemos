using Caliburn.Micro;

namespace AdvancedMVVM.Models
{
    public class PresentationStatistics: PropertyChangedBase
    {
        private int _usersWithGlasses;
        private int _neutralUsers;
        private int _happyUsers;
        private int _angryUsers;
        private int _detectedFaces;
        private double _ageAverage;
        private int _callCount;
        private int _totalHappyUsers;

        public int UsersWithGlasses
        {
            get => _usersWithGlasses;
            set
            {
                if (value == _usersWithGlasses) return;
                _usersWithGlasses = value;
                NotifyOfPropertyChange(() => UsersWithGlasses);
            }
        }

        public int NeutralUsers
        {
            get => _neutralUsers;
            set
            {
                if (value == _neutralUsers) return;
                _neutralUsers = value;
                NotifyOfPropertyChange(() => NeutralUsers);
            }
        }

        public int HappyUsers
        {
            get => _happyUsers;
            set
            {
                if (value == _happyUsers) return;
                _happyUsers = value;
                NotifyOfPropertyChange(() => HappyUsers);
            }
        }

        public int AngryUsers
        {
            get => _angryUsers;
            set
            {
                if (value == _angryUsers) return;
                _angryUsers = value;
                NotifyOfPropertyChange(() => AngryUsers);
            }
        }

        public int DetectedFaces
        {
            get => _detectedFaces;
            set
            {
                if (value == _detectedFaces) return;
                _detectedFaces = value;
                NotifyOfPropertyChange(() => DetectedFaces);
            }
        }

        public double AgeAverage
        {
            get => _ageAverage;
            set
            {
                if (value.Equals(_ageAverage)) return;
                _ageAverage = value;
                NotifyOfPropertyChange(() => AgeAverage);
            }
        }

        public int CallCount
        {
            get => _callCount;
            set
            {
                if (value == _callCount) return;
                _callCount = value;
                NotifyOfPropertyChange(() => CallCount);
            }
        }

        public int TotalHappyUsers
        {
            get => _totalHappyUsers;
            set
            {
                if (value == _totalHappyUsers) return;
                _totalHappyUsers = value;
                NotifyOfPropertyChange(() => TotalHappyUsers);
            }
        }
    }
}