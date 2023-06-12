using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Dtos
{
    public class ResponseData<T>
    {
        public ResponseData()
        {
        }

        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get=>!string.IsNullOrEmpty(ErrorMessage); }
    }
}
