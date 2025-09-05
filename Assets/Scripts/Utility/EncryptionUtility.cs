
public static class EncryptionUtility
{

    private const uint DELTA = 0x9E3779B9;
    private static readonly uint[] XXTEAKey = new uint[4] { 0x510E527F, 0xF3BCC908, 0x5A827999, 0xFE94F82B };

    public static void EncryptXXTEA(byte[] _buffer, int _length)
    {
        int n = (_length / 4) - 1;
        if (n < 1) return; 
        uint z = ReadUInt32(_buffer, n * 4);
        uint y;
        uint sum = 0;
        uint e;
        int p;
        int q = 6 + 52 / (n + 1);

        unchecked
        {
            while (q-- > 0)
            {
                sum += DELTA;
                e = (sum >> 2) & 3;

                for (p = 0; p < n; p++)
                {
                    y = ReadUInt32(_buffer, (p + 1) * 4);
                    z = ReadUInt32(_buffer, p * 4) + MX(sum, y, z, p, e);
                    WriteUInt32(_buffer, p * 4, z);
                }
                y = ReadUInt32(_buffer, 0);
                z = ReadUInt32(_buffer, n * 4) + MX(sum, y, z, p, e);
                WriteUInt32(_buffer, n * 4, z);
            }
        }
    }

    public static void DecryptXXTEA(byte[] _buffer, int _length)
    {
        int n = (_length / 4) - 1;
        if (n < 1) return;
        uint z;
        uint y = ReadUInt32(_buffer, 0);
        uint sum;
        uint e;
        int p;
        int q = 6 + 52 / (n + 1);

        unchecked
        {
            sum = (uint)(q * DELTA);
            while (sum != 0)
            {
                e = (sum >> 2) & 3;
                for (p = n; p > 0; p--)
                {
                    z = ReadUInt32(_buffer, (p - 1) * 4);
                    y = ReadUInt32(_buffer, p * 4) - MX(sum, y, z, p, e);
                    WriteUInt32(_buffer, p * 4, y);
                }
                z = ReadUInt32(_buffer, n * 4);
                y = ReadUInt32(_buffer, 0) - MX(sum, y, z, p, e);
                WriteUInt32(_buffer, 0, y);
                sum -= DELTA;
            }
        }
    }
    private static uint MX(uint sum, uint y, uint z, int p, uint e)
    {
        return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (XXTEAKey[p & 3 ^ e] ^ z);
    }
    private static uint ReadUInt32(byte[] _buffer, int _offset)
    {
        return (uint)(_buffer[_offset])
            | (uint)(_buffer[_offset + 1]) << 8
            | (uint)(_buffer[_offset + 2]) << 16
            | (uint)(_buffer[_offset + 3]) << 24;
    }
    private static void WriteUInt32(byte[] _buffer, int _offset, uint _value)
    {
        _buffer[_offset] = (byte)(_value & 0xFF);
        _buffer[_offset + 1] = (byte)(_value >> 8 & 0xFF);
        _buffer[_offset + 2] = (byte)(_value >> 16 & 0xFF);
        _buffer[_offset + 3] = (byte)(_value >> 24 & 0xFF);
    }
}