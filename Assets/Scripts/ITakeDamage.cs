
using System;

public interface ITakeDamage
{
    int Health { get; set; }

    void SetInitialHealth(int initialHealth);

    void TakeDamage();

    void Death();

}
