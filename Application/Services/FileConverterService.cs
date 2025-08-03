using System.Drawing;

namespace Application;

public class FileConverterService
{
    public static string PlaceHolder = "data:image;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAABmJLR0QA/wD/AP+gvaeTAAAEJElEQVR4nO3dv49MURTA8e8Ku4kIiUKCRKHRaFQqlYqCjdiQkCgp7Cr5Eyh1dHTb6VSikbAaK1HQiFixiSAhg8KuXcWbiTFz38977jtnZs43ec3uvDt35pP33sydNcDzPM/z5JoDvgAfgVnluUx8U8BnYLO7rQHnVGfk8Yl/II5ioDNkCI5iqLMMo6wDFzUnNek5isEcxWCOYjBHMZijKLUJvAb2Bn4XQvEtbiutd0NHMQbiKAZBHMUgSFF+oa9fUhBwlLolBwFHqVMrIOAoVWsNBBylSq2CgKOU1ToIOEpRKiDgKHmpgYCjhFIFAUcZTB0EHKU/EyDgKL3MgED2p0RW/8RoBlgAloAf3W0JmO/+TipTIGATZT/wkuHH3tuWu7eRyBwI2EKZoRijH0XiSDEJAnZQFijH6G1XBe7PLAjYQHlOdZBnAvdnGgT0X311qA7SEbg/8yCgi1IH5LvA/Y0ECOidvvyUVZAGyjzVQcb6oj4F7Az8vG2UGbKXtGUYy8C0wP2ZBNkC3AFeAXsCv28bZT/FKGP9xnAbsNg3vhWUabJT0jOyC30HeNr9mcSR0csUyHbgYeA+Hufc3sL7FOnMgOwCngTGXwUOF+yn/T5FOhMge4AXgbHfAgcr7D9OKOogB4A3gXFfAftqjDMuKKogh4D3gTGXgN0NxhsHFDWQIwx/CcEm8AjYETHuqKOogBwDvgXGeoDMZwqjjNI6yEngZ2Cce8DWBuPlNaoorYKcB34HxrhFtlQineT7lFngQ3dL+S1IrYFcBv4E9r9ZY4wmSRwpcwNjpDzSWgG5DmwM7LcBXKs52abFoAxipEZJCjJFdgQM7rMGXGo238Y1QcnDSImSDGQLcDdw+19kF3aN6lxT8m6beu0sCcg0/6/Y9rYOcDxuvtFVOVJCR8Y6cKHi/jGJg+St2H4FjsbPV6SiJ7UIo8r+sYmC5K3YrpAtk1gq9pSUaulfDCR2xVajsi83GDwyquwfe6SIgEit2GqUh1KGUbR/DEo0SN6K7XOardhqFPukSqJEg4RWbB+SXdxHqdhrgtQ1JRpkcFtE9kP/VIUWMi2giILcR3bFNlVzZNe3qt9i1ObpSwzkNmlWbKXrf59R56ul2kKJBtkAblScqHahU8pLbJ2+okGuVJygdrPUf4I0UKJBRqUPNHti20aZSJC6r3zaRJkYkBNkKCs0+wi2LZSJAZGojVdfDlKz1CgO0qCUKA7SsFTXFAeJKMWR4iCRpUYpzUGGS4lSmoOES4VSmoPkJ3GhdxDh1D8P8YZT/TzEC9cUJRrEt7T/qU1p2g/e6pYKpTTtB255S4FSmvaDtr5Jo3gJG9V/iDrWOYrBHMVgjmIwRzGYoxgsb5X4tOakJr0QyorqjLwhlFXd6XiQnabekR0dp5Tn4nme541RfwFbbDokN3PzagAAAABJRU5ErkJggg==";
    public static string ConvertToBase64(Stream file, int w = 256)
    {
        if (file.Length > 0)
        {
            var ms = new MemoryStream();
            file.CopyTo(ms);

            ms = ResizeImage(ms, w);

            var fileBytes = ms.ToArray();
            return "data:image;base64," + Convert.ToBase64String(fileBytes);
        }
        else
        {
            throw new FileLoadException();
        }
    }

        public static MemoryStream ResizeImage(MemoryStream ms, int w)
        {
    #if WINDOWS
            Image img = Image.FromStream(ms);
            int h = Convert.ToInt32(w * img.Height / img.Width);
            Bitmap imgN = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(imgN))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, w, h);
            }
            MemoryStream res = new MemoryStream();
            imgN.Save(res, img.RawFormat);
            res.Position = 0;
            return res;
    #else
            throw new PlatformNotSupportedException("Image resizing is only supported on Windows platforms.");
    #endif
        }
}