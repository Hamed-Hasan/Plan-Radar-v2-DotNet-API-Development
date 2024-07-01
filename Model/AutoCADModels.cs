using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoCADApi.Models
{
    public class AutoCADFile
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string Urn { get; set; } = string.Empty;
        public ICollection<Pin> Pins { get; set; } = new List<Pin>();
    }

    public class ImageFile
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string Urn { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Pin> Pins { get; set; } = new List<Pin>();
    }

    public class UploadFile
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
    }


    public class Pin
    {
        [Key]
        public int Id { get; set; }
        public int? AutoCADFileId { get; set; }
        public AutoCADFile? AutoCADFile { get; set; }
        public int? ImageFileId { get; set; }
        public ImageFile? ImageFile { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public byte[] AudioClip { get; set; } = Array.Empty<byte>();
        public byte[] VideoClip { get; set; } = Array.Empty<byte>();
        public UploadFile? UploadFile { get; set; }
        public ModalContent? ModalContent { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class ModalContent
    {
        [Key]
        public int Id { get; set; }
        public int PinId { get; set; }
        public string AdditionalInfo { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
