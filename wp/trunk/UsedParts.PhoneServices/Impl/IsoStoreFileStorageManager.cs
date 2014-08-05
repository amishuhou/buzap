using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using UsedParts.Common;

namespace UsedParts.PhoneServices.Impl
{
    public class IsoStoreFileStorageManager : IFileStorageManager
    {
        private readonly IsolatedStorageFile _isoStore = IsolatedStorageFile.GetUserStoreForApplication();

        public void SaveFile ( string content, string localPath )
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                SaveFile(ms, localPath);
            }
        }

        public void SaveFile ( byte[] buffer, string localPath )
        {
            using (var ms = new MemoryStream(buffer))
            {
                SaveFile(ms, localPath);
            }
        }

        public void CopyFile ( string sourcePath, string destPath )
        {
            try
            {
                CreateDirectoryIfRequired(destPath);
                if (!_isoStore.FileExists(sourcePath)) 
                    return;
                if (_isoStore.FileExists(destPath))
                    _isoStore.DeleteFile(destPath);
                _isoStore.CopyFile(sourcePath, destPath);
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't copy from {0} to {1}", sourcePath, destPath), isex);
            }
        }

        public void SaveFile ( Stream source, string localPath )
        {
            try
            {
                CreateDirectoryIfRequired(localPath);
                if (_isoStore.FileExists(localPath))
                    _isoStore.DeleteFile(localPath);

                using (var sw = new BinaryWriter(CreateFile(localPath)))
                {
                    source.Seek(0, SeekOrigin.Begin);
                    var sr = new BinaryReader(source);
                    var content = sr.ReadBytes((int)source.Length);
                    sw.Write(content);
                    source.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't save {0}", localPath), isex);
            }
        }

        public Stream CreateFile ( string localPath )
        {
            try
            {
                CreateDirectoryIfRequired(localPath);
                if (_isoStore.FileExists(localPath))
                    _isoStore.DeleteFile(localPath);
                return _isoStore.CreateFile(localPath);

            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't save {0}", localPath), isex);
            }
        }

        public bool IsExists ( string filePath )
        {
            return _isoStore.FileExists(filePath);
        }

        public void DeleteFile ( string localPath )
        {
            try
            {
                if (IsExists(localPath))
                    _isoStore.DeleteFile(localPath);
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't save {0}", localPath), isex);
            }
        }

        public string ReadFileAsString ( string localPath )
        {
            try
            {
                using (var sr = new StreamReader(new IsolatedStorageFileStream(
                    localPath, FileMode.Open, _isoStore)))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't read content {0}", localPath), isex);
            }
        }

        public byte[] ReadFile ( string localPath )
        {
            try
            {
                using (var sr = new IsolatedStorageFileStream(
                    localPath, FileMode.Open, _isoStore))
                {
                    var buffer = new byte[sr.Length];
                    sr.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't read content {0}", localPath), isex);
            }
        }

        public Stream OpenFile ( string localPath )
        {
            try
            {
                return new IsolatedStorageFileStream(
                    localPath, FileMode.Open, _isoStore);
            }
            catch (IsolatedStorageException isex)
            {
                throw new FileStorageException(string.Format("Can't open {0}", localPath), isex);
            }
        }

        public DateTimeOffset GetLastUpdateTime ( string filePath )
        {
            return _isoStore.GetLastWriteTime(filePath);
        }

        private void CreateDirectoryIfRequired ( string filePath )
        {
            var parts = filePath.Split("/".ToCharArray());
            if (parts.Length == 0)
                return;
            var directoryPath = string.Empty;
            for (var i = 0; i < parts.Length - 1; i++)
            {
                directoryPath += "/" + parts[i];
                directoryPath = directoryPath.Replace("//", "/");
                if (_isoStore.DirectoryExists(directoryPath))
                    continue;
                _isoStore.CreateDirectory(directoryPath);
            }
        }


    }
}
