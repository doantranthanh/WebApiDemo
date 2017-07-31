using System;
using System.Threading.Tasks;

namespace XpertHR.LBA.DataServices.CustomExceptions
{
    public class CustomExceptionService : ICustomExceptionService
    {
        public Task ThrowItemNotFoundException()
        {
            return Task.Run(() =>
            {
                throw new ItemNotFoundException("This is a custom exception.");
            });          
        }

        public Task ThrowArgumentNullException()
        {
            return Task.Run(() =>
            {
                throw new ArgumentNullException("This is a custom argument null exception.");
            });
        }
    }
}
