using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersDagger : Dagger
{
    [Constructable]
    public NecromancersDagger()
    {
        Name = "Necromancer's Dagger";
        Hue = Utility.Random(200, 800);
        MinDamage = Utility.RandomMinMax(5, 45);
        MaxDamage = Utility.RandomMinMax(45, 90);
        Attributes.SpellDamage = 15;
        Attributes.RegenMana = 5;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitCurse = 20;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersDagger(Serial serial) : base(serial)
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
