using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheGrovesFinalReach : LeafArms
{
    [Constructable]
    public TheGrovesFinalReach()
    {
        Name = "The Grove's Final Reach";
        Hue = 0x48C; // Greenish hue to match the forest theme
        BaseArmorRating = Utility.RandomMinMax(15, 50); // A lighter armor to reflect the lightness of nature

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusHits = 15;
        Attributes.RegenHits = 2;
        Attributes.DefendChance = 10;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 5;
        ArmorAttributes.SelfRepair = 5;

        SkillBonuses.SetValues(0, SkillName.Herding, 15.0); // Reflects nature’s bond with animals
        SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0); // Related to healing and caring for animals
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Tracking animals and finding your way in the wilderness

        ColdBonus = 10; // Symbolizing the chill of the forest's night
        PhysicalBonus = 5; // Increased resilience against physical damage
        PoisonBonus = 10; // Forests are full of natural poisons and toxic plants

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheGrovesFinalReach(Serial serial) : base(serial)
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
