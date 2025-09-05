using System;
using System.IO;

public abstract class BaseSaveData : ISaveData, IDisposable
{
    protected byte[] buffer;
    protected readonly MemoryStream stream;
    protected FileStream fileStream;
    protected readonly BinaryWriter writer;
    protected readonly BinaryReader reader;
    protected readonly string path;
    protected int dataVersion;
    public BaseSaveData(string _path, int _bufferSize)
    {
        path = _path;
        buffer = new byte[_bufferSize];
        stream = new MemoryStream(buffer, true);
        writer = new BinaryWriter(stream);
        reader = new BinaryReader(stream);
    }
    protected void Padding()
    {
        while(stream.Position % 4 != 0)
        {
            writer.Write((byte)0);
        }
    }
    
    public virtual void LoadData()
    {
        ResetStream();
        if (fileStream == null)
        {
            fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.WriteThrough);
        }
        fileStream.Read(buffer, 0, buffer.Length);
        EncryptionUtility.DecryptXXTEA(buffer, (int)fileStream.Length);
    }

    public virtual void SaveData()
    {
        if (fileStream == null)
        {
            fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.WriteThrough);
        }
        EncryptionUtility.EncryptXXTEA(buffer, (int)stream.Position);
        fileStream.Seek(0, SeekOrigin.Begin);
        fileStream.Write(buffer, 0, (int)stream.Position);
        fileStream.SetLength(stream.Position);
        fileStream.Flush();
    }
    protected void ResetStream()
    {
        stream.Position = 0;
    }
    public void Dispose()
    {
        writer?.Dispose();
        reader?.Dispose();
        stream?.Dispose();
        fileStream?.Dispose();
    }
}
