using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Images.Exif.Lib.Services
{
    public class ExifService : Interfaces.IExifService
    {
        public IDictionary<string, string> GetProperties(Image image)
        {
            IDictionary<string, string> res
            = new Dictionary<string, string>();

            PropertyItem[] props = image.PropertyItems.ToArray();

            PropertyItem pi;

            //Latitude
            pi = props.FirstOrDefault(p => p.Id == 0x0001);

            if (pi != null)
            {
                //S or N
                string latitudeRef = ASCIIEncoding.ASCII.GetString(pi.Value, 0, pi.Len).Replace("\0", "");

                pi = props.FirstOrDefault(p => p.Id == 0x0002);

                if (pi != null)
                {
                    double dn = BitConverter.ToUInt32(pi.Value, 0);
                    double dd = BitConverter.ToUInt32(pi.Value, 4);
                    double mn = BitConverter.ToUInt32(pi.Value, 8);
                    double md = BitConverter.ToUInt32(pi.Value, 12);
                    double sn = BitConverter.ToUInt32(pi.Value, 16);
                    double sd = BitConverter.ToUInt32(pi.Value, 20);

                    if (latitudeRef == "N")
                    {
                        res.Add("Latitude", (dn / dd + mn / md / 60.0 + sn / sd / 3600.0).ToString());
                    }
                    else
                    {
                        res.Add("Latitude", (-dn / dd - mn / md / 60.0 - sn / sd / 3600.0).ToString());
                    }
                }
            }

            //Longitude
            pi = props.FirstOrDefault(p => p.Id == 0x0003);

            if (pi != null)
            {
                string longituderef = ASCIIEncoding.ASCII.GetString(pi.Value, 0, pi.Len).Replace("\0", "");

                pi = props.FirstOrDefault(p => p.Id == 0x0004);

                if (pi != null)
                {
                    double dn = BitConverter.ToUInt32(pi.Value, 0);
                    double dd = BitConverter.ToUInt32(pi.Value, 4);
                    double mn = BitConverter.ToUInt32(pi.Value, 8);
                    double md = BitConverter.ToUInt32(pi.Value, 12);
                    double sn = BitConverter.ToUInt32(pi.Value, 16);
                    double sd = BitConverter.ToUInt32(pi.Value, 20);

                    if (longituderef == "E")
                    {
                        res.Add("Longitude", (dn / dd + mn / md / 60.0 + sn / sd / 3600.0).ToString());
                    }
                    else
                    {
                        res.Add("Longitude", (-dn / dd - mn / md / 60.0 - sn / sd / 3600.0).ToString());
                    }
                }
            }

            //Createion Time
            pi = props.FirstOrDefault(p => p.Id == 0x0132);

            if (pi != null)
            {
                res.Add("DateTime", Encoding.ASCII.GetString(pi.Value, 0, pi.Len));
            }

            //Format
            pi = props.FirstOrDefault(p => p.Id == 0x1000);

            if (pi != null)
            {
                res.Add("Format", Encoding.ASCII.GetString(pi.Value, 0, pi.Len));
            }

            //Make
            pi = props.FirstOrDefault(p => p.Id == 0x010f);

            if (pi != null)
            {
                res.Add("Make", Encoding.ASCII.GetString(pi.Value, 0, pi.Len));
            }

            //Model
            pi = props.FirstOrDefault(p => p.Id == 0x0110);

            if (pi != null)
            {
                res.Add("Model", Encoding.ASCII.GetString(pi.Value, 0, pi.Len));
            }

            return res;
        }
    }
}
