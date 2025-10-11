namespace Todo.Common;

//
// Ok or Error. And need to accomodate return datao
//

public class Result
{
    private bool ok;

    private string error;

    private Result()
    {
        this.ok = true;
        this.error = string.Empty;
    }

    private Result(string error)
    {
        this.ok = false;
        this.error = error;
    }

    public static TR Evaluate<TR>(Func<Result> f, Func<string, TR> onErr, Func<TR> onOk)
    {
        var r = f();
        if (r.IsErr())
        {
            return onErr(r.error);
        }
        return onOk();
    }

    public bool IsErr()
    {
        if (this.ok)
        {
            return false;
        }
        return true;
    }

    public bool IsOk()
    {
        if (!this.ok)
        {
            return false;
        }
        return true;
    }

    public string GetErr()
    {
        return this.error;
    }

    public static Result Ok()
    {
        return new Result();
    }

    public static Result Err(string error)
    {
        return new Result(error);
    }
}

public class Result<T>
    where T : class
{
    private bool ok;

    private string error;

    private T? value;

    public bool IsErr()
    {
        if (this.ok)
        {
            return false;
        }
        return true;
    }

    public bool IsOk()
    {
        if (!this.ok)
        {
            return false;
        }
        return true;
    }

    public string GetErr()
    {
        return this.error;
    }

    public T? GetVal()
    {
        return this.value;
    }

    private Result(T val)
    {
        this.value = val;
        this.ok = true;
        this.error = string.Empty;
    }

    private Result(string error)
    {
        this.value = null;
        this.ok = false;
        this.error = error;
    }

    public static Result<T> Ok(T val)
    {
        return new Result<T>(val);
    }

    public static Result<T> Err(string error)
    {
        return new Result<T>(error);
    }
}