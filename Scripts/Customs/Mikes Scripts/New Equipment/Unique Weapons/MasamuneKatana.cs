using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasamuneKatana : Katana
{
    [Constructable]
    public MasamuneKatana()
    {
        Name = "Masamune Katana";
        Hue = Utility.Random(700, 2900);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(65, 105);
        Attributes.AttackChance = 15;
        Attributes.BonusDex = 15;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.Terathan;
        WeaponAttributes.HitMagicArrow = 30;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasamuneKatana(Serial serial) : base(serial)
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
