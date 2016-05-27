using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Helpers
{
    public enum ResultEnum : byte
    {
        ILLEGAL_CALL,
        OK,
        ERROR,
        OFFLINE,
        TIMED_OUT,
        CANCELED,
    };

    public class Result
    {
        public static Result OK { get { return new Result(ResultEnum.OK); } }
        public static Result ERROR { get { return new Result(ResultEnum.ERROR); } }
        public static Result CANCELED { get { return new Result(ResultEnum.CANCELED); } }
        public static Result TIMED_OUT { get { return new Result(ResultEnum.TIMED_OUT); } }
        public static Result ILLEGAL_CALL { get { return new Result(ResultEnum.ILLEGAL_CALL); } }

        public Exception Exception { get; set; }
        public ResultEnum Value { get; set; }
        public bool IsOK { get { return Value == ResultEnum.OK; } }

        public Result(ResultEnum resultEnum)
        {
            this.Value = resultEnum;
        }

        public Result(System.Exception ex)
        {
            this.Value = ResultEnum.ERROR;
            this.Exception = ex;
        }

        public void SynthesizeResultAndThrowOnError(Result other)
        {
            if (IsOK)
                if (other.IsOK)
                    return;
                else
                {
                    this.Value = other.Value;
                    this.Exception = other.Exception;
                }

            throw new ResultException();
        }
    }
}
