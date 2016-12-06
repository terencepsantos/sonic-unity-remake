
using System;

public interface ITakeDamage
{
    int Health { get; set; }

    void SetInitialHealth();

    void TakeDamage();

    void Death();

}
