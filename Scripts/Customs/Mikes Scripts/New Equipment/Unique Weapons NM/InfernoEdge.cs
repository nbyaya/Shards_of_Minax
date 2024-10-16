using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernoEdge : TwoHandedAxe
{
    [Constructable]
    public InfernoEdge()
    {
        Name = "Inferno Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 25;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.OrcSlaying;
        WeaponAttributes.HitFireArea = 55;
        WeaponAttributes.DurabilityBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernoEdge(Serial serial) : base(serial)
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
