using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersVengeance : GnarledStaff
{
    [Constructable]
    public NecromancersVengeance()
    {
        Name = "Necromancer's Vengeance";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.Fey;
        WeaponAttributes.HitHarm = 50;
        WeaponAttributes.HitLeechMana = 40;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersVengeance(Serial serial) : base(serial)
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
