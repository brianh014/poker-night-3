using System.Threading.Tasks;
using Amazon.Lambda.Core;
using System.Text.Json.Serialization;
using Common.Models;
using Common.Data;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NewUser
{
    public class Function
    {
        
        /// <summary>
        /// Ensure user in db from user signing up or logging in
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Event> FunctionHandler(Event input, ILambdaContext context)
        {
            LambdaLogger.Log($"EVENT: {System.Text.Json.JsonSerializer.Serialize(input)}");

            var dao = new PlayersDao();

            var foundPlayer = await dao.GetPlayerByLogin(input.Request.UserAttributes.Sub, input.Request.UserAttributes.Email);
            if (foundPlayer != null)
            {
                foundPlayer.LastLogIn = DateTime.UtcNow;
                foundPlayer.Email = input.Request.UserAttributes.Email.ToLower();
                foundPlayer.Name = input.Request.UserAttributes.Name;
                foundPlayer.LoginId = input.Request.UserAttributes.Sub;
                await dao.UpdatePlayer(foundPlayer);
                LambdaLogger.Log($"Player {foundPlayer.Id} already created. Updated.");
                return input;
            }

            var player = new Player
            {
                Id = Guid.NewGuid().ToString(),
                LoginId = input.Request.UserAttributes.Sub,
                Email = input.Request.UserAttributes.Email.ToLower(),
                Name = input.Request.UserAttributes.Name,
                LastLogIn = DateTime.UtcNow
            };
            await dao.CreatePlayer(player);
            LambdaLogger.Log($"Created player: {System.Text.Json.JsonSerializer.Serialize(player)}");
            return input;
        }
    }

    public class Event
    {
        [JsonPropertyName("request")]
        public Request Request { get; set; }
        [JsonPropertyName("response")]
        public Response Response { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("triggerSource")]
        public string TriggerSource { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("userPoolId")]
        public string UserPoolId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }

    public class Request
    {
        [JsonPropertyName("userAttributes")]
        public UserAttributes UserAttributes { get; set; }
    }

    public class UserAttributes
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
    }

    public class Response
    {
    }
}
