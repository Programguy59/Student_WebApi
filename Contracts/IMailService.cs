using System.Threading.Tasks;

namespace Contracts
{
    public interface IMailService
    {
        public void Send(string subject, string message);
    }
}