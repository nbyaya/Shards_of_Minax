using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaleplumeCrest : FeatheredHat
{
    [Constructable]
    public GaleplumeCrest()
    {
        Name = "Galeplume Crest";
        Hue = 1157; // Soft, sky-blue hue to reflect the wind and feathers

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 3;
        Attributes.RegenHits = 3;
        Attributes.RegenMana = 3;
        Attributes.DefendChance = 5;
        Attributes.Luck = 50;
        Attributes.ReflectPhysical = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tracking, 10.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(2, SkillName.Bushido, 10.0);

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaleplumeCrest(Serial serial) : base(serial)
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
