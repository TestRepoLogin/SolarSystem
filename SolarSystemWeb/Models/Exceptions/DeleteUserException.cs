using System;

namespace SolarSystemWeb.Models.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое в случае ошибки, 
    /// возникшей при удалении пользователя
    /// </summary>
    public class DeleteUserException : Exception
    {
        public DeleteUserException()
        {
        }

        public DeleteUserException(string message)
        : base(message)
        {
        }

        public DeleteUserException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}