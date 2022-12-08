namespace Common
{
    public class ThrowThisException
    {
        public ThrowThisException(Exception ExceptionType,string ErrorMsg)
        {
            ExceptionType.Data["Error"] = ErrorMsg;
            throw ExceptionType;
        }
    }
}
