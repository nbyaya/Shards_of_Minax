using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuillotineBladeDagger : Dagger
{
    [Constructable]
    public GuillotineBladeDagger()
    {
        Name = "Guillotine Blade Dagger";
        Hue = Utility.Random(100, 2200);
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(40, 80);
        Attributes.LowerManaCost = 10;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuillotineBladeDagger(Serial serial) : base(serial)
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
