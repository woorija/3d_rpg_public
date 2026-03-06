public interface ICsvToCodeGenerator
{
    string csvPath { get; }
    string outputScriptPath { get; }
    void ReadCSV();
    void GenerateClassCode();
}
