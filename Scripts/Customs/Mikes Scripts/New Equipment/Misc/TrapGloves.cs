using System;
using Server;
using Server.Items;
using Server.Mobiles;

public class TrapGloves : LeatherGloves // Inherits from LeatherGloves or BaseArmor
{
    public override int LabelNumber { get { return 1061591; } } // Label number for identification, change as needed
    
    [Constructable]
    public TrapGloves() : base()
    {
        Name = "Gloves of Trap Finding"; // Name your item
        // Optionally, set some base properties, like Weight, if you want them to differ from standard leather gloves
    }
	
	public override void AddNameProperties(ObjectPropertyList list)
	{
		base.AddNameProperties(list);
		list.Add("Bonus to STR from TrapFinding");
	}

	public override void OnAdded(object parent)
	{
		base.OnAdded(parent);

		if (parent is Mobile)
		{
			Mobile wearer = (Mobile)parent;
			double trapFindingSkill = wearer.Skills[SkillName.RemoveTrap].Value; 

			int hpBonus = (int)trapFindingSkill;

			// Assuming 1 STR = 2 HP for simplification, adjust based on your server's balance
			int strBonus = hpBonus / 2;

			wearer.RawStr += strBonus; // Adjust strength to give HP bonus


		}
	}

	public override void OnRemoved(object parent)
	{
		base.OnRemoved(parent);

		if (parent is Mobile)
		{
			Mobile wearer = (Mobile)parent;
			double trapFindingSkill = wearer.Skills[SkillName.DetectHidden].Value; // Replace with actual Trap Finding skill

			int hpBonus = (int)trapFindingSkill;

			int strBonus = hpBonus / 2;

			wearer.RawStr -= strBonus; // Reverse the STR bonus


		}
	}


    public TrapGloves(Serial serial) : base(serial)
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

