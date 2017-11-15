using Caliburn.Micro;

namespace AdvancedMVVM.Controls
{
    public sealed partial class StatisticsControl
    {
        public StatisticsControl()
        {
            InitializeComponent();
        }
    }

    public class PresentationStatistics: PropertyChangedBase
    {
        private int _usersWithGlasses;
        private int _neutralUsers;
        private int _happyUsers;
        private int _angryUsers;
        private int _detectedFaces;
        private double _ageAverage;

        public int UsersWithGlasses
        {
            get { return _usersWithGlasses; }
            set
            {
                if (value == _usersWithGlasses) return;
                _usersWithGlasses = value;
                NotifyOfPropertyChange(() => UsersWithGlasses);
            }
        }

        public int NeutralUsers
        {
            get { return _neutralUsers; }
            set
            {
                if (value == _neutralUsers) return;
                _neutralUsers = value;
                NotifyOfPropertyChange(() => NeutralUsers);
            }
        }

        public int HappyUsers
        {
            get { return _happyUsers; }
            set
            {
                if (value == _happyUsers) return;
                _happyUsers = value;
                NotifyOfPropertyChange(() => HappyUsers);
            }
        }

        public int AngryUsers
        {
            get { return _angryUsers; }
            set
            {
                if (value == _angryUsers) return;
                _angryUsers = value;
                NotifyOfPropertyChange(() => AngryUsers);
            }
        }

        public int DetectedFaces
        {
            get { return _detectedFaces; }
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
    }
}
