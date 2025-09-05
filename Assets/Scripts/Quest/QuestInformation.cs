public class QuestInformation
{
    public string questName { get; private set; }
    public string startDescription { get; private set; }
    public string inprogressDescription { get; private set; }
    public string completeDescription { get; private set; }
    public QuestInformation(string _name, string  _startDescription,string _inprogressDescription, string _completeDescription)
    {
        questName = _name;
        startDescription = _startDescription;
        inprogressDescription = _inprogressDescription;
        completeDescription = _completeDescription;
    }
}
