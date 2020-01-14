using System.ComponentModel.DataAnnotations;

namespace ProgrammaticFiltering.Models
{
    #region snippet1
    public class ProgrammaticFilter
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Filter { get; set; }
        public string EmbedId { get; set; }
    }
    #endregion
}