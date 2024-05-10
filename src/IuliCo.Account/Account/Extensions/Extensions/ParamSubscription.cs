using System;
using System.Reflection;
using System.Threading;
using System.Threading.Generic;
using IuliCo.Core;
using IuliCo.Core.Enums;

internal sealed class ParamSubscription<T> : Subscription
{
    private T? _param;
    private TimerRule<T>? _TimerRule;

    public ParamSubscription(TimerRule<T> timerRule, T gparam)
    {
        this._TimerRule = timerRule;
        this._param = gparam;
    }

    internal override void Invoke()
    {
        if (this._TimerRule != null)
        {
            if (_param != null)
            {
                this._TimerRule._Action!(this._param, (int)DateTime.Now.Ticks);
                if (this._param != null && this._TimerRule._Active)
                    AsyncLogger.Instance.LogAsync(LogLevel.Info, this._param.ToString() ?? "No param was found for timer rule").ConfigureAwait(false);
            }
            if (this._TimerRule != null)
            {
                if (!this._TimerRule._Active)
                {
                    ((IDisposable)this).Dispose();
                }
                else
                {
                    base.AddMilliseconds(this._TimerRule._period);
                }
            }
        }
    }

    internal override void Dispose()
    {
        this._TimerRule = null;
        this._param = default;
    }

    internal override MethodInfo GetMethod()
    {
        return this._TimerRule!._Action!.Method;
    }

    internal override ThreadPriority GetThreadPriority()
    {
        return this._TimerRule!._ThreadPriority;
    }
}

