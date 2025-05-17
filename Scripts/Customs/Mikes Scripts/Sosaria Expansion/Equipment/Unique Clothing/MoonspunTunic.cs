using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MoonspunTunic : Shirt
{
    [Constructable]
    public MoonspunTunic()
    {
        Name = "Moonspun Tunic";
        Hue = 1154; // A magical, ethereal blue to evoke the moonlit theme
        
        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 25;
        Attributes.BonusMana = 30;
        Attributes.RegenMana = 3;
        Attributes.LowerManaCost = 5;
        Attributes.SpellDamage = 10;
        Attributes.Luck = 50;
        Attributes.NightSight = 1; // Because it's Moonspun, gives a touch of night sight
        
        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 10;
        Resistances.Cold = 15;
        Resistances.Poison = 5;
        Resistances.Energy = 20;

        // Skill Bonuses (Thematically, we focus on magic, intelligence, and nature)
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0); // For the magical aspect
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0); // To boost intelligence-based casting
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // A calming, moonlit energy boost for meditation
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 10.0); // Ties in with spiritual connection and the moon’s mysteries
        SkillBonuses.SetValues(4, SkillName.Healing, 5.0); // Reflects the restorative aspect, like a calming night

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MoonspunTunic(Serial serial) : base(serial)
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
