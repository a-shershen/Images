using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Images.Exif.Lib.Interfaces
{
    public interface IExifService
    {
        IDictionary<string, string> GetProperties(System.Drawing.Image image);
    }
}
