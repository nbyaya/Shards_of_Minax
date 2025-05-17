using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThroatOfTheRoaringFlame : TigerPeltCollar
{
    [Constructable]
    public ThroatOfTheRoaringFlame()
    {
        Name = "Throat of the Roaring Flame";
        Hue = Utility.Random(1401, 1600);  // A fiery orange hue
        BaseArmorRating = Utility.RandomMinMax(10, 30);  // Light, flexible armor

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 5;
        Attributes.DefendChance = 5;
        Attributes.CastSpeed = 1;
        Attributes.RegenStam = 2;

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);   // Enhances knowledge of animals
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);   // Improves healing for animals
        SkillBonuses.SetValues(2, SkillName.Tactics, 5.0);   // Boosts the effectiveness in combat situations

        FireBonus = 15;   // Emphasizing the fire connection with Tigers
        PhysicalBonus = 5;  // Some physical protection from the wild

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThroatOfTheRoaringFlame(Serial serial) : base(serial)
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
