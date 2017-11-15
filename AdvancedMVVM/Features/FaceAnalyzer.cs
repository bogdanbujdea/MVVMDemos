using System.Collections.Generic;
using System.Linq;
using AdvancedMVVM.Models;
using AdvancedMVVM.ViewModels;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.Features
{
    public class FaceAnalyzer : IFaceAnalyzer
    {
        public List<FaceInfo> ProcessFaces(List<Face> faces, double heightScale, double widthScale)
        {
            var uiFaces = new List<FaceInfo>(faces.Select(f => new FaceInfo
            {
                Age = f.FaceAttributes.Age,
                Glasses = f.FaceAttributes.Glasses,
                Happiness = f.FaceAttributes.Emotion.Happiness,
                ScaledFaceRectangle = new FaceRectangle
                {
                    Height = (int)(f.FaceRectangle.Height * heightScale),
                    Width = (int)(f.FaceRectangle.Width * widthScale),
                    Left = (int)(f.FaceRectangle.Left * widthScale),
                    Top = (int)(f.FaceRectangle.Top * heightScale)
                },
                OriginalFaceRectangle = f.FaceRectangle,
                EmotionType = GetEmotionType(f.FaceAttributes.Emotion),
                Id = f.FaceId.ToString()
            }).ToList());
            return uiFaces;
        }

        private static EmotionType GetEmotionType(EmotionScores emotions)
        {
            if (emotions.Happiness > 0.7)
                return EmotionType.Happy;
            if (emotions.Anger > 0.7)
                return EmotionType.Angry;
            return EmotionType.Neutral;
        }
    }
}