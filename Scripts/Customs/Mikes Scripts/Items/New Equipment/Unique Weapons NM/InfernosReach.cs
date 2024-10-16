using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernosReach : Halberd
{
    [Constructable]
    public InfernosReach()
    {
        Name = "Inferno's Reach";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.ResistFireBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernosReach(Serial serial) : base(serial)
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
