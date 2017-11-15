using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.Features
{
    public class FaceDetector : IFaceDetector
    {
        private FaceServiceClient _faceServiceClient;
        private string _personGroupId;

        public FaceDetector()
        {
            _faceServiceClient = new FaceServiceClient("21d25f80b82048bcbf74bcf6c28988bd");
            _personGroupId = "issco-colleagues3323";
        }

        public async Task<List<Face>> DetectFaces(SoftwareBitmap softwareBitmap)
        {
            try
            {
                InMemoryRandomAccessStream destStream = new InMemoryRandomAccessStream();

                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId,
                    destStream);

                encoder.SetSoftwareBitmap(softwareBitmap);

                await encoder.FlushAsync();

                var faces = await _faceServiceClient.DetectAsync(
                    destStream.AsStream(), true, true, new List<FaceAttributeType> { FaceAttributeType.Age, FaceAttributeType.Emotion, FaceAttributeType.Gender, FaceAttributeType.Glasses });
                return faces.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new List<Face>();
            }
        }

        public async Task CreateFaceGroup()
        {
            await _faceServiceClient.CreatePersonGroupAsync(_personGroupId, "ISSCO");

            var personResult = await _faceServiceClient.CreatePersonAsync(_personGroupId, "BogdanB");
            var isscoFolder = await KnownFolders.PicturesLibrary.GetFolderAsync("issco");
            var bogdanbFolder = await isscoFolder.GetFolderAsync("bogdanb");
            foreach (var storageFile in await bogdanbFolder.GetFilesAsync())
            {
                var openStreamForReadAsync = await storageFile.OpenStreamForReadAsync();
                await _faceServiceClient.AddPersonFaceAsync(_personGroupId, personResult.PersonId, openStreamForReadAsync);
            }
            await _faceServiceClient.TrainPersonGroupAsync(_personGroupId);
            while (true)
            {
                var trainingStatus = await _faceServiceClient.GetPersonGroupTrainingStatusAsync(_personGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }
    }
}
