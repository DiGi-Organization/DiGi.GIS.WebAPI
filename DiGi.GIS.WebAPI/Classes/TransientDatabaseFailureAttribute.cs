using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using System;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Answers a transient database failure that escapes an action with HTTP 503 and <c>Retry-After: 30</c> - the "retry can succeed" response the hand-written <c>catch (NpgsqlException exception) when (exception.IsTransient)</c> blocks give - instead of the host's uniform 500 (DiGi-Organization/DiGi.GIS.WebAPI#71).
    /// <para>Applied at class level to every GIS controller, so an action that has no catch of its own, and every action added later, keeps the contract. An action with a generic <c>catch (Exception)</c> must let the transient failure through (<c>when (exception is not NpgsqlException { IsTransient: true })</c>) for this filter to see it. Any other exception is left unhandled, so the host still answers 500.</para>
    /// <para>A failure counts as transient when the exception or one of its inner exceptions is an <see cref="NpgsqlException"/> with <see cref="NpgsqlException.IsTransient"/> set - a refused or dropped connection, a timeout, or a server that is starting up, in recovery or out of connection slots.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TransientDatabaseFailureAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The seconds a client is told to wait before retrying, matching the hand-written blocks.
        /// </summary>
        public const string RetryAfter = "30";

        /// <summary>
        /// The response body, matching the hand-written blocks.
        /// </summary>
        public const string Message = "Database temporarily unavailable; retry shortly";

        /// <inheritdoc/>
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled || !IsTransient(context.Exception))
            {
                return;
            }

            ControllerActionDescriptor? controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            Serilog.Modify.Log(context.Exception, "{Type}:{Name} failed (transient database failure)", controllerActionDescriptor?.ControllerTypeInfo.Name ?? context.ActionDescriptor.DisplayName ?? string.Empty, controllerActionDescriptor?.MethodInfo.Name ?? string.Empty);

            context.HttpContext.Response.Headers["Retry-After"] = RetryAfter;
            context.Result = new ObjectResult(Message) { StatusCode = 503 };
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Checks whether an exception, or one of its inner exceptions, is a transient <see cref="NpgsqlException"/>.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns><see langword="true"/> when a retry can succeed; otherwise <see langword="false"/>.</returns>
        public static bool IsTransient(Exception? exception)
        {
            for (Exception? exception_Temp = exception; exception_Temp is not null; exception_Temp = exception_Temp.InnerException)
            {
                if (exception_Temp is NpgsqlException { IsTransient: true })
                {
                    return true;
                }
            }

            return false;
        }
    }
}
