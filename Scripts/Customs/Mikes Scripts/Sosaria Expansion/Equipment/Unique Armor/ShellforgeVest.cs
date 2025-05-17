using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShellforgeVest : DragonTurtleHideChest
{
    [Constructable]
    public ShellforgeVest()
    {
        Name = "Shellforge Vest";
        Hue = Utility.Random(1, 1000); // Color could vary, fitting the deep greens and blues of turtle hide
        BaseArmorRating = Utility.RandomMinMax(50, 75);

        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 30;
        Attributes.RegenHits = 4;
        Attributes.DefendChance = 20;
        
        SkillBonuses.SetValues(0, SkillName.Anatomy, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(2, SkillName.Parry, 5.0);
        
        ColdBonus = 20;
        PhysicalBonus = 15;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShellforgeVest(Serial serial) : base(serial)
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
