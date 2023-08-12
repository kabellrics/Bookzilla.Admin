using Bookzilla.Admin.Core.Contracts.Services;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookzilla.Admin.Core.Services
{
    public class CoverExtractor : ICoverExtractor
    {
        private bool IsImgFile(string name)
        {
            var listimgfile = new List<string>() { ".jpg", ".jpeg", ".png" };
            return listimgfile.Contains(Path.GetExtension(name));
        }
        public string GetCoverStream(string path)
        {
            if (Path.GetExtension(path).ToLower() == ".cbr")
            {
                return GetCoverFromCBR(path);
            }
            else if (Path.GetExtension(path).ToLower() == ".cbz")
            {
                return GetCoverFromCBZ(path);
            }
            else
            {
                return null;
            }
        }
        private string GetCoverFromCBZ(string path)
        {
            try
            {
                using (ZipArchive zipArchive = ZipArchive.Open(path))
                {
                    var entry = zipArchive.Entries.OrderBy(x => x.Key).First(ent => !ent.IsDirectory && IsImgFile(ent.Key));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        entry.OpenEntryStream().CopyTo(ms);
                        var tmpcoverpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp", $"tmp{Path.GetExtension(entry.Key)}");
                        using (FileStream fileStream = new FileStream(tmpcoverpath, FileMode.OpenOrCreate))
                        {
                            //entry.OpenEntryStream().CopyTo(ms);
                            ms.WriteTo(fileStream);
                            fileStream.Close();
                            ms.Close();
                        }
                        return tmpcoverpath;// ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }
        private string GetCoverFromCBR(string path)
        {
            try
            {
                using (RarArchive rarArchive = RarArchive.Open(path))
                {
                    var entry = rarArchive.Entries.OrderBy(x => x.Key).First(ent => !ent.IsDirectory && IsImgFile(ent.Key));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        entry.OpenEntryStream().CopyTo(ms);
                        var tmpcoverpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bookzilla", "temp", $"tmp{Path.GetExtension(entry.Key)}");
                        using (FileStream fileStream = new FileStream(tmpcoverpath, FileMode.OpenOrCreate))
                        {
                            //entry.OpenEntryStream().CopyTo(ms);
                            ms.WriteTo(fileStream);
                            fileStream.Close();
                            ms.Close();
                        }
                        return tmpcoverpath;// ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }
    }
}
