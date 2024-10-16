using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Heartseeker : Dagger
{
    [Constructable]
    public Heartseeker()
    {
        Name = "Heartseeker";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 60);
        MaxDamage = Utility.RandomMinMax(150, 200);
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 20;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechHits = 45;
        WeaponAttributes.HitFireball = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Heartseeker(Serial serial) : base(serial)
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
