using System.IO;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Class used to mimic the e-mail sender system
    /// </summary>
    public class FileMessageService : IMessageService
    {
        /// <summary>
        /// Saves mail data into the file
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="message">Mail message</param>
        /// <returns>Completed successfully task</returns>
        public Task Send(string email, string subject, string message)
        {
            var emailMessage = $"To: {email}\nSubject: {subject}\nMessage: {message}\n\n";

            File.AppendAllText("verificationEmails.txt", emailMessage);

            return Task.CompletedTask;
        }
    }
}