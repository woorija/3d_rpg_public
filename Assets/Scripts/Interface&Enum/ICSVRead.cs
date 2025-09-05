using Cysharp.Threading.Tasks;

public interface ICSVRead
{
    UniTask ReadCSV();
    /* CSVReader 클래스로 CSV파일을 라인 단위로 나누고
     * 나눈 데이터를 타입을 변환하여 제네릭 컬렉션에 담는 함수입니다.
     * 
     * 
     * 아래는 예시 코드입니다.
     * 
     *  string[] lines = CSVReader.Line_Split(".csv를 제외한 파일명");
     *  
     *  for (var i = 1; i < lines.Length; i++)
        {

            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;
            
            Dictionary.Add(CSVReader.GetIntData(values[0]),new Dictionary( CSVReader.GetBoolData(values[1]))l

        }
     */
}
