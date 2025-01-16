using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Text;
using wms.infrastructure.Logging;
using wms.infrastructure.Models;

namespace wms.infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var uri = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);

                if (!uri.Contains("/api"))
                {
                    await _next(context);
                    return;
                }

                StringValues requestId = string.Empty;
                StringValues username = string.Empty;
                StringValues origin = string.Empty;
                StringValues sessionId = string.Empty;
                StringValues token = string.Empty;
                DateTime reqDate = DateTime.Now;

                var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString();

                if (context.Request.Headers.TryGetValue("ContextID", out requestId))
                {
                    requestId = requestId.ToString();
                }
                else
                {
                    requestId = Guid.NewGuid().ToString().Replace("-", "");
                    context.Items.Add("ContextID", requestId);
                }

                if (string.IsNullOrEmpty(traceId))
                {
                    traceId = requestId;
                }

                if (context.Request.Headers.TryGetValue("Authorization", out token))
                {
                    token = token.ToString().Replace("Bearer ", "");
                }

                if (context.Request.Headers.TryGetValue("UserID", out username))
                {
                    username = username.ToString();
                }
                else if (!string.IsNullOrEmpty(token))
                {
                    username = GetUsernameFromToken(token);
                }

                if (context.Request.Headers.TryGetValue("Origin", out origin))
                {
                    origin = origin.ToString();
                }

                if (context.Request.Headers.TryGetValue("SessionID", out sessionId))
                {
                    sessionId = sessionId.ToString();
                }

                await FormatRequest(context.Request, uri, requestId, username, origin, sessionId, traceId);

                string bodyText = string.Empty;
                int codestatus = 200;
                int contentLenght = 0;
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    codestatus = context.Response.StatusCode;
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                _ = FormatResponse(bodyText, codestatus, contentLenght, requestId, uri, username, origin, sessionId, reqDate, traceId);
            }
            catch (Exception ex)
            {
                LogHelper.WriteSystemLog($"Log middleware reading error {ex.Message} stack:{ex.StackTrace}");
            }

        }

        private string GetUsernameFromToken(string token)
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var claim = handler.ReadJwtToken(token).Claims;
                var user = new UserPrincipalLog(claim);

                return user?.Username;
            }
            catch (Exception ex)
            {
                _logger.Error(new LogIdentify()
                {
                    ProcessID = Guid.NewGuid().ToString()
                }, ex.Message);
            }
            return string.Empty;
        }

        private async Task FormatRequest(HttpRequest request, string uri, string requestId, string UserName, string origin, string sessionId, string traceID)
        {
            if (string.IsNullOrEmpty(uri) || uri.ToLower().Contains("health"))
            {
                return;
            }

            request.EnableBuffering();

            var sb = new StringBuilder("{ContextID} {ClientID} {TraceID} {Method} {Uri} {ContentLength}");
            var propertyValues = new List<object>() { requestId, "", traceID, request.Method, uri, request.ContentLength };


            if (request.ContentLength > 0 && request.ContentLength <= 131072)
            {
                // We now need to read the request stream.First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                //...Then we copy the entire request stream into the new buffer.
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                var bodyAsText = Encoding.UTF8.GetString(buffer);

                //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
                request.Body.Seek(0, SeekOrigin.Begin);

                sb.Append(" {Body}"); propertyValues.Add(bodyAsText);
            }

            if (!string.IsNullOrEmpty(UserName))
            {
                sb.Append(" {UserName}"); propertyValues.Add(UserName);
            }

            if (!string.IsNullOrEmpty(origin))
            {
                sb.Append(" {Origin}"); propertyValues.Add(origin);
            }

            if (!string.IsNullOrEmpty(sessionId))
            {
                sb.Append(" {SessionID}"); propertyValues.Add(sessionId);
            }

            _logger.Info(sb.ToString(), propertyValues.ToArray());
        }

        private async Task FormatResponse(string bodyText, int statusCode, int contentLenght, string requestId, string uri, string UserName, string origin, string sessionId, DateTime reqDate, string traceID)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(uri) || uri.ToLower().Contains("health"))
                    {
                        return;
                    }
                    contentLenght = bodyText.Length;
                    StringBuilder sb = new StringBuilder("{ContextID} {ClientID} {TraceID} {Uri} {Status} {ReasonPhrase} {ContentLength}");
                    List<object> propertyValues = new List<object>() { requestId, "", traceID, uri, statusCode, "", contentLenght };

                    if (bodyText.Length > 0 && bodyText.Length <= 131072)
                    {
                        sb.Append(" {Response}"); propertyValues.Add(bodyText);
                    }

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        sb.Append(" {UserName}"); propertyValues.Add(UserName);
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        sb.Append(" {Origin}"); propertyValues.Add(origin);
                    }

                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        sb.Append(" {SessionID}"); propertyValues.Add(sessionId);
                    }

                    sb.Append(" {ResponseTime}"); propertyValues.Add(DateTime.Now.Subtract(reqDate).TotalMilliseconds);

                    switch (statusCode)
                    {
                        case 500:
                            _logger.Error(sb.ToString(), propertyValues.ToArray());
                            break;
                        case 204:
                        case 406:
                        case 400:
                            _logger.Warn(sb.ToString(), propertyValues.ToArray());
                            break;
                        default:
                            _logger.Info(sb.ToString(), propertyValues.ToArray());
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error(new LogIdentify()
                {
                    ProcessID = Guid.NewGuid().ToString()
                }, ex.Message);
            }

        }
    }
}
