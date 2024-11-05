using DataUploaderApp.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploaderApp
{
    internal class Helper
    {
        public static readonly DefaultDbContext DefaultDbContext = new DefaultDbContext();
    }
}
