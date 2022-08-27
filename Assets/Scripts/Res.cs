using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Res : IComparable<Res>
{
    public float pop;
    public float food;
    public float wood;
    public float stone;
    public float coin;

    readonly float[] res;

    public Res(float pop, float food, float wood, float stone, float coin)
    {
        res = new float[] { pop, food, wood, stone, coin };
        this.pop = pop;
        this.food = food;
        this.wood = wood;
        this.stone = stone;
        this.coin = coin;
    }

    public static readonly Res zero = new Res(0, 0, 0, 0, 0);

    // public static implicit operator Res(int x) => new Res(x, x, x, x, x);
    // public static implicit operator Res(float x) => new Res(x, x, x, x, x);

    public static Res operator +(Res a) => a;
    public static Res operator -(Res a) => new Res(-a.pop, -a.food, -a.wood, -a.stone, -a.coin);
    public static Res operator +(Res a, Res b)
        => new Res(a.pop + b.pop, a.food + b.food, a.wood + b.wood, a.stone + b.stone, a.coin + b.coin);
    public static Res operator -(Res a, Res b)
        => a + (-b);
    public static Res operator *(Res a, float b)
        => new Res(a.pop * b, a.food * b, a.wood * b, a.stone * b, a.coin * b);
    public static Res operator *(float b, Res a)
        => a * b;
    public static Res operator *(Res a, int b)
        => new Res(a.pop * b, a.food * b, a.wood * b, a.stone * b, a.coin * b);
    public static Res operator *(int b, Res a)
        => a * b;
    public static Res operator /(Res a, float b)
        => new Res(a.pop / b, a.food / b, a.wood / b, a.stone / b, a.coin / b);
    public static Res operator /(Res a, int b)
        => new Res(a.pop / b, a.food / b, a.wood / b, a.stone / b, a.coin / b);

    public int CompareTo(Res other)
    {
        if (other == null)
            return 1;

        Res dif = this - other;
        if (dif == zero)
            return 0;
        else if (dif.pop >= 0 && dif.food >= 0 && dif.wood >= 0 && dif.stone >= 0 && dif.coin >= 0)
            return 1;
        else
            return -1;
    }

    public static bool operator >(Res a, Res b)
    {
        return a.CompareTo(b) == 1;
    }

    public static bool operator <(Res a, Res b)
    {
        return a.CompareTo(b) == -1;
    }

    public static bool operator >=(Res a, Res b)
    {
        return a.CompareTo(b) >= 0;
    }

    public static bool operator <=(Res a, Res b)
    {
        return a.CompareTo(b) <= 0;
    }

    public override string ToString()
    {
        string output = "";
        if (pop != 0)
            output += ToString(0);
        if (food != 0)
            output += ToString(1);
        if (wood != 0)
            output += ToString(2);
        if (stone != 0)
            output += ToString(3);
        if (coin != 0)
            output += ToString(4);

        if (output == "")
            return "None";
        else
            return output;
    }

    public string ToString(int i)
    {
        return "<sprite=" + i + ">" + res[i].ToString();
    }

    public string ToColouredString()
    {
        string output = "";
        if (pop != 0)
            output += ToColouredString(0);
        if (food != 0)
            output += ToColouredString(1);
        if (wood != 0)
            output += ToColouredString(2);
        if (stone != 0)
            output += ToColouredString(3);
        if (coin != 0)
            output += ToColouredString(4);

        if (output == "")
            return "None";
        else
            return output;
    }

    public string ToColouredString(int i)
    {
        return res[i] >= 0 ? "<sprite=" + i + "><color=green>" + res[i].ToString("+#;-#;0") + "</color>" : "<sprite=" + i + "><color=red>" + res[i].ToString("+#;-#;0") + "</color>";
    }

    public Res Pop()
    {
        return new Res(pop, 0, 0, 0, 0);
    }

    public Res Food()
    {
        return new Res(0, food, 0, 0, 0);
    }

    public Res Wood()
    {
        return new Res(0, 0, wood, 0, 0);
    }
    public Res Stone()
    {
        return new Res(0, 0, 0, stone, 0);
    }

    public Res Coin()
    {
        return new Res(0, 0, 0, 0, coin);
    }
}
