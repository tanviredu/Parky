using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44325/";
        public static string NationalParkApiPath = APIBaseUrl+ "api/v1/nationalparks";
        public static string TrailsApiPath = APIBaseUrl +"api/v1/trails";

    }
}
