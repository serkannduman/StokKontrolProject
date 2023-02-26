namespace StokKontrolProject.WebUI.Models
{
    public class Upload
    {
        public static string ImageUpload(List<IFormFile> files,IWebHostEnvironment env,out bool result)
        {
            result= false;
            //localhost:PortNo/Uploads
            var uploads = Path.Combine(env.WebRootPath, "Uploads");

            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))
                {
                    if(file.Length <= 4194304)
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split('/')[1]}"; //Split sonrası / karakterınden bölerek bir array oluşur. Oluşan array => {"image","jpeg"} gibidir. 1.indexteki bizim için uzantıdır
                        
                        var filePath = Path.Combine(uploads,uniqueName);

                        using (var fileStream =new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            return filePath.Substring(filePath.IndexOf("\\Uploads\\"));
                        }
                    }
                    else
                    {
                        return "Boyut 4MB'dan büyük olamaz";
                    }
                }
                else
                {
                    return "Lütfen resim formatında bir dosya seçiniz";
                }
            }

            return "Dosya Seçilmedi";
        }
    }
}
