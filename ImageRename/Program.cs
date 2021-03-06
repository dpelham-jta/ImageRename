﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace ImageRename
{
    class Program
    {
        static Regex _stripName = new Regex(@"^\d+[a-zA-Z]?_\d+(_[a-zA-Z0-9]*)?");
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
            catch(Exception ex)
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
            var currentPics = Directory.EnumerateFiles(".\\", "*.jpg");
            var existingPics = Directory.EnumerateFiles(".\\img_copy\\", "*.jpg").ToList();
            Console.Out.Write("A");

            foreach (var pic in currentPics)
            {
                var renamedPic = RenamePic(pic.Replace(".\\", string.Empty));
                var newPicName = ".\\img_copy\\" + renamedPic;

                if (existingPics.Contains(newPicName))
                    continue;

                if(File.Exists(newPicName))
                {
                    var i = 'a';
                    var extendedName = string.Format("{0}{1}.jpg", newPicName.Replace(".jpg", string.Empty), i++);
                    while (File.Exists(extendedName))
                        extendedName = string.Format("{0}{1}.jpg", newPicName.Replace(".jpg", string.Empty), i++);

                    newPicName = extendedName;
                }

                File.Copy(".\\" + pic, newPicName);
                Console.Out.Write("a");
            }

            Console.Write("nd done.");
        }

        static string RenamePic(string picName)
        {
            var match = _stripName.Match(picName);
            return match.Value + ".jpg";
        }
    }
}
