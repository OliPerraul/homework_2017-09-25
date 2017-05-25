using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;

public class MoonSharpUtils
{
    public static bool IsTable(DynValue table)
    {
        switch (table.Type)
        {
            case DataType.Table:
                return true;

            case DataType.Boolean:
            case DataType.ClrFunction:
            case DataType.Function:
            case DataType.Nil:
            case DataType.Number:
            case DataType.String:
            case DataType.TailCallRequest:
            case DataType.Thread:
            case DataType.Tuple:
            case DataType.UserData:
            case DataType.Void:
            case DataType.YieldRequest:
                break;
        }
        return false;
    }

    public static bool IsFunction(DynValue function)
    {
        switch (function.Type)
        {
            case DataType.Function:
                return true;
            case DataType.Table:
            case DataType.Boolean:
            case DataType.ClrFunction:
            case DataType.Nil:
            case DataType.Number:
            case DataType.String:
            case DataType.TailCallRequest:
            case DataType.Thread:
            case DataType.Tuple:
            case DataType.UserData:
            case DataType.Void:
            case DataType.YieldRequest:
                break;
        }
        return false;
    }


    public static bool IsNumber(DynValue number)
    {
        switch (number.Type)
        {
            case DataType.Number:
                return true;
            case DataType.Table:
            case DataType.Boolean:
            case DataType.ClrFunction:
            case DataType.Function:
            case DataType.Nil:
            case DataType.String:
            case DataType.TailCallRequest:
            case DataType.Thread:
            case DataType.Tuple:
            case DataType.UserData:
            case DataType.Void:
            case DataType.YieldRequest:
                break;
        }
        return false;
    }

    public static bool IsString(DynValue str)
    {
        switch (str.Type)
        {
            case DataType.String:
                return true;
            case DataType.Table:
            case DataType.Boolean:
            case DataType.ClrFunction:
            case DataType.Function:
            case DataType.Nil:
            case DataType.Number:
            case DataType.TailCallRequest:
            case DataType.Thread:
            case DataType.Tuple:
            case DataType.UserData:
            case DataType.Void:
            case DataType.YieldRequest:
                break;
        }
        return false;
    }



}
