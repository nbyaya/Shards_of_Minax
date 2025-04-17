using System;
using Server;
using Server.Items;
using Server.Mobiles;

public class TrapGorget : LeatherGorget // Inherits from LeatherGorget or BaseArmor
{
    public override int LabelNumber { get { return 1061591; } } // Label number for identification, change as needed

    [Constructable]
    public TrapGorget() : base()
    {
        Name = "Gorget of Trap Finding"; // Name your item
        // Optionally, set some base properties, like Weight, if you want them to differ from standard leather gorget
    }

    public override void AddNameProperties(ObjectPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add("Bonus to Physical and Poison Resist from Trap Finding");
    }

    public override void OnAdded(object parent)
    {
        base.OnAdded(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value;

            int resistBonus = (int)(trapFindingSkill / 10);

            this.PhysicalBonus += resistBonus;
            this.PoisonBonus += resistBonus;
        }
    }

    public override void OnRemoved(object parent)
    {
        base.OnRemoved(parent);

        if (parent is Mobile)
        {
            Mobile wearer = (Mobile)parent;
            double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value;

            int resistBonus = (int)(trapFindingSkill / 10);

            this.PhysicalBonus -= resistBonus;
            this.PoisonBonus -= resistBonus;
        }
    }

    public TrapGorget(Serial serial) : base(serial)
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
