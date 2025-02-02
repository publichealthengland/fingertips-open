﻿using FingertipsUploadService.ProfileData.Entities.Job;
using System.IO;

namespace FingertipsUploadService.FpmFileReader
{
    public class FileReaderFactory
    {
        public IUploadFileReader Get(string filePath)
        {
            IUploadFileReader fileReader;
            if (IsCsv(filePath))
            {
                fileReader = new CsvFileReader(filePath);
            }
            else
            {
                fileReader = new ExcelFileReader(filePath);
            }
            return fileReader;
        }


        private static bool IsCsv(string dataFilePath)
        {
            var fileExt = Path.GetExtension(dataFilePath);
            return fileExt != null && fileExt.ToLower() == ".csv";
        }
    }
}