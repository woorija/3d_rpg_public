using System.Text;

public class CSVReader
{
    static StringBuilder sb = new StringBuilder();
    public static string GetStringData(string _data)
    {
        sb.Clear();
        sb.Append(_data);
        sb.Replace("<br>", "\n"); //줄바꿈 치환
        sb.Replace("<c>", ","); // ,치환
        return sb.ToString();
    }

    public static int GetIntData(string _data)
    {
        int n;
        if (int.TryParse(_data, out n)) //int형변환
        {
            return n;
        }
        return 0;
    }

    public static float GetFloatData(string _data)
    {
        float f;
        if (float.TryParse(_data, out f)) //int형변환
        {
            return f;
        }
        return 0;
    }
    public static bool GetBoolData(string _data)
    {
        bool b;
        if (bool.TryParse(_data, out b)) //int형변환
        {
            return b;
        }
        return false;
    }

    public static long GetLongData(string _data)
    {
        long l;
        if(long.TryParse(_data, out l))
        {
            return l;
        }
        return 0;
    }
}