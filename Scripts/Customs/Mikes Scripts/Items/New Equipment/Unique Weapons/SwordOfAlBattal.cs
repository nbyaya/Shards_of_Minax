using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SwordOfAlBattal : Longsword
{
    [Constructable]
    public SwordOfAlBattal()
    {
        Name = "Sword of Al-Battal";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.AttackChance = 10;
        Attributes.BonusHits = 20;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.BloodDrinker = 25;
        WeaponAttributes.HitLeechHits = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SwordOfAlBattal(Serial serial) : base(serial)
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
