using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TiendaDeportiva.Models;

namespace TiendaDeportiva.Extensions
{
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if(value == null)
            {

   
                    // Si el carrito es nulo, crear uno nuevo
                    List<CartItem> cart = new List<CartItem>();

                    // Agregar el carrito a la sesión
                   session.SetObject("Cart", cart);
                
            }
             value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
