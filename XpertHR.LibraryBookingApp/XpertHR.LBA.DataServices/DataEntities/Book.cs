using System;

namespace XpertHR.LBA.DataServices.DataEntities
{
    public sealed class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Rate { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}
