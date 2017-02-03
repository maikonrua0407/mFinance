using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Process.Common
{
    public class ClientResponseDetail
    {
        public int Stt { get; set; }
        public int Id { get; set; }
        public string Object { get; set; }
        public string Operation { get; set; }
        public string Result { get; set; }
        public string Detail { get; set; }
    }
}
