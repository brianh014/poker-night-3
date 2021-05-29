using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Common.Contracts;
using Common.Services;
using Games.Services;
using Players.Contracts;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PlayerResults
{
    public class Function
    {

        /// <summary>
        /// A function that gets and modifies player results
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IResponse> FunctionHandler(Request input, ILambdaContext context)
        {
            var handler = new LambdaHandler(input, new PlayerResultService(input));
            return await handler.GetResponse();
        }
    }
}
