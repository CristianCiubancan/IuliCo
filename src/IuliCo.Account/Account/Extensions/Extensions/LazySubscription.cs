﻿using System;
using System.Reflection;
using System.Threading;

internal sealed class LazySubscription : Subscription
{
    private TimerRule? _TimerRule;

    public LazySubscription(TimerRule timerRule)
    {
        this._TimerRule = timerRule;
    }

    internal override void Invoke()
    {
        if (this._TimerRule != null)
        {
            this._TimerRule._action!((int)DateTime.Now.Ticks);
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
    }

    internal override MethodInfo GetMethod()
    {
        if (this._TimerRule == null)
        {
            throw new NullReferenceException();
        }
        return this._TimerRule._action!.Method;
    }

    internal override ThreadPriority GetThreadPriority()
    {
        if (this._TimerRule == null)
        {
            throw new NullReferenceException();
        }
        return this._TimerRule._ThreadPriority;
    }
}

