using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Describes methods and properties required to send e-mail message to user
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Sends message to user
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="message">Mail message</param>
        /// <returns>Completed successfully task</returns>
         Task Send(string email, string subject, string message);
    }
}