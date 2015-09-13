using System;

namespace SolarSystemWeb.Models.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое в случае ошибки, связанной с ролями пользователей
    /// </summary>
    public class PermissionException : Exception
    {
        public PermissionException()
        {
        }

        public PermissionException(string message)
        : base(message)
        {
        }

        public PermissionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}