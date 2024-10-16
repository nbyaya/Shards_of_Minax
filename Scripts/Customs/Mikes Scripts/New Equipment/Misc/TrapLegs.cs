using System;
using Server;
using Server.Items;
using Server.Mobiles;

public class TrapLegs : LeatherLegs // Inherits from LeatherLegs or BaseArmor
{
    public override int LabelNumber { get { return 1061592; } } // Label number for identification, change as needed

    [Constructable]
    public TrapLegs() : base()
    {
        Name = "Legs of Trap Finding"; // Name your item
        // Optionally, set some base properties, like Weight, if you want them to differ from standard leather legs
    }

    public override void AddNameProperties(ObjectPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add("Bonus to DEX from TrapFinding");
    }

    public override void OnAdded(object parent)
    {
        base.OnAdded(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value;

            int dexBonus = (int)(trapFindingSkill / 2); // Adjust the division value to control how much DEX is gained per skill level

            wearer.RawDex += dexBonus; // Adjust dexterity
        }
    }

    public override void OnRemoved(object parent)
    {
        base.OnRemoved(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value; // Replace with actual Trap Finding skill

            int dexBonus = (int)(trapFindingSkill / 2); // Adjust the division value to control how much DEX is gained per skill level

            wearer.RawDex -= dexBonus; // Reverse the DEX bonus
        }
    }

    public TrapLegs(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);

        int version = reader.ReadInt();
    }
}
