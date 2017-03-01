using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwatchRename
{
    class Program
    {
        static Regex _stripLead = new Regex(@"^\w?");
        static Regex _stripName = new Regex(@"^([fb]|alt)\d{0,2}$");

        static void Main(string[] args)
        {
            try
            {
                CheckCopyFolder();
            }
            catch
            {
                Console.Out.WriteLine("You don't have enough permissions to make a folder here. Copy this somewhere local?");
                Console.ReadKey();
                return;
            }

            try
            {
                DoYerThing();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            Console.ReadKey();
        }

        static void CheckCopyFolder()
        {
            if (!Directory.Exists(".\\img_copy\\"))
                Directory.CreateDirectory(".\\img_copy\\");
        }

        static void DoYerThing()
        {
            var currentPics = Directory.EnumerateFiles(".\\", "*.tif");
            var existingPics = Directory.EnumerateFiles(".\\img_copy\\", "*.tif").ToList();
            Console.Out.Write("A");

            foreach (var pic in currentPics)
            {
                var newPicName = ".\\img_copy\\" + RenamePic(pic.Replace(".\\", string.Empty));

                if (existingPics.Contains(newPicName))
                    continue;

                if (File.Exists(newPicName))
                {
                    var i = 1;
                    var extendedName = string.Format("{0}({1}).tif", newPicName.Replace(".tif", string.Empty), i++);
                    while (File.Exists(extendedName))
                        extendedName = string.Format("{0}({1}).tif", newPicName.Replace(".tif", string.Empty), i++);

                    newPicName = extendedName;
                }

                File.Copy(".\\" + pic, newPicName);
                Console.Out.Write("a");
            }

            Console.Write("nd done.");
        }

        static string RenamePic(string picName)
        {
            var picNameHolder = picName.Replace(".tif", string.Empty);
            var picNameParts = picNameHolder.Split('_');
            var addTertiary = false;

            picNameHolder = string.Format("{0}_{1}{2}_swatch.tif", picNameParts[0], picNameParts[1],
                addTertiary ? "_" + picNameParts[2] : string.Empty);

            return picNameHolder;
        }
    }
}
