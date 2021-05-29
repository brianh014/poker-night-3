using Amazon.Lambda.Core;
using Common;
using Common.Contracts;
using Common.Services;
using Players.Contracts;
using Players.Services;
using System.Net;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Players
{
    public class Function
    {
        
        /// <summary>
        /// Function to get and modify players
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IResponse> FunctionHandler(Request input, ILambdaContext context)
        {
            var handler = new LambdaHandler(input, new PlayerService(input));
            return await handler.GetResponse();
        }
    }
}
