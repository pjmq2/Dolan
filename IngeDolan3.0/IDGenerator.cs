using System;
using System.Linq;
using System.Data;
using System.Linq.Dynamic;
using IngeDolan3._0.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

//      Por ahora este generador TIENE UN DEFECTO, lo que pasa es que usa count para ver cuantas filas hay en una tabla, el problema es que si uno borra una fila, entonces al hacer count, va a agarrar el mismo ID que el anterior, por ahora lo unico que se me ocurre es hacer una tabla con cada fila siendo un string del nombre de la tabla, y un int el numero de columnas que se han metido en la tabla (así no decrece)
namespace IngeDolan3._0.Generator
{
    public static class IDGenerator
    {
        public static string IntToString(int value)
        {
            char[] baseChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x' };
            int i = 40;
            char[] result = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                result[--i] = baseChars[value % targetBase];
                value = value / targetBase;
            }
            while (i > 0);

            return new string(result);
        }

        public static bool CanDo(string permission)
        {
            NewDolan2Entities db = new NewDolan2Entities();
            String userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                var modelUser = db.AspNetUsers.Where(x => x.Id == userId).ToList().FirstOrDefault();
                var user2 = modelUser.Users.FirstOrDefault();
                var userRole = user2.AspNetRole;
                var permisos = userRole.Permisos;

                //if found return true
                foreach (var per in permisos)
                {
                    if (per.nombre == permission)
                    {
                        return true;
                    }
                }
                //if it hasnt returned by now then it must be the user does not have permission
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}