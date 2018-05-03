using Google.Cloud.Vision.V1;
using Litium.Application.Media;
using Litium.Blobs;
using Litium.Studio.Plugins.ImageResizer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Litium.AddOns.SmartImage.GoogleCloudVision
{
    /// <summary>
    /// An implementation of ISmartImageAnalyzer which uses Google Cloud Vision API.
    /// </summary>
    public class GoogleSmartImageAnalyzer : BaseSmartImageAnalyzer
    {
        private const int BatchSize = 16;

        public GoogleSmartImageAnalyzer(BlobService blobService, IImageResizer imageResizer, MimeExtensionHelper mimeExtensionHelper)
            : base(blobService, imageResizer, mimeExtensionHelper)
        { }

        public override IEnumerable<AnalysisResponse> Process(ConcurrentQueue<ImageQueue> queues)
        {
            var client = ImageAnnotatorClient.Create();
            var batchReqs = new List<AnnotateImageRequest>();
            var requestSystemIds = new List<Guid>();
            var response = new List<AnalysisResponse>();
            while (batchReqs.Count < BatchSize && queues.TryDequeue(out var image))
            {
                var blobStream = OpenRead(image, new System.Drawing.Size(500, 500));
                var gImage = Image.FromStream(blobStream);
                batchReqs.Add(new AnnotateImageRequest
                {
                    Image = gImage,
                    Features =
                    {
                        new Feature { Type = Feature.Types.Type.LabelDetection },
                        new Feature { Type = Feature.Types.Type.LandmarkDetection }
                    }
                });
                requestSystemIds.Add(image.SystemId);

                if (batchReqs.Count >= BatchSize)
                {
                    response.AddRange(Analyze(client, batchReqs, requestSystemIds));
                }
            }

            if (batchReqs.Any())
            {
                response.AddRange(Analyze(client, batchReqs, requestSystemIds));
            }
            return response;
        }

        private IEnumerable<AnalysisResponse> Analyze(ImageAnnotatorClient client, List<AnnotateImageRequest> requests, List<Guid> requestSystemIds)
        {
            BatchAnnotateImagesResponse response = client.BatchAnnotateImages(requests);
            var requestIndex = 0;
            foreach (var res in response.Responses)
            {
                var imageSystemId = requestSystemIds[requestIndex];
                var labels = res.LabelAnnotations.Select(a => a.Description);
                var landmarks = res.LandmarkAnnotations.Select(a => a.Description);
                requestIndex++;
                yield return new AnalysisResponse() { SystemId = imageSystemId, Tags = landmarks.Concat(labels) };
            }
            requests.Clear();
            requestSystemIds.Clear();
        }
    }
}
