using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Common.Contracts;
using Common.Services;
using Games.Contracts;
using Games.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Games
{
    public class Function
    {

        /// <summary>
        /// A function to get and modify games
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<IResponse> FunctionHandler(Request input, ILambdaContext context)
        {
            var handler = new LambdaHandler(input, new GameService(input));
            return await handler.GetResponse();
        }
    }
}
