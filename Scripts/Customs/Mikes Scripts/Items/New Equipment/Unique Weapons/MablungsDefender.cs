using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MablungsDefender : VeterinaryLance
{
    [Constructable]
    public MablungsDefender()
    {
        Name = "Mablung's Defender";
        Hue = Utility.Random(600, 2800);
        MinDamage = Utility.RandomMinMax(20, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.DefendChance = 10;
        Attributes.RegenMana = 3;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.Terathan;
        WeaponAttributes.HitManaDrain = 30;
        WeaponAttributes.ResistPhysicalBonus = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MablungsDefender(Serial serial) : base(serial)
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
