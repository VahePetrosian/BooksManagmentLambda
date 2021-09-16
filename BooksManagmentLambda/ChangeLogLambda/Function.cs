using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ChangeLogLambda
{
  public class Function
  {
    public Function()
    {

    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
      foreach (var message in evnt.Records)
      {
        await ProcessMessageAsync(message, context);
      }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
      var client = new AmazonS3Client(RegionEndpoint.EUCentral1);

      var request = new PutObjectRequest
      {
        BucketName = "log-bucket-books-v-luigfsddasf",
        Key = $"{DateTime.Now.ToString("yyyy/MM/dd/hh-mm-ss")}-log.txt",
        ContentBody = message.Body,
        ContentType = "text/plain",
        StorageClass = S3StorageClass.Standard,
        CannedACL = S3CannedACL.NoACL
      };

      await client.PutObjectAsync(request);
    }
  }
}
