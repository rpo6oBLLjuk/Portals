using System;

public interface IMechanism
{
    event Action Activate;
    event Action Deactivate;
}
