public class HitResponse
{
    public Code code = Code.Ok;

    public bool isOk()
    {
        return code == Code.Ok;
    }

    public enum Code
    {
        Ok,
    }
}
