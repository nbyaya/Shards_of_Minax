using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingWindSash : BodySash
{
    [Constructable]
    public WhisperingWindSash()
    {
        Name = "Whispering Wind Sash";
        Hue = Utility.Random(300, 2300);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusStr = 10;
        Attributes.WeaponSpeed = 5;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        Resistances.Cold = 10;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingWindSash(Serial serial) : base(serial)
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
