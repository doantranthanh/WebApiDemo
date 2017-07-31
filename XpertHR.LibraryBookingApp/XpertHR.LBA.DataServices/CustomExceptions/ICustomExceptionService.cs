using System.Threading.Tasks;

namespace XpertHR.LBA.DataServices.CustomExceptions
{
    public interface ICustomExceptionService
    {
        Task ThrowItemNotFoundException();
        Task ThrowArgumentNullException();
    }
}