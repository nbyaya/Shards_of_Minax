using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KatanaOfCelestialHarmony : Katana
{
    [Constructable]
    public KatanaOfCelestialHarmony()
    {
        Name = "Katana of Celestial Harmony";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.CastSpeed = 1;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KatanaOfCelestialHarmony(Serial serial) : base(serial)
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
