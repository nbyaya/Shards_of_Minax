using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IlluminaDagger : Dagger
{
    [Constructable]
    public IlluminaDagger()
    {
        Name = "Illumina Dagger";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.SpellDamage = 10;
        Attributes.Luck = 150;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitLightning = 30;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public IlluminaDagger(Serial serial) : base(serial)
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
