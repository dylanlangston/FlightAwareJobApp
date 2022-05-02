using System.Net;
using System.Text.Json;

namespace FlightAwareJobApp
{
    /// <summary>
    /// Misc Extensions
    /// </summary>
    internal static class Extensions
    {
        private static TaskFactory _taskFactory => new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static void RunSync(this Task task)
            => _taskFactory.StartNew(() => task).Unwrap().GetAwaiter().GetResult();
        public static T RunSync<T>(this Task<T> task)
            => _taskFactory.StartNew(() => task).Unwrap().GetAwaiter().GetResult();

        public static Stream ResponseBody(this HttpResponseMessage response)
            => Task.Run(() => response).ResponseBodyAsync().RunSync();
        public static async Task<Stream> ResponseBodyAsync(this Task<HttpResponseMessage> response)
            => await (await response).Content.ReadAsStreamAsync();

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
            => collection == null || collection.Count == 0;

        public static bool Is500Error(this HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.InternalServerError
                || response.StatusCode == HttpStatusCode.NotImplemented
                || response.StatusCode == HttpStatusCode.BadGateway
                || response.StatusCode == HttpStatusCode.ServiceUnavailable
                || response.StatusCode == HttpStatusCode.GatewayTimeout
                || response.StatusCode == HttpStatusCode.HttpVersionNotSupported;
        }

        public static T HandleException<T>(this Func<T> action, T defaultResponse) => HandleException<T, Exception>(action, defaultResponse);
        public static T HandleException<T, E>(this Func<T> action, T defaultResponse) where E : Exception
        {
            try
            {
                return action();
            }
            catch (E)
            {
                return defaultResponse;
            }
        }

        public static void HandleException(this Action action) => HandleException<Exception>(action);
        public static void HandleException<E>(this Action action) where E : Exception
        {
            try
            {
                action();
            }
            catch (E) { }
        }



        static JsonSerializerOptions _jsonOptions;
        static JsonSerializerOptions jsonOptions
        {
            get
            {
                if (_jsonOptions == null)
                {
                    _jsonOptions = new JsonSerializerOptions();
                }
                return _jsonOptions;
            }
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
