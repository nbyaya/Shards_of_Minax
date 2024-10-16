using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EbonyWarAxeOfVampires : WarAxe
{
    [Constructable]
    public EbonyWarAxeOfVampires()
    {
        Name = "Ebony WarAxe of Vampires";
        Hue = Utility.Random(800, 950);
        MinDamage = Utility.RandomMinMax(30, 65);
        MaxDamage = Utility.RandomMinMax(65, 100);
        Attributes.BonusStr = 15;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.BloodDrinking;
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EbonyWarAxeOfVampires(Serial serial) : base(serial)
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
