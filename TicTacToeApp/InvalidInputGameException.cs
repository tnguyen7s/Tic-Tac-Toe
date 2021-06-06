using System;
class InvalidInputGameException:Exception{
    public InvalidInputGameException(string msg):base(msg)
    {
    }
}