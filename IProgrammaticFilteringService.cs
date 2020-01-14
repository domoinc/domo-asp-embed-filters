using ProgrammaticFiltering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammaticFiltering
{
    public interface IProgrammaticFilteringService
    {
        ProgrammaticFilter getProgrammaticFilter(string username);
    }
}
