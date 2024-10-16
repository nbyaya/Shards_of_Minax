using System;
using Server;
using Server.Items;
using Server.Mobiles;

public class TrapSleeves : LeatherArms // Inherits from LeatherSleeves or BaseArmor
{
    public override int LabelNumber { get { return 1061592; } } // Label number for identification, change as needed

    [Constructable]
    public TrapSleeves() : base()
    {
        Name = "Sleeves of Trap Finding"; // Name your item
        // Optionally, set some base properties, like Weight, if you want them to differ from standard leather sleeves
    }

    public override void AddNameProperties(ObjectPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add("Bonus to INT from Trap Finding");
    }

    public override void OnAdded(object parent)
    {
        base.OnAdded(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value;

            int intBonus = (int)(trapFindingSkill / 2); // Adjust the division value to control how much skill affects the bonus

            wearer.RawInt += intBonus; // Adjust intelligence
        }
    }

    public override void OnRemoved(object parent)
    {
        base.OnRemoved(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value; // Replace with actual Trap Finding skill

            int intBonus = (int)(trapFindingSkill / 2); // Adjust the division value to control how much skill affects the bonus

            wearer.RawInt -= intBonus; // Reverse the INT bonus
        }
    }

    public TrapSleeves(Serial serial) : base(serial)
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
