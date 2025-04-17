using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheOculus : SpiritScepter
{
    [Constructable]
    public TheOculus()
    {
        Name = "The Oculus";
        Hue = Utility.Random(250, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.CastRecovery = 2;
        Attributes.SpellChanneling = 1;
        Attributes.BonusInt = 15;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitEnergyArea = 25;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheOculus(Serial serial) : base(serial)
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
