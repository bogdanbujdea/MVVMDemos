using Caliburn.Micro;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.ViewModels
{
    public class FaceInfo : PropertyChangedBase
    {
        private double _age;
        private Glasses _glasses;
        private float _happiness;
        private string _id;
        private FaceRectangle _scaledFaceRectangle;

        public double Age
        {
            get => _age;
            set
            {
                if (value.Equals(_age)) return;
                _age = value;
                NotifyOfPropertyChange(() => Age);
            }
        }

        public Glasses Glasses
        {
            get => _glasses;
            set
            {
                if (value == _glasses) return;
                _glasses = value;
                NotifyOfPropertyChange(() => Glasses);
            }
        }

        public float Happiness
        {
            get => _happiness;
            set
            {
                if (value.Equals(_happiness)) return;
                _happiness = value;
                NotifyOfPropertyChange(() => Happiness);
            }
        }

        public FaceRectangle ScaledFaceRectangle
        {
            get => _scaledFaceRectangle;
            set
            {
                if (Equals(value, _scaledFaceRectangle)) return;
                _scaledFaceRectangle = value;
                NotifyOfPropertyChange(() => ScaledFaceRectangle);
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public EmotionType EmotionType { get; set; }

        public FaceRectangle OriginalFaceRectangle { get; set; }
    }
}