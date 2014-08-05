using System;
using System.IO;

namespace UsedParts.Common
{
    public interface IFileStorageManager
    {
        void SaveFile ( Stream source, string localPath );
        void SaveFile ( string content, string localPath );
        void SaveFile ( byte[] buffer, string localPath );
        void CopyFile ( string sourcePath, string destPath );
        bool IsExists ( string localPath );
        void DeleteFile ( string localPath );
        string ReadFileAsString ( string localPath );
        byte[] ReadFile ( string localPath );
        Stream OpenFile ( string localPath );
        Stream CreateFile ( string localPath );
        DateTimeOffset GetLastUpdateTime ( string localPath );
    }
}
