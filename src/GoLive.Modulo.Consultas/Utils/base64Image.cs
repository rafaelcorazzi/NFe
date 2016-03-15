using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GoLive.Modulo.Consultas.Utils
{
    public class base64Image
    {
        public static string ImageTobase64(byte[] imagem)
        {
            return Convert.ToBase64String(imagem);
        }
        public static Image base64ToImage(string imagem)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(imagem);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image , based on @Crulex comment, the below line has no need since MemoryStream already initialized
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}
