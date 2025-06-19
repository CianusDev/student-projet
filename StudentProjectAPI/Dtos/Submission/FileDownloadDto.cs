
namespace StudentProjectAPI.Dtos.Submission
{


    // DTO pour téléchargement de fichier
    public class FileDownloadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }
}
