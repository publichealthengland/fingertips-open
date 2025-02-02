﻿using System;

namespace Fpm.ProfileData.Entities.Profile
{
    public class Document
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string FileName { get; set; }  
        public byte[] FileData { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
    } 
}
