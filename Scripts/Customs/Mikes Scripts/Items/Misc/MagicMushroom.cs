using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

public class MagicMushroom : Item
{
    [Constructable]
    public MagicMushroom() : base(0x0D16) // Item ID for mushroom, you can change this to the desired item ID
    {
        Name = "Magic Mushroom";
        Weight = 1.0;
        Hue = 1153; // Hue value for blue color, you can change this to the desired hue
    }

    public MagicMushroom(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001); // "That must be in your pack for you to use it."
        }
        else if (from.Skills[SkillName.Camping].Value < 50.0)
        {
            from.SendMessage("You need at least 50 camping skill to use this.");
        }
        else if (from.BeginAction(typeof(MagicMushroom)) || from.FindItemOnLayer(Layer.OneHanded) is MagicMushroom)
        {
            from.SendMessage("You need to wait 60 seconds before using another Magic Mushroom.");
        }
        else
        {
            from.Mana += 50;
            from.SendMessage("You restore 50 mana.");

            if (from.Mana > from.ManaMax)
            {
                from.Mana = from.ManaMax;
            }

            from.BeginAction(typeof(MagicMushroom));
            Timer.DelayCall(TimeSpan.FromSeconds(60.0), new TimerStateCallback(EndAction_Callback), from);

            Consume();
        }
    }

    private static void EndAction_Callback(object state)
    {
        Mobile from = (Mobile)state;
        from.EndAction(typeof(MagicMushroom));
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);

        int version = reader.ReadInt();
    }
}
